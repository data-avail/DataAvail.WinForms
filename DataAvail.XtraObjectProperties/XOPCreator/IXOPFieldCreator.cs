using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator
{
    public interface IXOPFieldCreator
    {
        XtraFieldProperties CreateFkField(DataAvail.XtraObjectProperties.XtraFieldProperties XtraFieldProperties, FKFieldDescriptor FKFieldDescriptor);
    }

    public class FKFieldDescriptor
    {
        public FKFieldDescriptor(string Filter)
        {
            this.Filter = Filter;
        }

        public readonly string Filter;
    }

    public class FKFieldDescriptorTable : FKFieldDescriptor
    {
        public FKFieldDescriptorTable(string ParentTable, string ParentColumn, string ChildTable, string ChildColumn, string Filter)
            : base(Filter)
        {
            parentTable = ParentTable;

            parentColumn = ParentColumn;

            childTable = ChildTable;

            childColumn = ChildColumn;
        }

        public readonly string parentTable;

        public readonly string parentColumn;

        public readonly string childTable;

        public readonly string childColumn;
    }

    public class FKFieldDescriptorDictionary : FKFieldDescriptor
    {
        public FKFieldDescriptorDictionary(Dictionary<string, string> ValueNameDictionary, string Filter) 
            : base(Filter)
        {
            valueNameDictionary = ValueNameDictionary;
        }

        public readonly Dictionary<string, string> valueNameDictionary;
    }
}
