using System;
using System.Windows.Forms;

namespace BookRentalApp
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        int mdiID = 1;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.ShowDialog();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
        }

        private void MnuItemCodeMng_Click(object sender, EventArgs e)
        {
            DivForm divForm = new DivForm();
            divForm.MdiParent = this;
            divForm.Text = "구분코드 관리";
            divForm.Dock = DockStyle.Fill;
            divForm.Show();
            divForm.WindowState = FormWindowState.Maximized;
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (MetroFramework.MetroMessageBox.Show(this, "종료하겠습니까?", "종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
                == DialogResult.Yes)
            {
                e.Cancel = false;
                Environment.Exit(0);
            } else
            {
                e.Cancel = true;
            }
        }
    }
}
