using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookRentalApp
{
    public partial class SearchMemForm : MetroForm
    {
        public SearchMemForm()
        {
            InitializeComponent();
        }

        private void GrdMember_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SearchMemForm_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Commons.CONNECTIONSTRING))
            {
                conn.Open();
                string strQuery = "SELECT Idx, Names, Levels, Addr, Mobile, Email FROM membertbl ";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(strQuery, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "membertbl");

                GrdMember.DataSource = ds;
                GrdMember.DataMember = "membertbl";
            }
        }

        private void BtnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
