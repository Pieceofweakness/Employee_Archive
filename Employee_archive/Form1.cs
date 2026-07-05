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
    public partial class Form1 : Form
    {
        private DatabaseHelper dbHelper;
        public Form1()
        {
            dbHelper = new DatabaseHelper();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool isConn = dbHelper.TestConnection();
            if (isConn)
            {
                MessageBox.Show("TOOOP");
            }
            else
            {
                MessageBox.Show("НЕ РАБОТАЕТ");
            }


        }
    }
}
