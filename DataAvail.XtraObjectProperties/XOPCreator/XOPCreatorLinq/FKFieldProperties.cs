﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorLinq
{
 
    public class FKFieldProperties : DataAvail.XtraObjectProperties.XOPCreator.XOPCreatorCustomType.FKFieldProperties
    {
        public FKFieldProperties(object DataContext, PropertyInfo PropertyInfo, XtraFieldProperties XtraFieldProperties)
            : base(DataContext, PropertyInfo, XtraFieldProperties)
        {
        }
    }
}
