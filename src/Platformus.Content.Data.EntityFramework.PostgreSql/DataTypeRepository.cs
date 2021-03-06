﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.EntityFramework.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Platformus.Content.Data.Abstractions;
using Platformus.Content.Data.Models;

namespace Platformus.Content.Data.EntityFramework.PostgreSql
{
  public class DataTypeRepository : RepositoryBase<DataType>, IDataTypeRepository
  {
    public DataType WithKey(int id)
    {
      return this.dbSet.FirstOrDefault(dt => dt.Id == id);
    }

    public IEnumerable<DataType> All()
    {
      return this.dbSet.OrderBy(dt => dt.Position);
    }

    public IEnumerable<DataType> Range(string orderBy, string direction, int skip, int take)
    {
      return this.dbSet.OrderBy(dt => dt.Position).Skip(skip).Take(take);
    }

    public void Create(DataType dataType)
    {
      this.dbSet.Add(dataType);
    }

    public void Edit(DataType dataType)
    {
      this.storageContext.Entry(dataType).State = EntityState.Modified;
    }

    public void Delete(int id)
    {
      this.Delete(this.WithKey(id));
    }

    public void Delete(DataType dataType)
    {
      this.storageContext.Database.ExecuteSqlCommand(
        @"
          DELETE FROM ""CachedObjects"" WHERE ""ClassId"" IN (SELECT ""ClassId"" FROM ""Members"" WHERE ""PropertyDataTypeId"" = {0});
          CREATE TEMP TABLE ""TempDictionaries"" (""Id"" INT PRIMARY KEY);
          INSERT INTO ""TempDictionaries"" SELECT ""HtmlId"" FROM ""Properties"" WHERE ""MemberId"" IN (SELECT Id FROM ""Members"" WHERE ""PropertyDataTypeId"" = {0});
          DELETE FROM ""Properties"" WHERE ""MemberId"" IN (SELECT ""Id"" FROM ""Members"" WHERE ""PropertyDataTypeId"" = {0});
          DELETE FROM ""Localizations"" WHERE ""DictionaryId"" IN (SELECT ""Id"" FROM ""TempDictionaries"");
          DELETE FROM ""Dictionaries"" WHERE ""Id"" IN (SELECT ""Id"" FROM ""TempDictionaries"");
          DELETE FROM ""Members"" WHERE ""PropertyDataTypeId"" = {0};
        ",
        dataType.Id
      );

      this.dbSet.Remove(dataType);
    }

    public int Count()
    {
      return this.dbSet.Count();
    }
  }
}