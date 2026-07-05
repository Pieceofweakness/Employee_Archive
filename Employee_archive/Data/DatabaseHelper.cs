using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace Employee_archive
{
    public class DatabaseHelper: IDatabaseHelper
    {
        private readonly string connectionString = $"Host=localhost;Port=5432;Database=Employee_archive;Username=postgres;Password=1234";

        public DatabaseHelper()
        {
            
        }


        private IDbConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public DataTable QueryTable(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var result = conn.Query(sql, param);
                DataTable table = new DataTable();

                if (result.Any())
                {
                    var firstRow = (IDictionary<string, object>)result.First();
                    foreach (var prop in firstRow.Keys)
                        table.Columns.Add(prop);

                    foreach (IDictionary<string, object> row in result)
                        table.Rows.Add(row.Values.ToArray());
                }
                return table;
            }
        }
        public int Execute(string sql, object param = null)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                return conn.Execute(sql, param);
            }
        }






    }
}
