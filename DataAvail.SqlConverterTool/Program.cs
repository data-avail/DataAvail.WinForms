using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.SqlConverter.SqlServer;
using System.Xml.Linq;
using DataAvail.SqlConverter.XmlFormatter;

namespace DataAvail.SqlConverterTool
{
    class Program
    {
        #region Patterns

        const string connectionStringFormat = @"Data Source=BAIO-PC\BAIO;Initial Catalog={0};Integrated Security=True";

        const string projPath = @"C:\DataAvail\DataAvail.ConsumerApps";

        static string ConnectionString { get { return string.Format(connectionStringFormat, dbName); } }

        static string ModelFileName { get { return string.Format(@"{0}\{1}\GenModel\Model.xml", projPath, projName); } }

        static string ViewFileName { get { return string.Format(@"{0}\{1}\GenModel\View.xml", projPath, projName); } }

        static string SecurityFileName { get { return string.Format(@"{0}\{1}\GenModel\Security.xml", projPath, projName); } }

        #endregion        

        const string dbName = "Company";

        const string projName = "OficialAgency";

        static void Main(string[] args)
        {
            SqlServerDb serverDb = new DataAvail.SqlConverter.SqlServer.SqlServerDb(ConnectionString);

            DbExtensions.Options = new CustomOptionsA();

            XElement xmlLinq = serverDb.GetXOPElement("DataSet");

            xmlLinq.Save(ModelFileName);

            DbExtensions.Options = new CustomOptionsB();

            xmlLinq = serverDb.GetXOPElement("DataView");

            xmlLinq.Save(ViewFileName);
        }

    }
}
