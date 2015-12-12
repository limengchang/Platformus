﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using ExtCore.Data.Abstractions;
using ExtCore.Data.Models.Abstractions;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ModelBinding;
using Platformus.Globalization.Backend.ViewModels;
using Platformus.Globalization.Data.Abstractions;
using Platformus.Globalization.Data.Models;

namespace Platformus.Globalization.Backend.Controllers
{
  public abstract class ControllerBase : Platformus.Barebone.Backend.Controllers.ControllerBase
  {
    public ControllerBase(IStorage storage)
      : base(storage)
    {
    }

    public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
    {
      this.HandleViewModelMultilingualProperties(actionExecutingContext);
      base.OnActionExecuting(actionExecutingContext);
    }

    protected void CreateOrEditEntityLocalizations(IEntity entity)
    {
      foreach (PropertyInfo propertyInfo in this.GetDictionaryPropertiesFromEntity(entity))
      {
        Dictionary dictionary = this.GetOrCreateDictionaryForProperty(entity, propertyInfo);

        this.DeleteLocalizations(dictionary);
        this.CreateLocalizations(propertyInfo, dictionary);
      }
    }

    private IEnumerable<PropertyInfo> GetDictionaryPropertiesFromEntity(IEntity entity)
    {
      return entity.GetType().GetProperties().Where(pi => pi.PropertyType == typeof(Dictionary));
    }

    private Dictionary GetOrCreateDictionaryForProperty(IEntity entity, PropertyInfo propertyInfo)
    {
      PropertyInfo dictionaryIdPropertyInfo = entity.GetType().GetProperty(propertyInfo.Name + "Id");
      int dictionaryId = (int)dictionaryIdPropertyInfo.GetValue(entity);
      Dictionary dictionary = null;

      if (dictionaryId == 0)
      {
        dictionary = new Dictionary();
        this.Storage.GetRepository<IDictionaryRepository>().Create(dictionary);
        this.Storage.Save();
        dictionaryIdPropertyInfo.SetValue(entity, dictionary.Id);
      }

      else dictionary = this.Storage.GetRepository<IDictionaryRepository>().WithKey(dictionaryId);

      return dictionary;
    }

    private void DeleteLocalizations(Dictionary dictionary)
    {
      foreach (Localization localization in this.Storage.GetRepository<ILocalizationRepository>().FilteredByDictionaryId(dictionary.Id))
        this.Storage.GetRepository<ILocalizationRepository>().Delete(localization);

      this.Storage.Save();
    }

    private void CreateLocalizations(PropertyInfo propertyInfo, Dictionary dictionary)
    {
      IEnumerable<Culture> cultures = this.Storage.GetRepository<ICultureRepository>().All();

      foreach (Culture culture in cultures)
      {
        Localization localization = new Localization();

        localization.DictionaryId = dictionary.Id;
        localization.CultureId = culture.Id;

        string identity = propertyInfo.Name + culture.Code;
        string value = this.Request.Form[identity];

        localization.Value = value;
        this.Storage.GetRepository<ILocalizationRepository>().Create(localization);
      }

      this.Storage.Save();
    }

    private void HandleViewModelMultilingualProperties(ActionExecutingContext actionExecutingContext)
    {
      ViewModelBase viewModel = this.GetViewModelFromActionExecutingContext(actionExecutingContext);

      if (viewModel == null)
        return;

      try
      {
        IEnumerable<Culture> cultures = this.Storage.GetRepository<ICultureRepository>().All();

        foreach (PropertyInfo propertyInfo in this.GetMultilingualPropertiesFromViewModel(viewModel))
        {
          this.ModelState.Remove(propertyInfo.Name);

          bool hasRequiredAttribute = propertyInfo.CustomAttributes.Any(ca => ca.AttributeType == typeof(RequiredAttribute));

          foreach (Culture culture in cultures)
          {
            string identity = propertyInfo.Name + culture.Code;
            string value = this.Request.Form[identity];

            ModelStateEntry modelState = new ModelStateEntry();

            if (hasRequiredAttribute && string.IsNullOrEmpty(value))
              this.ModelState.Add(identity, this.CreateInvalidModelState(value));

            else this.ModelState.Add(identity, this.CreateValidModelState(value));
          }
        }
      }

      catch { }
    }

    private ViewModelBase GetViewModelFromActionExecutingContext(ActionExecutingContext actionExecutingContext)
    {
      if (!actionExecutingContext.ActionArguments.ContainsKey("createOrEdit"))
        return null;

      return actionExecutingContext.ActionArguments["createOrEdit"] as ViewModelBase;
    }

    private IEnumerable<PropertyInfo> GetMultilingualPropertiesFromViewModel(ViewModelBase viewModel)
    {
      return viewModel.GetType().GetProperties().Where(pi => pi.CustomAttributes.Any(ca => ca.AttributeType == typeof(MultilingualAttribute)));
    }

    private ModelStateEntry CreateValidModelState(string value)
    {
      ModelStateEntry modelState = new ModelStateEntry();

      modelState.ValidationState = ModelValidationState.Valid;
      modelState.AttemptedValue = value;
      return modelState;
    }

    private ModelStateEntry CreateInvalidModelState(string value)
    {
      ModelStateEntry modelState = this.CreateValidModelState(value);

      modelState.Errors.Add(string.Empty);
      modelState.ValidationState = ModelValidationState.Invalid;
      return modelState;
    }
  }
}