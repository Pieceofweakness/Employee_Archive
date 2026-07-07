using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO;

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

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Сохранить Excel файл";
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.FileName = $"Сотрудники_{DateTime.Now:dd-MM-yyyy}";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    ExportToExcel(saveFileDialog.FileName);
                    MessageBox.Show($"Данные успешно испортированы в - {saveFileDialog.FileName}");
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Ошибка - {ex.Message}");
                }
            }
        }


        public void ExportToExcel(string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Сотрудники");
                var employees = db.GetAllEmployees();

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "ФИО";
                worksheet.Cell(1, 3).Value = "Дата рождения";
                worksheet.Cell(1, 4).Value = "Телефон";
                worksheet.Cell(1, 5).Value = "Роль";
                worksheet.Cell(1, 6).Value = "Отработано дней";

                
                int row = 2;
                foreach (var emp in employees)
                {
                    worksheet.Cell(row, 1).Value = emp.ID_employee;
                    worksheet.Cell(row, 2).Value = emp.Full_Name;
                    worksheet.Cell(row, 3).Value = emp.Born_date;
                    worksheet.Cell(row, 4).Value = emp.Phone;
                    worksheet.Cell(row, 5).Value = emp.RoleName ?? "Без роли";
                    worksheet.Cell(row, 6).Value = emp.Work_days;
                    row++;
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }
        }
    }
}
