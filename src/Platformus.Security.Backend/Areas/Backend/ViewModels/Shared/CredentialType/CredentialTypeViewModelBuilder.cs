﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Platformus.Barebone;
using Platformus.Barebone.Backend.ViewModels;
using Platformus.Security.Data.Models;

namespace Platformus.Security.Backend.ViewModels.Shared
{
  public class CredentialTypeViewModelBuilder : ViewModelBuilderBase
  {
    public CredentialTypeViewModelBuilder(IHandler handler)
      : base(handler)
    {
    }

    public CredentialTypeViewModel Build(CredentialType credentialType)
    {
      return new CredentialTypeViewModel()
      {
        Id = credentialType.Id,
        Name = credentialType.Name,
        Position = credentialType.Position
      };
    }
  }
}