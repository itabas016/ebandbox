using FrameMobile.Common;
using FrameMobile.Model.News;
using SubSonic.DataProviders;
using SubSonic.Query;
using SubSonic.SqlGeneration.Schema;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Xunit;

namespace FrameMobile.UnitTests.Domain.Service
{
    public class TestSelect
    {
        [Fact(Skip = "MySQLInsert")]
        public void a()
        {
            var conn = System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionStrings.NEWS_MYSQL_CONNECTSTRING].ConnectionString;

            var provider = ProviderFactory.GetProvider(conn, "MySql.Data.MySqlClient");

            var query = new QueryCommand("select name as name2 from newssubcategory", provider);
            var reader = provider.ExecuteReader(query);

            var ret = new List<test_model>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    ret.Add(MapPost(reader));
                }
            }
            ret.ForEach(s => Console.WriteLine(s.Name333));
        }

        public test_model MapPost(DbDataReader reader)
        {
            test_model item = new test_model();

            item.Name333 = reader.GetString(reader.GetOrdinal("name2"));

            return item;
        }
    }

    public class test_model
    {
        public string Name333 { get; set; }
    }
}
