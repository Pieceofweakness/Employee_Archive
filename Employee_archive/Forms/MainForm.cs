using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee_archive.Forms
{
    public partial class MainForm : Form
    {
        private DatabaseHelper db;
        public MainForm()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            LoadEmployees();
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        public void LoadEmployees()
        {
            var employees = db.GetAllEmployees();

            var display = employees.Select(c => new
            {
                ID = c.ID_employee,
                Имя = c.Full_Name,
                Дата_Рождения = c.Born_date,
                Телефон = c.Phone,
                Роль = c.RoleName,
                Отработано_Дней = c.Work_days
            }).ToList();

            dgvEmployees.DataSource = display;

            ConfigureSizeCollumns();
        }

        public void ConfigureSizeCollumns()
        {
            dgvEmployees.Columns["ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEmployees.Columns["Имя"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEmployees.Columns["Дата_Рождения"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEmployees.Columns["Телефон"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEmployees.Columns["Роль"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvEmployees.Columns["Отработано_Дней"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Close();
        }
    }
}
