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
    public partial class AddNewEmployeeForm : Form
    {

        private DatabaseHelper db;
        public AddNewEmployeeForm()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            LoadRoles();
        }

        //Загрузка ролей
        public void LoadRoles()
        {
            var roles = db.GetAllRoles();

            cmbRole.DataSource = roles;
            cmbRole.DisplayMember = "Name_Role";
            cmbRole.ValueMember = "ID_Role";
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxFIO.Text))
            {
                MessageBox.Show("Введите ФИО сотрудника");
                return;
            }
            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Выберите должность");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtBoxBornDate.Text))
            {
                MessageBox.Show("Введите дату рождения");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtBoxPhone.Text))
            {
                MessageBox.Show("Введите номер телефона");
                return;
            }
            if((int)numDaysWork.Value <= 0)
            {
                MessageBox.Show("Введите количество отработанных дней");
                return;
            }
            string fio = txtBoxFIO.Text.Trim();
            string bornDate = txtBoxBornDate.Text.Trim();
            string phone = txtBoxPhone.Text.Trim();
            int selectedRoleId = (int)cmbRole.SelectedValue;
            int workDays = (int)numDaysWork.Value;

            var employee = new Employee
            {
                Full_Name = fio,
                Born_date = bornDate,
                Phone = phone,
                Role = selectedRoleId,
                Work_days = workDays
                
            };

            try
            {
                bool result = db.AddEmployee(employee);
                if(result)
                MessageBox.Show($"Новый сотрудник {employee.Full_Name} успешно добавлен");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Не удалось добавить сотрудника: ошибка - {ex.Message}");
            }

            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
            this.Close();
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
            this.Close();
        }
    }
}
