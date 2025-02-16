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
    public partial class AdminGiris : Form
    {
        private Form1 anaForm;
        public AdminGiris(Form1 form)
        {
            InitializeComponent();
            anaForm = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "1234")
            {
                anaForm.AdminGirisBasarili(); // Doğru şifre girilirse ana form üzerindeki işlemi gerçekleştir
                this.Close(); 
            }
            else 
            { 
                MessageBox.Show("Yanlış şifre!"); 
            }
        }
    }
}
