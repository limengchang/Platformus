﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Platformus.Barebone;
using Platformus.Content.Data.Abstractions;
using Platformus.Content.Data.Models;
using Platformus.Globalization.Data.Abstractions;
using Platformus.Globalization.Data.Models;

namespace Platformus.Content
{
  public class ObjectManager
  {
    private IHandler handler;

    public ObjectManager(IHandler handler)
    {
      this.handler = handler;
    }

    public IEnumerable<string> GetDisplayProperties(Object @object)
    {
      List<string> properties = new List<string>();

      foreach (Member member in this.handler.Storage.GetRepository<IMemberRepository>().FilteredByClassId(@object.ClassId))
      {
        if (member.DisplayInList == true)
        {
          Property property = this.handler.Storage.GetRepository<IPropertyRepository>().WithObjectIdAndMemberId(@object.Id, member.Id);

          if (property == null)
            properties.Add(string.Empty);

          else
          {
            Localization localization = this.handler.Storage.GetRepository<ILocalizationRepository>().WithDictionaryIdAndCultureId(property.HtmlId, 1);

            if (localization == null)
              properties.Add(string.Empty);

            else properties.Add(localization.Value);
          }
        }
      }

      return properties;
    }
  }
}