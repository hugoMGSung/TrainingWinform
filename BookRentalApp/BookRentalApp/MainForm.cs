﻿using System;
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
            DivForm form = new DivForm();
            form.MdiParent = this;
            form.Text = "구분코드관리";
            form.Dock = DockStyle.Fill;

            try
            {
                form.Show();
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception)
            {
            }
            
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

        private void MnuItemMemerMng_Click(object sender, EventArgs e)
        {
            try
            {
                MemberForm form = new MemberForm();
                form.MdiParent = this;
                form.Text = "회원관리";
                form.Dock = DockStyle.Fill;
                form.Show();
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception)
            {
            }
            
        }

        private void MnuItemRentalMng_Click(object sender, EventArgs e)
        {
            try
            {
                RentalForm form = new RentalForm();
                form.MdiParent = this;
                form.Text = "대여관리";
                form.Dock = DockStyle.Fill;
                form.Show();
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception)
            {
            }
            
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            LblDisplayUserID.Text = Commons.USERID;
        }

        private void a(object sender, EventArgs e)
        {

        }

        private void MnuItemLogin_Click(object sender, EventArgs e)
        {
            try
            {
                UserForm form = new UserForm();
                form.MdiParent = this;
                form.Text = "사용자관리";
                form.Dock = DockStyle.Fill;
                form.Show();
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception)
            {
            }
        }
    }
}
