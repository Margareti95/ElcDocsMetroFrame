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
    public partial class frmMain : MetroFramework.Forms.MetroForm
    {
        string connectionString = @"Data Source=.;Initial Catalog=elcLibrary;User Id = sa; Password = 123456";
        //string connectionString = @"Data Source=GRISHINAAN\SQLEXPRESS;Initial Catalog=elcLibrary;User Id = rpck; Password = 123456";
        int docsId = 0;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            GridFill();
            Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conSql = new SqlConnection(connectionString))
            {
                conSql.Open();
                SqlCommand sqlCom = new SqlCommand("elcDocsAddOrEdit", conSql);
                sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCom.Parameters.AddWithValue("@_id_Docs", docsId);
                sqlCom.Parameters.AddWithValue("@_Name_Docs", txtName.Text.Trim());
                sqlCom.Parameters.AddWithValue("@_Category_Docs", txtCategory.Text.Trim());
                sqlCom.Parameters.AddWithValue("@_Author_Docs", txtAuthor.Text.Trim());
                sqlCom.Parameters.AddWithValue("@_Date_Docs", txtDate.Text.Trim());
                sqlCom.ExecuteNonQuery();
                MetroFramework.MetroMessageBox.Show(this, "Данные успешно внесены!", "Корректировка данных");
                conSql.Close();
                GridFill();
                Clear();
            }
        }

        void GridFill() // Метод, выводящий данные из базы в DataGridView
        {
            using (SqlConnection conSql = new SqlConnection(connectionString))
            {
                conSql.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("elcDocsViewAll", conSql);
                sqlDa.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                DataTable dtblDocs = new DataTable();
                sqlDa.Fill(dtblDocs);
                dgvDocs.DataSource = dtblDocs;
                dgvDocs.Columns[0].Visible = false;
                conSql.Close();
            }
        }

        void Clear()
        {
            txtName.Text = txtAuthor.Text = txtCategory.Text = txtDate.Text = "";
            docsId = 0;
            btnSave.Text = "Сохранить";
            btnDelete.Enabled = false;
        }

        private void dgvDocs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dgvDocs.CurrentRow.Index != -1)
            {
                txtName.Text = dgvDocs.CurrentRow.Cells[1].Value.ToString();
                txtCategory.Text = dgvDocs.CurrentRow.Cells[2].Value.ToString();
                txtAuthor.Text = dgvDocs.CurrentRow.Cells[3].Value.ToString();
                txtDate.Text = dgvDocs.CurrentRow.Cells[4].Value.ToString();
                docsId = Convert.ToInt32(dgvDocs.CurrentRow.Cells[0].Value.ToString());
                btnSave.Text = "Обновить";
                btnDelete.Enabled = Enabled;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection conSql = new SqlConnection(connectionString))
            {
                conSql.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("elcDocsSearchByValue", conSql);
                sqlDa.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("_SearchValue", txtSearch.Text);
                DataTable dtblDocs = new DataTable();
                sqlDa.Fill(dtblDocs);
                dgvDocs.DataSource = dtblDocs;
                dgvDocs.Columns[0].Visible = false;
                txtSearch.Text = "";
                conSql.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection conSql = new SqlConnection(connectionString))
            {
                conSql.Open();
                if (MetroFramework.MetroMessageBox.Show(this, "Вы уверены, что хотите удалить выбранную запись?", "Удаление", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand sqlCom = new SqlCommand("elcDocsDeleteByID", conSql);
                    sqlCom.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCom.Parameters.AddWithValue("@_id_Docs", docsId);
                    sqlCom.ExecuteNonQuery();
                    MetroFramework.MetroMessageBox.Show(this, "Данные успешно удалены!", "Удаление данных");

                }
                conSql.Close();
                GridFill();
                Clear();
            }
        }
    }
}
