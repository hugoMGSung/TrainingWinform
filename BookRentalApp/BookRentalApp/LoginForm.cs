using MetroFramework.Forms;
using MetroFramework;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace BookRentalApp
{
    public partial class LoginForm : MetroFramework.Forms.MetroForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                LoginProcess();
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            LoginProcess();
        }

        private void LoginProcess()
        {
            if (string.IsNullOrEmpty(TxtUserID.Text) || string.IsNullOrEmpty(TxtPassword.Text))
            {
                return;
            }

            string resultId = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(Commons.CONNECTIONSTRING))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT userID FROM userTBL WHERE userID = @userID AND password = @password";
                    SqlParameter paramUserId = new SqlParameter("@userID", SqlDbType.NVarChar, 12);
                    paramUserId.Value = (TxtUserID.Text);
                    cmd.Parameters.Add(paramUserId);
                    SqlParameter paramPassword = new SqlParameter("@password", SqlDbType.VarChar);
                    string strTemp = TxtPassword.Text;
                    var mdHash = MD5.Create();
                    var passResult = Commons.GetMd5Hash(mdHash, strTemp);

                    paramPassword.Value = passResult;
                    cmd.Parameters.Add(paramPassword);

                    SqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    resultId = rdr["userID"] != null ? rdr["userID"].ToString() : string.Empty;  // 사용자 아이디가 있으면 아이디를 없으면 "" 입력
                }
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, "오류가 발생했습니다", "오류");
                TxtUserID.Text = TxtPassword.Text = string.Empty;
                TxtUserID.Focus();
                return;
            }
            

            if (resultId == "") // 로그인 아이디가 없으면
            {
                MetroMessageBox.Show(this, "아이디나 비밀번호를 정확히 입력하세요", "로그인 오류");
                TxtUserID.Text = TxtPassword.Text = string.Empty;
                TxtUserID.Focus();
                return;
            }
            else
            {
                Commons.USERID = resultId;
                this.Close();
            }
        }

        private void TxtUserID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                TxtPassword.Focus();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
