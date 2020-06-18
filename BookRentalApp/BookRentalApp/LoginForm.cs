using MetroFramework.Forms;
using MetroFramework;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace BookRentalApp
{
    public partial class LoginForm : MetroFramework.Forms.MetroForm
    {
        string strConn = "Data Source=127.0.0.1;Initial Catalog=BookRentalshopDB;Persist Security Info=True;User ID=sa;Password=p@ssw0rd!";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                LoginProcess();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginProcess();
        }

        private void LoginProcess()
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                return;
            }

            string resultId = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT userID FROM userTBL WHERE userID = @userID AND password = @password";
                    SqlParameter paramUserId = new SqlParameter("@userID", SqlDbType.NVarChar, 12);
                    paramUserId.Value = (textBox1.Text);
                    cmd.Parameters.Add(paramUserId);
                    SqlParameter paramPassword = new SqlParameter("@password", SqlDbType.VarChar);
                    paramPassword.Value = textBox2.Text;
                    cmd.Parameters.Add(paramPassword);

                    SqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();
                    resultId = rdr["userID"] != null ? rdr["userID"].ToString() : string.Empty;  // 사용자 아이디가 있으면 아이디를 없으면 "" 입력
                }
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, "오류가 발생했습니다", "오류");
                textBox1.Text = textBox2.Text = string.Empty;
                textBox1.Focus();
                return;
            }
            

            if (resultId == "") // 로그인 아이디가 없으면
            {
                MetroMessageBox.Show(this, "아이디나 비밀번호를 정확히 입력하세요", "로그인 오류");
                textBox1.Text = textBox2.Text = string.Empty;
                textBox1.Focus();
                return;
            }
            else
            {
                this.Close();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                textBox2.Focus();
            }
        }
    }
}
