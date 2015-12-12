﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Platformus.Barebone.Backend.ViewModels.Shared
{
  public class BackendMenuViewModel : ViewModelBase
  {
    public IEnumerable<BackendMenuGroupViewModel> BackendMenuGroups { get; set; }
  }
}