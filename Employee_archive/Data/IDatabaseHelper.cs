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

        DataTable QueryTable(string sql, object param = null);

        int Execute(string sql, object param = null);


    }
}
