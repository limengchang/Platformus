﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Platformus.Barebone.Backend.ViewModels.Shared
{
  public class BackendMenuGroupViewModel : ViewModelBase
  {
    public string Name { get; set; }
    public int Position { get; set; }
    public IEnumerable<BackendMenuItemViewModel> BackendMenuItems { get; set; }
  }
}