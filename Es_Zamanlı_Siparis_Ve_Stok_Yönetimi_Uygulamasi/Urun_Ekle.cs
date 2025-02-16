using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Es_Zamanlı_Siparis_Ve_Stok_Yönetimi_Uygulamasi
{
    public partial class Urun_Ekle : Form
    {
        private Form1 anaForm;
        public Urun_Ekle(Form1 form)
        {
            InitializeComponent();
            anaForm = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string urunAdi = textBox1.Text;
            string urunFiyati = textBox3.Text.Replace(',', '.'); 
            string urunStoku = textBox2.Text.Replace(',', '.');

            // Ürünü veritabanına ekle
            string query = $"INSERT INTO products (productName, price, stock) VALUES ('{urunAdi}', '{urunFiyati}', '{urunStoku}')";
            bool success = DataBaseHelper.ExecuteNonQuery(query);

            if (success)
            {
                MessageBox.Show("Ürün başarıyla eklendi.");
                anaForm.VerileriYenile(); 
                this.Close();
            }
            else
            {
                MessageBox.Show("Ürün eklenirken bir hata oluştu.");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
                MessageBox.Show("Lütfen geçerli bir sayı giriniz.");
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
                MessageBox.Show("Lütfen geçerli bir sayı giriniz.");
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }

            if (char.IsDigit(e.KeyChar))
            {
                string[] parts = (sender as TextBox).Text.Split(',');
                if (parts.Length > 1 && parts[1].Length >= 2)
                {
                    e.Handled = true;
                    MessageBox.Show("Sadece iki ondalık basamağa izin verilir.");
                }
            }
        }
    }
}
