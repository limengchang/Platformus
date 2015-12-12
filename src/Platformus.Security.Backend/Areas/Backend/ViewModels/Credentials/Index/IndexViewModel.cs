﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Platformus.Barebone.Backend.ViewModels;
using Platformus.Barebone.Backend.ViewModels.Shared;

namespace Platformus.Security.Backend.ViewModels.Credentials
{
  public class IndexViewModel : ViewModelBase
  {
    public int UserId { get; set; }
    public GridViewModel Grid { get; set; }
  }
}