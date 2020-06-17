using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookRentalApp
{
    public partial class DivForm : MetroFramework.Forms.MetroForm
    {
        public DivForm()
        {
            InitializeComponent();
        }

        private void DivForm_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'bookrentalshopDataSet.divtbl' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.divtblTableAdapter.Fill(this.bookrentalshopDataSet.divtbl);

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow data = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = data.Cells[0].Value.ToString();
                textBox2.Text = data.Cells[1].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = string.Empty;
        }
    }
}
