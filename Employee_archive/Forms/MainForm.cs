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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddNewEmployeeForm addNewEmployeeForm = new AddNewEmployeeForm();
            addNewEmployeeForm.ShowDialog();
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedIdEmployee = (int)dgvEmployees.CurrentRow.Cells["ID"].Value;

            var employee = new Employee
            {
                ID_employee = selectedIdEmployee
            };

            DialogResult result = MessageBox.Show(
            $"Вы уверены, что хотите удалить сотрудника?\n\n" +
            $"ФИО: {dgvEmployees.CurrentRow.Cells["Имя"].Value}\n" +
            $"Должность: {dgvEmployees.CurrentRow.Cells["Роль"].Value}",
            "Подтверждение удаления",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool res = db.DeleteEmployee(employee);

                if (res)
                {
                    MessageBox.Show("УДАЛЕН");
                }
                else
                {
                    MessageBox.Show("Не получилось");
                }
            }

            LoadEmployees();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(dgvEmployees.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите работника");
                return;
            }

            DataGridViewRow selectedRow = dgvEmployees.SelectedRows.Count > 0 ? dgvEmployees.SelectedRows[0] : dgvEmployees.CurrentRow;

            int employeeId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
            string fullName = selectedRow.Cells["Имя"].Value.ToString();
            string bornDate = selectedRow.Cells["Дата_Рождения"].Value.ToString();
            string phone = selectedRow.Cells["Телефон"].Value.ToString();
            string roleName = selectedRow.Cells["Роль"].Value.ToString();
            int workDays = Convert.ToInt32(selectedRow.Cells["Отработано_Дней"].Value);


            var roles = db.GetAllRoles();
            var role = roles.FirstOrDefault(r => r.Name_Role == roleName);
            int roleId = role.ID_Role;

            var employee = new Employee
            {
                ID_employee = employeeId,
                Full_Name = fullName,
                Born_date = bornDate,
                Phone = phone,
                Role = roleId,
                RoleName = roleName,
                Work_days = workDays
            };

            this.Hide();
            EditEmployeeForm editEmployeeForm = new EditEmployeeForm(employee);
            editEmployeeForm.ShowDialog();
            this.Close();


        }
    }
}
