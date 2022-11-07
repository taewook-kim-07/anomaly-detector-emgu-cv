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
        private int pagesize = 3;

        private void ShowWorkList(ref database Database, int page, int page_size, ref DataGridView datagridview, ref Label label, ref TextBox textbox)
        {
            int count = Database.Select(ref datagridview, page - 1, page_size);

            label.Text = $"마지막 조회 시간 ({DateTime.Now})\n{count}개";
            textbox.Text = page.ToString();
        }

        public Form_DataViewer(ref database parentdb)
        {
            InitializeComponent();
            this.Text = $"({DateTime.Now})";
            Database = parentdb;
            ShowWorkList(ref Database, 1, pagesize, ref dataGridView1, ref label1, ref textBox1);
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
                ShowWorkList(ref Database, Convert.ToInt32(textBox1.Text), pagesize, ref dataGridView1, ref label1, ref textBox1);
            }
        }
    }
}
