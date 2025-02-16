using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es_Zamanlı_Siparis_Ve_Stok_Yönetimi_Uygulamasi
{
    public static class OrderTracker
    {
        private static Dictionary<int, DateTime> waitingTimes = new Dictionary<int, DateTime>();

        // Bekleme süresi ağırlığı (saniye başına 0.5 puan)
        private const decimal WaitingTimeWeight = 0.5m;

        /// <summary>
        /// Bekleme süresini başlatır.
        /// </summary>
        /// <param name="customerId">Müşteri ID</param>
        public static void StartWaiting(int customerId)
        {
            if (waitingTimes.ContainsKey(customerId))
            {
                waitingTimes[customerId] = DateTime.Now;
            }
            else
            {
                waitingTimes.Add(customerId, DateTime.Now);
            }
        }

        /// <summary>
        /// Müşterinin bekleme süresini saniye olarak döndürür.
        /// </summary>
        /// <param name="customerId">Müşteri ID</param>
        /// <returns>Bekleme süresi (saniye)</returns>
        public static int Bekleme_Suresi(int customerId)
        {
            if (waitingTimes.ContainsKey(customerId))
            {
                DateTime startTime = waitingTimes[customerId];
                TimeSpan elapsedTime = DateTime.Now - startTime;
                return (int)elapsedTime.TotalSeconds;
            }
            return 0;
        }

        /// <summary>
        /// Öncelik skorunu hesaplar.
        /// </summary>
        /// <param name="customerType">Müşteri tipi ("premium" veya "normal")</param>
        /// <param name="waitingTime">Bekleme süresi (saniye)</param>
        /// <returns>Öncelik skoru</returns>
        public static decimal Oncelik_Skoru(string customerType, int waitingTime)
        {
            decimal basePriority;

            string normalizedType = customerType.ToLower();
            if (normalizedType == "Standard")
            {
                normalizedType = "Standart";
            }

            if (normalizedType == "premium")
            {
                basePriority = 15m;
            }
            else if (normalizedType == "Standart")
            {
                basePriority = 10m;
            }
            else
            {
                basePriority = 10m;
            }


            // Bekleme süresi etkisi
            decimal waitingTimeImpact = waitingTime * WaitingTimeWeight;

            // Toplam öncelik skoru
            return basePriority + waitingTimeImpact;
        }
    }
}
