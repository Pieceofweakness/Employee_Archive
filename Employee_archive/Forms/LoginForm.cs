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
    public partial class LoginForm : Form
    {
        private DatabaseHelper db;
        public LoginForm()
        {
            InitializeComponent();
            db = new DatabaseHelper();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.ToString();
            string password = txtPassword.Text.ToString();

            var admin = db.Authenticate(login, password);
            if(admin != null)
            {
                MessageBox.Show($"ВХОД УСПЕШЕН " +
                    $"Добро пожаловать:{admin.Full_Name}");
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.ShowDialog();
                this.Close();
            }


        }
    }
}
