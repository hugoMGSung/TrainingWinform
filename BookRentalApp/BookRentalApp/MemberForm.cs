using MetroFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookRentalApp
{
    public partial class MemberForm : MetroFramework.Forms.MetroForm
    {
        string strConn = "Data Source=127.0.0.1;Initial Catalog=bookrentalshop;Persist Security Info=True;User ID=sa;Password=mssql_p@ssw0rd!";
        string mode = "";

        public MemberForm()
        {
            InitializeComponent();
        }

        private void DivForm_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'bookrentalshopDataSet.divtbl' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            //this.divtblTableAdapter.Fill(this.bookrentalshopDataSet.divtbl);
            UpdateData();
        }

        private void UpdateData()
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                string strQuery = "SELECT Idx, Names, Levels, Addr, Mobile, Email FROM membertbl ";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(strQuery, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "membertbl");

                GrdDivTbl.DataSource = ds;
                GrdDivTbl.DataMember = "membertbl";
            }


            SetColumnHeaders();
            mode = "";
        }

        private void SetColumnHeaders()
        {
            DataGridViewColumn column = GrdDivTbl.Columns[0];
            column.Width = 40;
            column.HeaderText = "순번";
            column = GrdDivTbl.Columns[1];
            column.Width = 110;
            column.HeaderText = "이름";
            column = GrdDivTbl.Columns[2];
            column.Width = 60;
            column.HeaderText = "등급";
            column = GrdDivTbl.Columns[3];
            column.Width = 120;
            column.HeaderText = "주소";
            column = GrdDivTbl.Columns[4];
            column.Width = 100;
            column.HeaderText = "핸드폰";
            column = GrdDivTbl.Columns[5];
            column.Width = 170;
            column.HeaderText = "이메일";
        }

        private void UpdateProcess()
        {
            if (string.IsNullOrEmpty(TxtIdx.Text) || string.IsNullOrEmpty(TxtNames.Text))
            {
                MetroMessageBox.Show(this, "빈값은 넣을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    if (mode == "UPDATE")
                    {
                        cmd.CommandText = "UPDATE divtbl SET Names = @names WHERE Division = @division";
                    }
                    else if (mode == "INSERT")
                    {
                        cmd.CommandText = "INSERT INTO divtbl VALUES (@division, @names)";
                    }

                    SqlParameter paramName = new SqlParameter("@names", SqlDbType.NVarChar, 45);
                    paramName.Value = (TxtNames.Text);
                    cmd.Parameters.Add(paramName);
                    SqlParameter paramDiv = new SqlParameter("@division", SqlDbType.VarChar);
                    paramDiv.Value = TxtIdx.Text;
                    cmd.Parameters.Add(paramDiv);
                    

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MetroMessageBox.Show(this, "오류가 발생했습니다", "오류");
                }
                finally
                {
                    UpdateData();
                }
            }
        }

        private void TxtDivision_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                TxtNames.Focus();
            }
        }

        private void TxtNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                BtnSave_Click(sender, new EventArgs());
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (mode != "UPDATE")
            {
                MetroMessageBox.Show(this, "삭제할 데이터를 선택하세요", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                DeleteProcess();
            }
        }

        private void DeleteProcess()
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE divtbl WHERE Division = @division";
                    SqlParameter paramDiv = new SqlParameter("@division", SqlDbType.VarChar);
                    paramDiv.Value = TxtIdx.Text;
                    cmd.Parameters.Add(paramDiv);


                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MetroMessageBox.Show(this, "오류가 발생했습니다", "오류");
                }
                finally
                {
                    UpdateData();
                }
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            TxtIdx.Text = TxtNames.Text = string.Empty;
            TxtIdx.ReadOnly = false;
            mode = "INSERT";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            UpdateProcess();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            TxtIdx.Text = TxtNames.Text = string.Empty;
            TxtIdx.ReadOnly = false;
        }

        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow data = GrdDivTbl.Rows[e.RowIndex];
                TxtIdx.Text = data.Cells[0].Value.ToString();
                TxtIdx.ReadOnly = true;
                TxtNames.Text = data.Cells[1].Value.ToString();
                CboLevels.Text = data.Cells[2].Value.ToString();
                TxtAddr.Text = data.Cells[3].Value.ToString();
                TxtMobile.Text = data.Cells[4].Value.ToString();
                TxtEmail.Text = data.Cells[5].Value.ToString();
                mode = "UPDATE";
            }
        }

        private void MemberForm_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel2_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel3_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel4_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel5_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel6_Click(object sender, EventArgs e)
        {

        }
    }
}
