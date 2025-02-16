using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es_Zamanlı_Siparis_Ve_Stok_Yönetimi_Uygulamasi
{
    public static class Hesaplama
    {
            private const decimal WaitingTimeWeight = 0.5m;

            
            public static int Bekleme_Suresi(DateTime orderDate)
            {
                TimeSpan elapsedTime = DateTime.Now - orderDate;
                return (int)elapsedTime.TotalSeconds;
            }

           
            public static decimal Oncelik_Skoru(string customerType, DateTime orderDate)
            {
                decimal basePriority;

                string normalizedType = customerType.ToLower();
                if (normalizedType == "standard")
                {
                    normalizedType = "standart";
                }

                if (normalizedType == "premium")
                {
                    basePriority = 15m;
                }
                else if (normalizedType == "standart")
                {
                    basePriority = 10m;
                }
                else
                {
                    basePriority = 10m;
                }

                int waitingTime = Bekleme_Suresi(orderDate);
                decimal waitingTimeImpact = waitingTime * WaitingTimeWeight;

                return basePriority + waitingTimeImpact;
            }
        }
    }