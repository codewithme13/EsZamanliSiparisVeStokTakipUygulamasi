using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using System.Windows.Forms;
using System.Threading;

namespace Es_Zamanlı_Siparis_Ve_Stok_Yönetimi_Uygulamasi
{
    public class DataBaseHelper
    {
        private static string connectionString = "Host=localhost;Port=5432;Database=mydatabase;Username=myuser;Password=mypassword"";



        public static List<Dictionary<string, object>> ExecuteQuery44(string query, Dictionary<string, object> parameters = null)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }

                                results.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veritabanı sorgusu sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return results;
        }
        //stok güncellenirken diğer işlmeleri askıya alma
        private static readonly object lockObject = new object();
        private static bool isUpdatingStock = false;

        public static void UpdateStock(int productId, int newQuantity)
        {
            lock (lockObject)
            {
                try
                {
                    isUpdatingStock = true;
                    Thread.Sleep(5000); // Stok güncelleme işlemi simülasyonu.
                    Console.WriteLine($"Ürün ID: {productId}, Yeni Stok: {newQuantity}");
                }
                finally
                {
                    isUpdatingStock = false;
                    Console.WriteLine("Stok güncellemesi tamamlandı.");
                }
            }
        }

        public static void PlaceOrder(int productId, int quantity)
        {
            lock (lockObject)
            {
                if (isUpdatingStock)
                {
                    Console.WriteLine("Stok güncelleniyor. Sipariş verilemez.");
                    return;
                }

                Console.WriteLine("Sipariş veriliyor...");
                Thread.Sleep(2000); 
                Console.WriteLine($"Ürün ID: {productId}, Sipariş Adedi: {quantity}");
            }
        }
        public static DataTable GetPendingOrders()
        {
            string query = @"
        SELECT c.customerid, c.customername, c.customertype, 
               o.orderid, o.productid, o.quantity, p.productname, p.price, p.stock,
               c.budget, c.totalspent, o.orderdate, o.orderstatus
        FROM orders o
        INNER JOIN customers c ON c.customerid = o.customerid
        INNER JOIN products p ON p.productid = o.productid
        WHERE o.orderstatus = 'bekliyor'";

            DataTable orderData = ExecuteQuery(query);
            return orderData;
        }


            public static void ApproveOrdersWithThreads()
        {
            DataTable pendingOrders = GetPendingOrders();

            if (pendingOrders == null || pendingOrders.Rows.Count == 0)
            {
                MessageBox.Show("Bekleyen sipariş bulunmamaktadır.");
                return;
            }


            pendingOrders.Columns.Add("OncelikSkoru", typeof(decimal));

            foreach (DataRow row in pendingOrders.Rows)
            {
                string customerType = row["customertype"].ToString();
                DateTime orderDate = Convert.ToDateTime(row["orderdate"]);
                row["OncelikSkoru"] = Hesaplama.Oncelik_Skoru(customerType, orderDate);
            }

            var sortedOrders = pendingOrders.AsEnumerable()
                .OrderByDescending(row => Convert.ToDecimal(row["OncelikSkoru"]))
                .ToList();
            List<Thread> threads = new List<Thread>();

            foreach (DataRow row in sortedOrders)
            {
                Thread orderThread = new Thread(() => ProcessOrder(row));
                threads.Add(orderThread);
                orderThread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();  //threadin bitmesini bekler --> siparisşlerin onaylnmasını beklıyor 
            }

        }
        public static int GetOrderId(int customerId, int productId, DateTime orderDate)
        {
            string query = $"SELECT orderid FROM orders WHERE customerid = @customerId AND productid = @productId AND orderdate = @orderDate";
                        var parameters = new Dictionary<string, object>
                {
                    { "@customerId", customerId },
                    { "@productId", productId },
                    { "@orderDate", orderDate }
                };

            object result = Calıstır(query, parameters);
            if (result == null)
            {
                Console.WriteLine("Order ID bulunamadı.");
                return -1;
            }

            return Convert.ToInt32(result);
        }

        public static object Calıstır(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sorguyu çalıştırırken hata oluştu: {ex.Message}");
                return null;
            }
        }

        public static bool AddLog(int customerId, int orderId, DateTime logDate, string logType, string logDetails, string customerType)
        {
            try
            {
                string query = @"
            INSERT INTO logs (customerid, orderid, logdate, logtype, logdetails, customertype)
            VALUES (@customerid, @orderid, @logdate, @logtype, @logdetails, @customertype)
        ";

                var parameters = new Dictionary<string, object>
        {
            { "@customerid", customerId },
            { "@orderid", orderId },
            { "@logdate", logDate },
            { "@logtype", logType },
            { "@logdetails", logDetails },
            { "@customertype", customerType }
        };

                Console.WriteLine("SQL Query: " + query);
                Console.WriteLine("Parameters:");
                foreach (var param in parameters)
                {
                    Console.WriteLine($"{param.Key}: {param.Value}");
                }

                bool result = SorguyuYurut(query, parameters);
                if (!result)
                {
                    Console.WriteLine("Log kaydederken hata oluştu: Sorgu başarısız.");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log eklerken hata oluştu: {ex.Message}");
                return false;
            }
        }

        public static bool SorguyuYurut(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        // Parametreleri ekle
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sorguyu yürütürken hata oluştu: {ex.Message}");
                return false;
            }
        }


        public static DataTable ExecuteQuery(string query)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (var adapter = new NpgsqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    return null;
                }
            }
        }
        public static bool ExecuteNonQuery(string query)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static object ExecuteScalar(string query)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                return null;
            }
        }


        public static DataTable ExecuteQuery1(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanı hatası: {ex.Message}");
                return null;
            }
        }

        public static object ExecuteScalar1(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        // Parametreleri ekle
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        return command.ExecuteNonQuery(); // Satır sayısını döndürecek
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Veritabanı hatası: {ex.Message}");
                return null;
            }
        }
        public static bool Kaydet(int customerId, int productId, int quantity, decimal totalPrice, DateTime orderDate, string orderStatus)
        {
            try
            {
                string insertQuery = @"
                INSERT INTO orders (customerid, productid, quantity, totalprice, orderdate, orderstatus)
                VALUES (@customerId, @productId, @quantity, @totalPrice, @orderDate, @orderStatus)
            ";

                // Parametreler
                var parameters = new Dictionary<string, object>
            {
                { "@customerId", customerId },
                { "@productId", productId },
                { "@quantity", quantity },
                { "@totalPrice", totalPrice },
                { "@orderDate", orderDate },
                { "@orderStatus", orderStatus }
            };

                int result = Convert.ToInt32(ExecuteScalar1(insertQuery, parameters));

                if (result != 0)
                {
                    string updateQuery = "UPDATE orders SET orderstatus = @orderStatus WHERE customerid = @customerId AND productid = @productId AND orderdate = @orderDate";

                            var updateParameters = new Dictionary<string, object>
                    {
                        { "@customerId", customerId },
                        { "@productId", productId },
                        { "@orderDate", orderDate }
                    };

                    ExecuteScalar1(updateQuery, updateParameters);

                    return true; 
                }
                else
                {
                    return false; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sipariş kaydederken hata oluştu: {ex.Message}");
                return false;
            }

        }

        public static bool ExecuteNonQuery23(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sorguyu yürütürken hata oluştu: {ex.Message}");
                return false;
            }
        }
        public static void ProcessOrder(DataRow row)
        {
            return;
        }
        public static void ThreadBaslat()
        {
            ApproveOrdersWithThreads();
        }

        public static DataTable islev1(string query, Dictionary<string, object> parameters = null)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    DataTable dataTable = new DataTable();
                    try
                    {
                        connection.Open();
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Veritabanı sorgusu hatası: " + ex.Message);
                    }
                    return dataTable;
                }
            }
        }
    }
}
