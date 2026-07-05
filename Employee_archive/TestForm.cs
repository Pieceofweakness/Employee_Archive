using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee_archive
{
    public partial class TestForm : Form
    {
        private DatabaseHelper db;
        public TestForm()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            Load();
        }



        public void Load()
        {
            var display = db.GetAllEmployees();

            dataGridView1.DataSource = display;
        }

    }
}
