using System;
using System.Collections.Generic;
using System.Text;

using System.Text.Json;
using ConsoleRPG.DTO;
using ConsoleRPG.Utility;

namespace ConsoleRPG.ApiGateway.Shop.DB
{
    class ShopDB
    {
        public Dictionary<long, Account_Jewel> Account_Jewels { get; private set; }

        public ShopDB()
        {
            Account_Jewels = Account_Jewel_Load();
        }

        private void Account_Jewel_Save()
        {
            string filePath = "DB\\Account_Jewel.txt";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Account_Jewels, options);
            File.WriteAllText(filePath, jsonString);
        }
        private Dictionary<long, Account_Jewel> Account_Jewel_Load()
        {
            string filePath = "DB\\Account_Jewel.txt";
            if (!File.Exists(filePath))
            {
                Account_Jewel Account_Jewel = new(1, 1000, 1000);

                Account_Jewels = new Dictionary<long, Account_Jewel>();
                Account_Jewels[1] = Account_Jewel;

                Account_Jewel_Save();
                return Account_Jewels;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<long, Account_Jewel>>(jsonString);
        }

        public Account_Jewel GetAccount_Jewel(long account_Uid)
        {
            return Account_Jewels[account_Uid];
        }

        public bool Account_Jewel_Free_Update(long uid, int free)
        {
            Account_Jewels[uid].Jewel_Free = Account_Jewels[uid].Jewel_Free + free;

            Account_Jewel_Save();
            Account_Jewels = Account_Jewel_Load();
            return true;
        }

        public bool Account_Jewel_Money_Update(long uid, int money)
        {
            Account_Jewels[uid].Jewel_Money = Account_Jewels[uid].Jewel_Money + money;

            Account_Jewel_Save();
            Account_Jewels = Account_Jewel_Load();
            return true;
        }


    }
}
