using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data
{
    public class DevArtDataTableStubManager : DataAvail.Data.DbContext.DbContextDataAdapter.IDataTableStubManager
    {
        internal static readonly DataAvail.Data.DbContext.DbContextDataAdapter.IDataTableStubManager StubManager = new DevArtDataTableStubManager();

        public DataAvail.Data.DbContext.DbContextDataAdapter.IDataTableStub GetDataTableStub(System.Data.DataTable DataTable)
        {
            if (DataTable is Devart.Common.DbDataTable)
                return new DevArtDataTableStub((Devart.Common.DbDataTable)DataTable);

            throw new Exception("Can't find suitable DataTableStub class");

        }
    }
}
