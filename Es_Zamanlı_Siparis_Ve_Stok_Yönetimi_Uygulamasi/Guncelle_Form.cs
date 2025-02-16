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
    public partial class Guncelle_Form : Form
    {
        private int urunID; 
        private Form1 anaForm;
        public Guncelle_Form(int id, string urunAdi, string urunFiyati, string urunStoku, Form1 form)
        {
            urunID = id;
            anaForm = form;
            InitializeComponent();

            textBox1.Text = urunAdi; 
            textBox2.Text = urunStoku; 
            textBox3.Text = urunFiyati;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string yeniAd = textBox1.Text;
            string yeniFiyat = textBox3.Text.Replace(',', '.'); 
            string yeniStok = textBox2.Text.Replace(',', '.');

            string query = $"UPDATE products SET productname = '{yeniAd}', price = '{yeniFiyat}', stock = '{yeniStok}' WHERE productid = {urunID}";
            bool success = DataBaseHelper.ExecuteNonQuery(query);

            if (success)
            {
                MessageBox.Show("Ürün başarıyla güncellendi.");
                anaForm.VerileriYenile();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ürün güncellenirken bir hata oluştu.");
            }

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '0')
            {
                e.Handled = true; 
                MessageBox.Show("Lütfen geçerli bir sayı giriniz.");
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
