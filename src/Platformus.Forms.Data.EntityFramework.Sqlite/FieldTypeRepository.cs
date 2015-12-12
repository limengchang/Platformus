﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework.Sqlite;
using Platformus.Forms.Data.Abstractions;
using Platformus.Forms.Data.Models;

namespace Platformus.Forms.Data.EntityFramework.Sqlite
{
  public class FieldTypeRepository : RepositoryBase<FieldType>, IFieldTypeRepository
  {
    public FieldType WithKey(int id)
    {
      return this.dbSet.FirstOrDefault(ft => ft.Id == id);
    }

    public IEnumerable<FieldType> All()
    {
      return this.dbSet.OrderBy(ft => ft.Position);
    }
  }
}