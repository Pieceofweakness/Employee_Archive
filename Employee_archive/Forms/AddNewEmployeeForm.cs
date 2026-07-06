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
        public void LoadRoles()
        {
            var roles = db.GetAllRoles();

            cmbRole.DataSource = roles;
            cmbRole.DisplayMember = "Name_Role";
            cmbRole.ValueMember = "ID_Role";
        }



        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string fio = txtBoxFIO.Text.Trim();
            string bornDate = txtBoxBornDate.Text.Trim();
            string phone = txtBoxPhone.Text.Trim();
            int selectedRoleId = (int)cmbRole.SelectedValue;
            int workDays =Convert.ToInt32( txtBoxWorkDays.Text.Trim());

            var employee = new Employee
            {
                Full_Name = fio,
                Born_date = bornDate,
                Phone = phone,
                Role = selectedRoleId,
                Work_days = workDays
                
            };

            bool result = db.AddEmployee(employee);
            if (result)
            {
                MessageBox.Show($"Новый сотрудник {fio} успешно добавлен");
            }
            else
            {
                MessageBox.Show("Не удалось добавить сотудника");
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
