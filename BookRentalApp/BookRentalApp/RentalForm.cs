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
    public partial class RentalForm : MetroFramework.Forms.MetroForm
    {
        string mode = "";

        public RentalForm()
        {
            InitializeComponent();
        }

        private void DivForm_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'bookrentalshopDataSet.divtbl' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            //this.divtblTableAdapter.Fill(this.bookrentalshopDataSet.divtbl);
            UpdateData();
            UpdateCombo();
        }

        private void UpdateCombo()
        {
            // 회원번호
            using (SqlConnection conn = new SqlConnection(Commons.CONNECTIONSTRING))
            {
                conn.Open();
                string strQuery = "SELECT Idx, Names FROM dbo.membertbl ";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Dictionary<string, string> temps = new Dictionary<string, string>();
                while (reader.Read())
                {
                    temps.Add(reader[0].ToString(), reader[1].ToString());
                }
                CboMember.DataSource = new BindingSource(temps, null);
                CboMember.DisplayMember = "Value";
                CboMember.ValueMember = "Key";
            }
        }

        private void UpdateData()
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONNECTIONSTRING))
            {
                conn.Open();
                string strQuery = "SELECT r.idx AS '대여번호', m.Names AS '대여회원', " +
                                  "       t.Names AS '장르', " +
                                  "       b.Names AS '대여책제목', b.ISBN, " +
                                  "       r.rentalDate AS '대여일' " +
                                  "  FROM rentaltbl AS r " +
                                  " INNER JOIN membertbl AS m " +
                                  "    ON r.memberIdx = m.Idx " +
                                  " INNER JOIN bookstbl AS b " +
                                  "    ON r.bookIdx = b.Idx " +
                                  " INNER JOIN divtbl AS t " +
                                  "    ON b.division = t.division ";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(strQuery, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "rentaltbl");

                GrdRentalTbl.DataSource = ds;
                GrdRentalTbl.DataMember = "rentaltbl";
            }

            mode = "";
        }

        private void UpdateProcess()
        {
            //if (string.IsNullOrEmpty(TxtDivision.Text) || string.IsNullOrEmpty(TxtNames.Text))
            //{
            //    MetroMessageBox.Show(this, "빈값은 넣을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            using (SqlConnection conn = new SqlConnection(Commons.CONNECTIONSTRING))
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

                    //SqlParameter paramName = new SqlParameter("@names", SqlDbType.NVarChar, 45);
                    //paramName.Value = (TxtNames.Text);
                    //cmd.Parameters.Add(paramName);
                    //SqlParameter paramDiv = new SqlParameter("@division", SqlDbType.VarChar);
                    //paramDiv.Value = TxtDivision.Text;
                    //cmd.Parameters.Add(paramDiv);
                    

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
            using (SqlConnection conn = new SqlConnection(Commons.CONNECTIONSTRING))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE divtbl WHERE Division = @division";


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
            mode = "INSERT";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            UpdateProcess();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
        }

        private void GrdDivTbl_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow data = GrdRentalTbl.Rows[e.RowIndex];
                mode = "UPDATE";
            }
        }

        private void RentalForm_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchMemForm form = new SearchMemForm();
            if (form.ShowDialog() == DialogResult.Yes)
            {
                MetroMessageBox.Show(this, "YES");
            }
        }
    }
}
