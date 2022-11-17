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

using AnomalyDetector.utils;

namespace AnomalyDetector
{
    public partial class Form_DataViewer : Form
    {
        database Database;
        private int pagesize = 30;

        private void ShowWorkList(ref database Database, int page, int page_size, ref DataGridView datagridview, ref TextBox textbox)
        {
            int count = Database.Select(ref datagridview, page - 1, page_size, checkBox1.Checked);            

            this.Text = $"마지막 조회 시간 ({DateTime.Now})\n{count}개";
            textbox.Text = page.ToString();
        }

        public Form_DataViewer(ref database parentdb)
        {
            InitializeComponent();

            Database = parentdb;
            ShowWorkList(ref Database, 1, pagesize, ref dataGridView1, ref textBox1);
        }

        private void Form_DataViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<Form> closeForm = new List<Form>();
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Form_IMG_Viewer")
                {
                    closeForm.Add(frm);
                }
            }
            foreach (var frm in closeForm)
            {
                frm.Close();
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = this.dataGridView1.Rows[e.RowIndex];
                Form_IMG_Viewer form = new Form_IMG_Viewer(ref Database, row.Cells["id"].Value.ToString());
                form.Show();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                ShowWorkList(ref Database, Convert.ToInt32(textBox1.Text), pagesize, ref dataGridView1, ref textBox1);
            }
        }

        private void button_prev_Click(object sender, EventArgs e)
        {
            int now_page = Convert.ToInt32(textBox1.Text);
            if (now_page <= 1)
                now_page = 2;

            ShowWorkList(ref Database, now_page - 1, pagesize, ref dataGridView1, ref textBox1);
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            ShowWorkList(ref Database, Convert.ToInt32(textBox1.Text) + 1, pagesize, ref dataGridView1, ref textBox1);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ShowWorkList(ref Database, Convert.ToInt32(textBox1.Text), pagesize, ref dataGridView1, ref textBox1);
        }
    }
}
