using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_archive
{
    public interface IDatabaseHelper
    {
        //Подключение
        bool TestConnection();


        //Запросы
        int Execute(string sql, object param = null);


        //Авторизация
        Administrator Authenticate(string login, string password);

        //CRUD над сотрудниками
        List<Employee> GetAllEmployees();
        bool AddEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);

        //Роли
        List<Role> GetAllRoles();

        //Статистика
        int GetTotalEmployeesCount();
        double GetAverageWorkDays();




    }
}
