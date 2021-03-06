﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAvail.SqlConverter
{
    public interface IColumn
    {
        ITable Table { get; }

        string Name { get; }

        DbType DbType { get; }

        int Size { get; }

        string DefaultValue { get; }

        bool IsNullable { get; }

        bool IsPk { get; }

        bool IsPkAutoGenerated { get; }
    }
}
