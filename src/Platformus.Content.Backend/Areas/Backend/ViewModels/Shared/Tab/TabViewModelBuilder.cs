﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Platformus.Barebone;
using Platformus.Content.Data.Models;
using Platformus.Globalization.Backend.ViewModels;

namespace Platformus.Content.Backend.ViewModels.Shared
{
  public class TabViewModelBuilder : ViewModelBuilderBase
  {
    public TabViewModelBuilder(IHandler handler)
      : base(handler)
    {
    }

    public TabViewModel Build(Tab tab)
    {
      return new TabViewModel()
      {
        Id = tab.Id,
        Name = tab.Name,
        Position = tab.Position
      };
    }
  }
}