using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;

namespace elcDocsMF
{
    public partial class frmLogin : MetroFramework.Forms.MetroForm
    {
        string connectionString = @"Data Source=.;Initial Catalog=elcLibrary;User Id = sa; Password = 123456";
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcon = new SqlConnection(connectionString);
            string query = "SELECT*FROM Users WHERE Login = '" + txtLogin.Text.Trim() + "' AND Pass = '" + txtPass.Text.Trim() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            if (dtbl.Rows.Count == 1)
            {
                frmMain frmM = new frmMain();
                this.Hide();
                frmM.Show();
            }

            else
            {
                MetroFramework.MetroMessageBox.Show(this,"Введите корректный логин или пароль и повторите попытку входа!", "Сообщение");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
