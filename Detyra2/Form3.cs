﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serveri;

namespace Detyra
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            MessageBox.Show(SessionManager.user.username);

        }

        private void addBillsBtn_Click(object sender, EventArgs e)
        {
            Hide();
            Form4 addBillsForm = new Form4();
            addBillsForm.ShowDialog();
            addBillsForm.Dispose();
            addBillsForm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Client c1 = new Client();
            string mesazhi = "merrfaturat";
            c1.ClientSend(mesazhi);
            DataTable table = new DataTable();
            table.Columns.Add("Lloji", typeof(string));
            table.Columns.Add("Viti", typeof(int));
            table.Columns.Add("Muaji", typeof(int));
            table.Columns.Add("Cmimi", typeof(double));
            dataGridView1.DataSource = table;
            string pergjijga = c1.DekriptoPergjigjen();
            List<Fatura> userFaturat = merrFaturat(pergjijga);
            if (userFaturat.Count != 0) {
                foreach (Fatura f in userFaturat) {
                    table.Rows.Add(f.llojiFatures, f.viti, f.muaji, f.vleraEuro);
                }
            }
        }
        private List<Fatura> merrFaturat(string response)
        {
            List<Fatura> userBills = new List<Fatura>();
            string[] arr = response.Split('?');
            for (int i = 0; i < arr.Length-1; i++)
            {
                string[] fatura = arr[i].Split('*');
                string lloji = fatura[0];
                int viti = Int32.Parse(fatura[1]);
                int muaji = Int32.Parse(fatura[2]);
                double cmimi = Double.Parse(fatura[3]);
                Fatura f = new Fatura(lloji, viti, muaji, cmimi);
                userBills.Add(f);
            }
            return userBills;
        }

    }

}