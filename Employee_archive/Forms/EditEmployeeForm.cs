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
    public partial class EditEmployeeForm : Form
    {
        private DatabaseHelper db;
        public Employee CurrentEmployee;
        public List<Role> Roles;

        public EditEmployeeForm(Employee employee)
        {
            InitializeComponent();
            db = new DatabaseHelper();
            CurrentEmployee = employee;
            LoadRoles();
            LoadEmployeeData();
            
        }

        //Загрузка ролей
        public void LoadRoles()
        {
            Roles = db.GetAllRoles();

            cmbRole.DataSource = Roles;
            cmbRole.DisplayMember = "Name_Role";
            cmbRole.ValueMember = "ID_Role";
        }

        //Загрузка данных сотрудника
        public void LoadEmployeeData()
        {
            txtBoxFIO.Text = CurrentEmployee.Full_Name;
            txtBoxBornDate.Text = CurrentEmployee.Born_date;
            txtBoxPhone.Text = CurrentEmployee.Phone;
            numWorkDays.Value = CurrentEmployee.Work_days;

            var role = Roles.Find(r => r.ID_Role == CurrentEmployee.Role);
            if (role != null)
            {
                cmbRole.SelectedItem = role;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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
            if ((int)numWorkDays.Value <= 0)
            {
                MessageBox.Show("Введите количество отработанных дней");
                return;
            }
            string fio = txtBoxFIO.Text.Trim();
            string bornDate = txtBoxBornDate.Text.Trim();
            string phone = txtBoxPhone.Text.Trim();
            int selectedRoleId = (int)cmbRole.SelectedValue;
            int workDays = (int)numWorkDays.Value;
            CurrentEmployee.Full_Name = fio;
            CurrentEmployee.Born_date = bornDate;
            CurrentEmployee.Phone = phone;
            CurrentEmployee.Role = selectedRoleId;
            CurrentEmployee.Work_days= workDays;

            try
            {
                bool result = db.UpdateEmployee(CurrentEmployee);
                if (result)
                {
                    MessageBox.Show($"Данные сотрудника {CurrentEmployee.Full_Name} успешно изменены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось изменить данные сотрудника: ошибка - {ex.Message}");
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
