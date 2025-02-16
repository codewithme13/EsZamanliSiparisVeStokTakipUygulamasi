using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using LiveCharts.Defaults;
using System.IO;

namespace Es_Zamanlı_Siparis_Ve_Stok_Yönetimi_Uygulamasi
{
    public partial class Form1 : Form
    {
        private bool adminAuthenticated = false;
        public bool isLoggedIn = false;
        private int previousTabIndex = -1;
        public string SelectedCustomerName { get; set; }

       
        public Form1()
        {
            InitializeComponent();
            LoadPieChart();
            SetupButtons();
            LoadLogsFromFile();
            tabControl1.SelectedIndex = 4;
            tabControl1.TabPages[0].Enabled = false; 
          

        }
        static int productId = 10;  
        static int quantity = 10;  

        private void LoadLogsFromFile()
        {
            string logFilePath = @"C:\Users\umut\source\repos\Es_Zamanlı_Siparis_Ve_Stok_Yönetimi_Uygulamasi\logs.txt";
            if (File.Exists(logFilePath))
            {
                try
                {
                    string[] logs = File.ReadAllLines(logFilePath);
                    foreach (string log in logs)
                    {
                        string logDetails = log.Substring(log.LastIndexOf(":") + 2); 
                        string formattedLog = log;

                        if (logDetails.Contains("Satın alma başarılı"))
                        {
                            LogTextBox.SelectionColor = Color.Green;
                        }
                        else if (logDetails.Contains("Satın alma işlemi onaylandı. Başarılı"))
                        {
                            LogTextBox.SelectionColor = Color.Green;
                        }                    
                        else if (logDetails.Contains("Satın alma isteği başarılı"))
                        {
                            LogTextBox.SelectionColor = Color.Blue;
                        }
                        else if (logDetails.Contains("Ürün stoğu yetersiz"))
                        {
                            LogTextBox.SelectionColor = Color.Red;
                        }
                        else if (logDetails.Contains("Bütçe yetersiz"))
                        {
                            LogTextBox.SelectionColor = Color.Red;
                        }
                        else if (logDetails.Contains("İşlem iptal edildi.Ürün stoğu yetersiz"))
                        {
                            LogTextBox.SelectionColor = Color.Red;
                        }
                        else if (logDetails.Contains("İşlem iptal edildi.Stok yetersiz"))
                        {
                            LogTextBox.SelectionColor = Color.Red;
                        }
                        else if (logDetails.Contains("İşlem iptal edildi.Bütçe yetersiz"))
                        {
                            LogTextBox.SelectionColor = Color.Red;
                        }
                        
                        else
                        {
                            LogTextBox.SelectionColor = LogTextBox.ForeColor;
                        }

                        LogTextBox.AppendText(formattedLog + "\n");

                        LogTextBox.SelectionColor = LogTextBox.ForeColor;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loglar yüklenirken bir hata oluştu: " + ex.Message);
                }
            }
        }
        //stok güncellenirken diğer işlmeleri askıya alma
        public static void ProcessStop()
        {

            Console.WriteLine("Stok güncellenmeye başlandı.");
            DataBaseHelper.UpdateStock(productId, quantity);

            Console.WriteLine("Sipariş işlemine başlanıyor...");
            DataBaseHelper.PlaceOrder(productId, quantity);

            System.Threading.Thread.Sleep(6000);
            
        }
        public void DisplayLogs(string customerName, string customerType, string productName, int quantity, string logDetails)
        {
            string formattedLog;

            if (logDetails == "Satın alma başarılı")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın aldı. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Green;
            }
            else if (logDetails == "Satın alma işlemi onaylandı. Başarılı")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın aldı. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Green;
            }
            else if (logDetails == "İşlem iptal edildi. Stok yetersiz")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın almak istedi. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Red;
            }
            else if (logDetails == "İşlem iptal edildi.Bütçe yetersiz")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın almak istedi. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Red;
            }
            else if (logDetails == "Satın alma isteği başarılı")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın almak istiyor. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Blue;
            }

            else if (logDetails == "Ürün stoğu yetersiz")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın almak istedi. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Red;
            }
            else if (logDetails == "Bütçe yetersiz")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın almak istedi. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Red;
            }
            else if (logDetails == "İşlem iptal edildi.Stok yetersiz")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın almak istedi. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Red;
            }
            else if (logDetails == "İşlem iptal edildi.Ürün stoğu yetersiz")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın almak istedi. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Red;
            }
            else if (logDetails == "İşlem iptal edildi.Bütçe yetersiz")
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet satın almak istedi. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = Color.Red;
            }
            else
            {
                formattedLog = $"{customerName} ({customerType}), {productName}'den {quantity} adet ile ilgili işlem yaptı. {logDetails}";
                LogTextBox.SelectionStart = LogTextBox.TextLength;
                LogTextBox.SelectionLength = 0;
                LogTextBox.SelectionColor = LogTextBox.ForeColor;
            }

            LogTextBox.AppendText(formattedLog + "\n");

            LogTextBox.SelectionColor = LogTextBox.ForeColor;
            LogTextBox.SelectionStart = LogTextBox.TextLength;
            LogTextBox.ScrollToCaret();
            SaveLogToFile(formattedLog);

        }

        private void SaveLogToFile(string log)
        {
            string logFilePath = @"C:\Users\umut\source\repos\Es_Zamanlı_Siparis_Ve_Stok_Yönetimi_Uygulamasi\logs.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {log}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Log kaydedilirken bir hata oluştu: " + ex.Message);
            }
        }

        private void LoadPieChart()
        {
            var pieChart = new LiveCharts.WinForms.PieChart();

            pieChart.Width = 337; 
            pieChart.Height = 338; 

            pieChart.Location = new Point(67, 20); 
            pieChart.Dock = DockStyle.None;

            TabPage stockPage = tabControl1.TabPages[2];
            stockPage.Controls.Add(pieChart);

            string query = "SELECT productname, stock FROM products";
            DataTable productData = DataBaseHelper.ExecuteQuery(query);

            if (productData != null && productData.Rows.Count > 0)
            {
                var seriesCollection = new SeriesCollection();
                List<string> criticalProducts = new List<string>();

                foreach (DataRow row in productData.Rows)
                {
                    string productName = row["productname"].ToString();
                    int stock = Convert.ToInt32(row["stock"]);

                    if (stock <= 20)
                    {
                        criticalProducts.Add($"{productName} (Stok: {stock})");
                    }

                    seriesCollection.Add(new PieSeries
                    {
                        Title = productName,
                        Values = new ChartValues<double> { stock },
                        DataLabels = true,
                        LabelPoint = chartPoint => $"{chartPoint.Y}"
                    });
                }

                pieChart.Series = seriesCollection;
                pieChart.LegendLocation = LegendLocation.Right;

                if (criticalProducts.Count > 0)
                {
                    Label criticalLabel = new Label
                    {
                        AutoSize = true,
                        Location = new Point(67, 370), 
                        Text = "Kritik Seviyedeki Ürünler:\n" + string.Join("\n", criticalProducts),
                        ForeColor = Color.Red 
                    };
                    stockPage.Controls.Add(criticalLabel);
                }
            }
            else
            {
                MessageBox.Show("Veri çekilemedi veya veri bulunamadı.");
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (!adminAuthenticated)
                {
                    AdminGiris adminForm = new AdminGiris(this);
                    adminForm.ShowDialog();
                    VerileriYenile();
                    if (!adminAuthenticated)
                    {
                        tabControl1.SelectedIndex = 4;
                    }
                }
            }
            else
            {
                adminAuthenticated = false;
            }
            if (tabControl1.SelectedIndex == 4)
            {
                Dinamik_Oncelik_Bekleme_Paneli(sender, e);
            }
            if (tabControl1.SelectedIndex == 1)
            {
                Musteriler(sender, e);
            }
            if (tabControl1.SelectedIndex == 2)
            {
                Listeleme(sender, e);
            }




        }

        public void AdminGirisBasarili()
            {
                adminAuthenticated = true;
                tabControl1.TabPages[0].Enabled = true; 
                tabControl1.SelectedIndex = 0;
            }
        public void ActivateTabIndex(int index)
        {
            if (index >= 0 && index < tabControl1.TabCount)
            {
                tabControl1.SelectedIndex = index;
                adminAuthenticated = true;
                tabControl1.TabPages[0].Enabled = true;
            }
        }


        private void ListeleProduct()
        {
            try
            {
                string query = "SELECT * FROM products";

                DataTable productData = DataBaseHelper.ExecuteQuery(query);

                if (productData != null && productData.Rows.Count > 0)
                {
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                    dataGridView1.DataSource = productData;

                    dataGridView1.ReadOnly = true;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                else
                {
                    MessageBox.Show("Veritabanında görüntülenecek veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Ürün_Sil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int urunID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);
                string urunAdi = dataGridView1.SelectedRows[0].Cells["productname"].Value.ToString();

                string checkOrdersQuery = $"SELECT COUNT(*) FROM orders WHERE ProductID = {urunID} AND orderstatus = 'bekliyor'";
                int orderCount = Convert.ToInt32(DataBaseHelper.ExecuteScalar(checkOrdersQuery));

                if (orderCount > 0)
                {
                    DialogResult result = MessageBox.Show(
                        $"{urunAdi} ürününe ait bekleyen sipariş kayıtları bulunmaktadır. \nÜrünü ve ilgili tüm kayıtları silmek istiyor musunuz?",
                        "Silme Onayı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            string getOrderDetailsQuery = @"
                            SELECT c.customername, c.customertype, p.productname, o.quantity 
                            FROM orders o
                            JOIN customers c ON o.customerid = c.customerid
                            JOIN products p ON o.productid = p.productid
                            WHERE o.productid = @productId AND o.orderstatus = 'bekliyor'";
                            var orderParams = new Dictionary<string, object> { { "@productId", urunID } };
                            var orderDetails = DataBaseHelper.ExecuteQuery44(getOrderDetailsQuery, orderParams);

                            foreach (var detail in orderDetails)
                            {
                                string customername = detail["customername"].ToString();
                                string customerType = detail["customertype"].ToString();
                                string productname = detail["productname"].ToString();
                                int quantity = Convert.ToInt32(detail["quantity"]);

                                DisplayLogs(customername, customerType, productname, quantity, "İşlem iptal edildi.Ürün stoğu yetersiz");
                            }

                            string deleteLogsQuery = @"
                            DELETE FROM logs 
                            WHERE orderid IN 
                            (SELECT orderid FROM orders WHERE productid = @productId AND orderstatus = 'bekliyor')";
                            DataBaseHelper.ExecuteNonQuery23(deleteLogsQuery, orderParams);

                            string deleteOrdersQuery = "DELETE FROM orders WHERE productid = @productId AND orderstatus = 'bekliyor'";
                            DataBaseHelper.ExecuteNonQuery23(deleteOrdersQuery, orderParams);

                            string deleteProductQuery = "DELETE FROM products WHERE productid = @productId";
                            var productParams = new Dictionary<string, object> { { "@productId", urunID } };
                            bool success = DataBaseHelper.ExecuteNonQuery23(deleteProductQuery, productParams);

                            if (success)
                            {
                                MessageBox.Show("Ürün ve ilgili tüm kayıtlar başarıyla silindi.");
                                VerileriYenile();
                            }
                            else
                            {
                                MessageBox.Show("Ürün silinirken bir hata oluştu.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Silme işlemi sırasında bir hata oluştu: {ex.Message}");
                        }
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show(
                        $"{urunAdi} ürününü silmek istediğinize emin misiniz?",
                        "Silme Onayı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            string deleteProductQuery = "DELETE FROM products WHERE ProductID = @productId";
                            var productParams = new Dictionary<string, object> { { "@productId", urunID } };
                            bool success = DataBaseHelper.ExecuteNonQuery23(deleteProductQuery, productParams);

                            if (success)
                            {
                                MessageBox.Show("Ürün başarıyla silindi.");
                                VerileriYenile();
                            }
                            else
                            {
                                MessageBox.Show("Ürün silinirken bir hata oluştu.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Silme işlemi sırasında bir hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz ürünü seçin.");
            }
        }
        private void Güncelle_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int urunID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);
                string urunAdi = dataGridView1.SelectedRows[0].Cells["productname"].Value.ToString();
                string urunFiyati = dataGridView1.SelectedRows[0].Cells["price"].Value.ToString();
                string urunStoku = dataGridView1.SelectedRows[0].Cells["stock"].Value.ToString();
                Guncelle_Form form3 = new Guncelle_Form(urunID, urunAdi, urunFiyati, urunStoku, this);
                form3.ShowDialog();
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz ürünü seçin.");
            }
        }
        private void Ürün_Ekle_Click(object sender, EventArgs e)
        {
            Urun_Ekle form4 = new Urun_Ekle(this);
            form4.ShowDialog();
        }

        public void VerileriYenile()
            {
                string query = "SELECT * FROM products";
                DataTable productData = DataBaseHelper.ExecuteQuery(query);
                dataGridView1.DataSource = productData;
            }




        private void button2_Click(object sender, EventArgs e)
        {
            Siparis_Ver siparisVerForm = new Siparis_Ver(SelectedCustomerName,this);
            siparisVerForm.ShowDialog();
        }

        private void Listeleme(object sender, EventArgs e)
        {
            string query = "SELECT productname, stock, price FROM products";

            DataTable productData = DataBaseHelper.ExecuteQuery(query);

            if (productData != null && productData.Rows.Count > 0)
            {
                dataGridView3.DataSource = productData;

                dataGridView3.Columns["productname"].HeaderText = "Ürün Adı";
                dataGridView3.Columns["stock"].HeaderText = "Stok Miktarı";
                dataGridView3.Columns["price"].HeaderText = "Fiyat";
            }
            else
            {
                MessageBox.Show("Veri çekilemedi veya ürün bulunamadı.");
            }
        }

        private void Dinamik_Oncelik_Bekleme_Paneli(object sender, EventArgs e)
        {
            try
            {
                dataGridView4.DataSource = null;
                button6.Text = string.Empty;
                button8.Text = string.Empty;
                button9.Text = string.Empty;
                button10.Text = string.Empty;
                label7.Text = "İşlem Tamamlandı.";
                progressBar1.Visible = false;
                string query = @"
        SELECT c.customerid, c.customername, c.customertype, 
               o.orderid, o.productid, p.productname,o.orderdate
        FROM customers c
        INNER JOIN orders o ON c.customerid = o.customerid
        INNER JOIN products p ON o.productid = p.productid
        WHERE o.orderstatus = 'bekliyor'"; 

                DataTable orderData = DataBaseHelper.ExecuteQuery(query);

                if (orderData != null)
                {
                    orderData.Columns.Add("Bekleme Süresi", typeof(int));
                    orderData.Columns.Add("Öncelik Skoru", typeof(decimal));

                   
                    foreach (DataRow row in orderData.Rows)
                    {
                        int orderId = Convert.ToInt32(row["orderid"]);
                        int customerId = Convert.ToInt32(row["customerid"]); 
                        DateTime orderDate = Convert.ToDateTime(row["orderdate"]); 
                        int waitingTime = Hesaplama.Bekleme_Suresi(orderDate); 
                        string customerType = row["customertype"].ToString(); 
                        decimal priorityScore = Hesaplama.Oncelik_Skoru(customerType, orderDate);

                        row["Bekleme Süresi"] = waitingTime;
                        row["Öncelik Skoru"] = priorityScore;
                    }

                    var filteredRows = orderData.AsEnumerable()
                        .Where(row => Convert.ToInt32(row["Bekleme Süresi"]) > 0)
                        .OrderByDescending(row => Convert.ToDecimal(row["Öncelik Skoru"]))
                        .CopyToDataTable();

                    if (filteredRows.Rows.Count > 0)
                    {
                        dataGridView4.DataSource = filteredRows;
                        dataGridView4.ReadOnly = true;

                        dataGridView4.Columns["orderid"].Visible = false;
                        dataGridView4.Columns["productid"].Visible = false;
                        dataGridView4.Columns["customerid"].Visible = false;
                        dataGridView4.Columns["orderdate"].Visible = false;



                        dataGridView4.Columns["customername"].HeaderText = "Müşteri Adı";
                        dataGridView4.Columns["customertype"].HeaderText = "Müşteri Tipi";
                        dataGridView4.Columns["productname"].HeaderText = "Ürün Adı";
                        dataGridView4.Columns["Bekleme Süresi"].HeaderText = "Bekleme Süresi";
                        dataGridView4.Columns["Öncelik Skoru"].HeaderText = "Öncelik Skoru";



                        foreach (DataGridViewRow row in dataGridView4.Rows)
                        {
                            int waitingTime = Convert.ToInt32(row.Cells["Bekleme Süresi"].Value);
                            row.DefaultCellStyle.BackColor = waitingTime == 0 ? Color.LightYellow : Color.LightBlue;
                        }


                        List<string> customerNames = new List<string>();
                        foreach (DataRow row in filteredRows.Rows)
                        {
                            customerNames.Add(row["customername"].ToString());
                        }

                        string customerNamesText = string.Join(", ", customerNames);

                        string message = $"{customerNamesText} siparişleri işleniyor...";

                        label7.Text = message;  

                        progressBar1.Visible = true; 

                        progressBar1.Style = ProgressBarStyle.Marquee;
                        progressBar1.MarqueeAnimationSpeed = 30;
                        if (filteredRows.Rows.Count >= 1)
                        {
                            button6.Text = filteredRows.Rows[0]["customername"].ToString();
                        }
                        if (filteredRows.Rows.Count >= 2)
                        {
                            button8.Text = filteredRows.Rows[1]["customername"].ToString();
                        }
                        if (filteredRows.Rows.Count >= 3)
                        {
                            button9.Text = filteredRows.Rows[2]["customername"].ToString();
                        }
                        if (filteredRows.Rows.Count > 0)
                        {
                            int lastIndex = filteredRows.Rows.Count - 1; 
                            button10.Text = filteredRows.Rows[lastIndex]["customername"].ToString(); 
                        }
                    }
                    else
                    {
                        MessageBox.Show("Şu anda bekleyen sipariş bulunmamaktadır.");
                    }
                }
                else
                {
                    MessageBox.Show("Sipariş bilgileri alınamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tablo şuan için boş lütfen sipariş veriniz :)");
            }
        }


        private void SetupButtons()
        {
          
            button6.Size = new Size(100, 50);
            button8.Size = new Size(100, 50);
            button9.Size = new Size(100, 50);
            button10.Size = new Size(100, 50);

            button6.BackColor = Color.LightGreen;
            button8.BackColor = Color.LightGreen;
            button9.BackColor = Color.LightGreen;
            button10.BackColor = Color.LightGreen;
        }
        private void Musteriler(object sender, EventArgs e)
        {
            string query = @"
    SELECT c.customerid, c.customername, c.budget, c.customertype, 
           (SELECT MIN(o.orderdate) FROM orders o WHERE o.customerid = c.customerid) AS first_order_date,
           (SELECT o.orderstatus FROM orders o WHERE o.customerid = c.customerid AND o.orderstatus = 'bekliyor' ORDER BY o.orderdate ASC LIMIT 1) AS order_status
    FROM customers c";

            DataTable customerData = DataBaseHelper.ExecuteQuery(query);

            if (customerData != null)
            {
                customerData.Columns.Add("Bekleme Süresi", typeof(int));
                customerData.Columns.Add("Öncelik Skoru", typeof(decimal));

                foreach (DataRow row in customerData.Rows)
                {
                    int customerId = Convert.ToInt32(row["customerid"]);
                    DateTime firstOrderDate = row.IsNull("first_order_date") ? DateTime.Now : Convert.ToDateTime(row["first_order_date"]);
                    string customerType = row["customertype"].ToString();
                    string orderStatus = row["order_status"].ToString();

                    int waitingTime;
                    decimal priorityScore;

                    if (orderStatus == "bekliyor")
                    {
                        waitingTime = Hesaplama.Bekleme_Suresi(firstOrderDate);
                        priorityScore = Hesaplama.Oncelik_Skoru(customerType, firstOrderDate);
                    }
                    else
                    {
                        waitingTime = 0;
                        priorityScore = Hesaplama.Oncelik_Skoru(customerType, DateTime.Now);
                    }

                    row["Bekleme Süresi"] = waitingTime;
                    row["Öncelik Skoru"] = priorityScore;
                }

                dataGridView2.DataSource = customerData;
                dataGridView2.ReadOnly = true;

                if (dataGridView2.Columns.Contains("first_order_date"))
                {
                    dataGridView2.Columns["first_order_date"].Visible = false;
                }
                if (dataGridView2.Columns.Contains("order_status"))
                {
                    dataGridView2.Columns["order_status"].Visible = false;
                }

                foreach (DataGridViewRow gridRow in dataGridView2.Rows)
                {
                    int waitingTime = Convert.ToInt32(gridRow.Cells["Bekleme Süresi"].Value);

                    if (waitingTime == 0)
                    {
                        gridRow.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                    else
                    {
                        gridRow.DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                }
            }
            else
            {
                MessageBox.Show("Müşteri bilgileri alınamadı.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();

            GirisForm girisForm = new GirisForm();
            girisForm.Show();
        }


        private void Siparisi_Onayla_Click(object sender, EventArgs e)
        {
            try
            {
                string query = @"
        SELECT c.customerid, c.customername, c.customertype, 
               o.orderid, o.productid, o.quantity, p.productname, p.price, p.stock,
               c.budget, c.totalspent, o.orderdate, o.orderstatus
        FROM orders o
        INNER JOIN customers c ON c.customerid = o.customerid
        INNER JOIN products p ON p.productid = o.productid
        WHERE o.orderstatus = 'bekliyor'";

                DataTable orderData = DataBaseHelper.ExecuteQuery(query);

                if (orderData == null || orderData.Rows.Count == 0)
                {
                    MessageBox.Show("Bekleyen sipariş bulunmamaktadır.");
                    return;
                }

                Dictionary<int, decimal> customerBudgets = new Dictionary<int, decimal>();
                Dictionary<int, int> productStocks = new Dictionary<int, int>();

                orderData.Columns.Add("OncelikSkoru", typeof(decimal));

                foreach (DataRow row in orderData.Rows)
                {
                    string customerType = row["customertype"].ToString();
                    DateTime orderDate = Convert.ToDateTime(row["orderdate"]);
                    row["OncelikSkoru"] = Hesaplama.Oncelik_Skoru(customerType, orderDate);
                }

                var sortedOrders = orderData.AsEnumerable()
                    .OrderByDescending(row => Convert.ToDecimal(row["OncelikSkoru"]))
                    .CopyToDataTable();

                foreach (DataRow row in sortedOrders.Rows)
                {
                    int orderId = Convert.ToInt32(row["orderid"]);
                    int customerId = Convert.ToInt32(row["customerid"]);
                    int productId = Convert.ToInt32(row["productid"]);
                    int quantity = Convert.ToInt32(row["quantity"]);
                    decimal price = Convert.ToDecimal(row["price"]);
                    decimal totalPrice = quantity * price;
                    string customerType = row["customertype"].ToString();
                    string customername = row["customername"].ToString();
                    string productname = row["productname"].ToString();

                    if (!customerBudgets.ContainsKey(customerId))
                    {
                        string budgetQuery = "SELECT budget FROM customers WHERE customerid = @customerId";
                        var budgetParams = new Dictionary<string, object> { { "@customerId", customerId } };
                        customerBudgets[customerId] = (decimal)DataBaseHelper.islev1(budgetQuery, budgetParams).Rows[0]["budget"];
                    }

                    if (!productStocks.ContainsKey(productId))
                    {
                        productStocks[productId] = Convert.ToInt32(row["stock"]);
                    }

                    decimal currentBudget = customerBudgets[customerId];
                    int currentStock = productStocks[productId];

                    if (currentStock - quantity < 0)
                    {
                        UpdateOrderStatusAndLog(orderId, customerId, "Hata", "Ürün stoğu yetersiz", customerType);
                        DisplayLogs(customername, customerType, productname, quantity, "İşlem iptal edildi. Stok yetersiz");
                        continue;
                    }

                    if (currentBudget - totalPrice < 0)
                    {
                        UpdateOrderStatusAndLog(orderId, customerId, "Hata", "Bütçe yetersiz", customerType);
                        DisplayLogs(customername, customerType, productname, quantity, "İşlem iptal edildi. Bütçe yetersiz");
                        continue;
                    }

                    // Güncellenen stok ve bütçeyi kaydet
                    customerBudgets[customerId] -= totalPrice;
                    productStocks[productId] -= quantity;

                    UpdateDatabaseForOrder(orderId, customerId, productId, quantity, totalPrice, customerBudgets[customerId]);

                    DataBaseHelper.AddLog(customerId, orderId, DateTime.Now, "Bilgilendirme", "Satın alma başarılı", customerType);
                    DisplayLogs(customername, customerType, productname, quantity, "Satın alma işlemi onaylandı. Başarılı");
                }

                VerileriYenile();
                Dinamik_Oncelik_Bekleme_Paneli(sender, e);
                Musteriler(sender, e);

                MessageBox.Show("Tüm siparişler başarıyla işleme alındı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
        }

        private void UpdateOrderStatusAndLog(int orderId, int customerId, string status, string message, string customerType)
        {
            string updateOrderStatusQuery = @"
    UPDATE orders 
    SET orderstatus = @status 
    WHERE orderid = @orderId";

            var orderStatusParams = new Dictionary<string, object>
    {
        { "@status", status },
        { "@orderId", orderId }
    };

            DataBaseHelper.ExecuteNonQuery23(updateOrderStatusQuery, orderStatusParams);
            DataBaseHelper.AddLog(customerId, orderId, DateTime.Now, status, message, customerType);
        }

        private void UpdateDatabaseForOrder(int orderId, int customerId, int productId, int quantity, decimal totalPrice, decimal newBudget)
        {
            string updateProductQuery = @"
    UPDATE products 
    SET stock = stock - @quantity 
    WHERE productid = @productId";

            string updateCustomerQuery = @"
    UPDATE customers 
    SET budget = @budget, 
        totalspent = totalspent + @totalPrice 
    WHERE customerid = @customerId";

            string updateOrderStatusQuery = @"
    UPDATE orders 
    SET orderstatus = 'Onaylandı' 
    WHERE orderid = @orderId";

            var productParams = new Dictionary<string, object>
    {
        { "@quantity", quantity },
        { "@productId", productId }
    };

            var customerParams = new Dictionary<string, object>
    {
        { "@budget", newBudget },
        { "@totalPrice", totalPrice },
        { "@customerId", customerId }
    };

            var orderStatusParams = new Dictionary<string, object>
    {
        { "@orderId", orderId }
    };

            DataBaseHelper.ExecuteNonQuery23(updateProductQuery, productParams);
            DataBaseHelper.ExecuteNonQuery23(updateCustomerQuery, customerParams);
            DataBaseHelper.ExecuteNonQuery23(updateOrderStatusQuery, orderStatusParams);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

            GirisForm girisForm = new GirisForm();
            girisForm.Show();
        }
    }
} 

