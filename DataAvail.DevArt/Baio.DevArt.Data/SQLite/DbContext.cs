using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DevArt.Data.SQLite
{
    public class DbContext : DataAvail.Data.DbContext.SQLiteDbContext
    {
        private readonly DataAvail.Data.DbContext.IDbContextObjectCreator _objectCreator = new DbContextObjectCreator();

        public override DataAvail.Data.DbContext.IDbContextObjectCreator ObjectCreator
        {
            get
            {
                return _objectCreator;
            }
        }

        public override DataAvail.Data.DbContext.IDbContextDataAdapter DataAdapter
        {
            get
            {
                return new DataAvail.Data.DbContext.DbContextDataAdapterAsync(DevArtDataTableStubManager.StubManager);
            }
        }
    }
}
