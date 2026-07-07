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

        //Запросы и подключение
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



        //Авторизация
        public Administrator Authenticate(string login, string password)
        {
            using(var conn = GetConnection())
            {
                conn.Open();
                var admin = conn.QueryFirstOrDefault<Administrator>(
                    "SELECT * FROM administrators WHERE Login = @login and Password = @password",
                    new { login, password });

                if (admin != null)
                {
                    return new Administrator
                    {
                        ID_Admin = admin.ID_Admin,
                        Full_Name = admin.Full_Name
                    };
                }
                return null;
            }
        }



        //CRUD над работниками
        public List<Employee> GetAllEmployees()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT e.*, r.Name_Role as RoleName
                               FROM employees e
                               left join Roles r ON e.Role = r.ID_Role";

                return conn.Query<Employee>(sql).ToList();
            }
        }

        public bool AddEmployee(Employee employee)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"
                        INSERT INTO employees(Full_Name,Born_date,Phone,Role,Work_days) 
                        VALUES (@Full_Name, @Born_date, @Phone, @Role, @Work_days)";
                return conn.Execute(sql, employee) > 0;
            }
        }

        public bool UpdateEmployee(Employee employee)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE employees SET
                        Full_Name = @Full_Name,
	                    Born_date = @Born_date,
	                    Phone = @Phone,
	                    Role = @Role,
	                    Work_days = @Work_days
                    WHERE ID_employee = @ID_employee";
                return conn.Execute(sql, employee) > 0;
            }
        }

        public bool DeleteEmployee(Employee employee)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string sql = @"
                        DELETE FROM employees
                        WHERE ID_employee = @ID_employee";
                return conn.Execute(sql, employee) > 0;
            }
        }

        
        //Роли

        public List<Role> GetAllRoles()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Roles";

                return conn.Query<Role>(sql).ToList();
            }
        }
        

    }
}
