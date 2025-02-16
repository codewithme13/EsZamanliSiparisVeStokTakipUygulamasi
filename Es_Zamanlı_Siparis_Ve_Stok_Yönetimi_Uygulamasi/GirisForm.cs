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
    public partial class GirisForm : Form
    {
        private Form1 anaForm;
        public bool customersGenerated = false;

        public GirisForm()
        {
            InitializeComponent();
            LoadMusteri();

        }
        public GirisForm(Form1 form)
        {
            InitializeComponent();
            LoadMusteri();
            anaForm = form;

        }
        private void LoadMusteri()
        {
            string query = "SELECT customername FROM customers";
            DataTable dt = DataBaseHelper.ExecuteQuery(query);

            if (dt != null)
            {
                UserBox.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    UserBox.Items.Add(row["customername"].ToString());
                }
            }
            else
            {
                MessageBox.Show("Müşteri verileri yüklenemedi.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox1.UseSystemPasswordChar = false;
            }
            else
            {
                textBox1.UseSystemPasswordChar = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Şifre kontrolü
            if (textBox1.Text == "1234")
            {
                // Form1'i aç ve TabControl index 0'ı göster
                Form1 form1 = new Form1();
                form1.isLoggedIn = true;
                form1.Show();
                form1.ActivateTabIndex(0);
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı şifre! Lütfen tekrar deneyin.");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (UserBox.Items.Count == 0)
            {
                MessageBox.Show("Lütfen müşteri seçin.");
                return; // Eğer ComboBox boşsa, işlemi sonlandır
            }

            // ComboBox'tan seçilen müşteri adı
            string selectedCustomerName = UserBox.SelectedItem.ToString();
            if (textBox2.Text == "1234")
            {
                // Form1'i aç ve TabControl index 0'ı göster
                Form1 form1 = new Form1();
                form1.isLoggedIn = false;
                form1.SelectedCustomerName = selectedCustomerName; // Seçilen müşteri adını aktar
                form1.Show();
                form1.ActivateTabIndex(1);
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı şifre! Lütfen tekrar deneyin.");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1) 
            {
                GenerateCustomers(); 
            }
        }
        public void GenerateCustomers()
        {
            if (customersGenerated)
                return; // Eğer müşteriler zaten oluşturulmuşsa, tekrar oluşturma

            Random rand = new Random();
            int customerCount = rand.Next(5, 11);
            int premiumCount = 0;

            string maxIdQuery = "SELECT MAX(customerid) FROM customers";
            object result = DataBaseHelper.ExecuteScalar(maxIdQuery);
            int currentMaxId = result != DBNull.Value ? Convert.ToInt32(result) : 0;

            for (int i = 1; i <= customerCount; i++)
            {
                string customerName = $"Customer{currentMaxId + i}";
                decimal budget = rand.Next(500, 3001);
                string customerType = (premiumCount < 2 || rand.NextDouble() > 0.5) ? "Premium" : "Standard";

                if (customerType == "Premium")
                {
                    premiumCount++;
                }

                decimal totalSpent = 0;

                string insertQuery = $@"
            INSERT INTO customers (CustomerName, Budget, CustomerType, TotalSpent)
            VALUES ('{customerName}', {budget}, '{customerType}', {totalSpent})";

                DataBaseHelper.ExecuteNonQuery(insertQuery);
            }

            customersGenerated = true; // Müşteriler başarıyla oluşturuldu
            LoadMusteri(); // Yeni müşteriler oluşturulduktan sonra ComboBox'ı güncelle
            MessageBox.Show("Müşteriler başarıyla oluşturuldu.");

        }

    }
}
