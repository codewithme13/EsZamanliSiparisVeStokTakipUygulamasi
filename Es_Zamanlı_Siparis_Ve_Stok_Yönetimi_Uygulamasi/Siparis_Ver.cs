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
    public partial class Siparis_Ver : Form
    {
        private string selectedCustomerName; // Formun içinde kullanılacak customername
        private Form1 mainForm;
        public Siparis_Ver(string customerName, Form1 mainForm)
        {
            InitializeComponent();
            this.selectedCustomerName = customerName;
            this.mainForm = mainForm; 
            LoadProducts();
            InitializeComboBox();
        }
        private void InitializeComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add(selectedCustomerName); 
            comboBox1.SelectedIndex = 0; 
        }
        private void LoadProducts()
        {
            string query = "SELECT productname FROM products";
            DataTable dt = DataBaseHelper.ExecuteQuery(query);

            if (dt != null)
            {
                comboBox2.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    comboBox2.Items.Add(row["productname"].ToString());
                }
            }
            else
            {
                MessageBox.Show("Ürün verileri yüklenemedi.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text;

            if (!string.IsNullOrEmpty(text) && !int.TryParse(text, out int result))
            {
                MessageBox.Show("Lütfen geçerli bir pozitif tam sayı girin.");
                textBox1.Text = "";
            }
            else if (int.TryParse(text, out result) && (result < 0 || result > 5))
            {
                MessageBox.Show("Lütfen 0 ile 5 arasında bir tam sayı girin.");
                textBox1.Text = "";
            }
        }

    private void button1_Click(object sender, EventArgs e)
    {
        string selectedProductName = comboBox2.SelectedItem?.ToString();
        string quantityText = textBox1.Text;

        if (string.IsNullOrEmpty(selectedProductName) || string.IsNullOrEmpty(quantityText))
        {
            MessageBox.Show("Lütfen tüm alanları doldurun.");
            return;
        }

        if (!int.TryParse(quantityText, out int quantity) || quantity <= 0)
        {
            MessageBox.Show("Geçerli bir miktar girin.");
            return;
        }

        string customerQuery = $"SELECT customerid,budget, customertype FROM customers WHERE customername = '{selectedCustomerName}'";
        DataTable customerData = DataBaseHelper.ExecuteQuery(customerQuery);
        if (customerData == null || customerData.Rows.Count == 0)
        {
            MessageBox.Show("Müşteri bulunamadı.");
            return;
        }
            DateTime orderDate = DateTime.Now;
            int customerId = Convert.ToInt32(customerData.Rows[0]["customerid"]);
        string customerType = customerData.Rows[0]["customertype"].ToString();
            decimal customerBudget = Convert.ToDecimal(customerData.Rows[0]["budget"]);

            if (customerType == "Standard")
        {
            customerType = "Standart";
        }

        string productQuery = $"SELECT productid, price, stock FROM products WHERE productname = '{selectedProductName}'";
        DataTable productData = DataBaseHelper.ExecuteQuery(productQuery);
        if (productData == null || productData.Rows.Count == 0)
        {
            MessageBox.Show("Ürün bulunamadı.");
            return;
        }
            int productId = Convert.ToInt32(productData.Rows[0]["productid"]);
        decimal productPrice = Convert.ToDecimal(productData.Rows[0]["price"]);
        int stockQuantity = Convert.ToInt32(productData.Rows[0]["stock"]);
        int orderId = DataBaseHelper.GetOrderId(customerId, productId, orderDate);
            decimal totalPrice = productPrice * quantity;

            if (stockQuantity <= 0)
        {
            MessageBox.Show("Stokta yeterli miktar bulunmamaktadır.");
                DataBaseHelper.Kaydet(customerId, productId, quantity, totalPrice, orderDate, "Hata");
                int orderId1 = DataBaseHelper.GetOrderId(customerId, productId, orderDate);
                DataBaseHelper.AddLog(customerId, orderId1, DateTime.Now, "Hata", "Ürün stoğu yetersiz", customerType);
                mainForm.DisplayLogs(selectedCustomerName, customerType, selectedProductName, quantity, "Ürün stoğu yetersiz");
                return;
        }

        if (quantity > stockQuantity)
        {
            MessageBox.Show("Sipariş vermek istediğiniz miktar, stok miktarından fazla.");
                DataBaseHelper.Kaydet(customerId, productId, quantity, totalPrice, orderDate, "Hata");
                int orderId2 = DataBaseHelper.GetOrderId(customerId, productId, orderDate);
                DataBaseHelper.AddLog(customerId, orderId2, DateTime.Now, "Hata", "Ürün stoğu yetersiz", customerType);
                mainForm.DisplayLogs(selectedCustomerName, customerType, selectedProductName, quantity, "Ürün stoğu yetersiz");
                return;
        }
        if (totalPrice > customerBudget)
            {
                MessageBox.Show("Müşterinin bütçesi yetersiz.");
                DataBaseHelper.Kaydet(customerId, productId, quantity, totalPrice, orderDate, "Hata");
                int orderId3 = DataBaseHelper.GetOrderId(customerId, productId, orderDate);
                DataBaseHelper.AddLog(customerId, orderId3, DateTime.Now, "Hata", "Bütçe yetersiz", customerType);
                mainForm.DisplayLogs(selectedCustomerName, customerType, selectedProductName, quantity, "Bütçe yetersiz");
                return;
        }

            string orderStatus = "bekliyor";
            bool success = DataBaseHelper.Kaydet(customerId, productId, quantity, totalPrice, orderDate, orderStatus);

        if (success)
        {
            OrderTracker.StartWaiting(customerId);
            MessageBox.Show("Sipariş başarıyla verildi.");
                int orderId4 = DataBaseHelper.GetOrderId(customerId, productId, orderDate);
                DataBaseHelper.AddLog(customerId, orderId4, DateTime.Now, "Bilgilendirme", "Satın alma isteği başarılı", customerType);
                mainForm.DisplayLogs(selectedCustomerName, customerType, selectedProductName, quantity, "Satın alma isteği başarılı");

                comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox1.Clear();
        }
        else
        {
            MessageBox.Show("Sipariş verilirken bir sorun oluştu.");
                int orderId5 = DataBaseHelper.GetOrderId(customerId, productId, orderDate);
                DataBaseHelper.AddLog(customerId, orderId5, DateTime.Now, "Hata", "Veritabanı Hatası", customerType);
                mainForm.DisplayLogs(selectedCustomerName, customerType, selectedProductName, quantity, "Sipariş verilirken bir sorun oluştu.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.Show();
        }
    }
}
