using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

using Dm = ConsoleRPG.Utility.DataManager;
using ConsoleRPG.DTO;


namespace ConsoleRPG.ApiGateway.Item.DB
{      

    class ItemDB
    {
        public Dictionary<long, Account_Gold> Account_Golds { get; private set; }
        public Dictionary<long, Equipment> Equipments { get; private set; }
        private long LastEquipmentUid { get; set; }


        public ItemDB()
        {
            Account_Golds = Account_Gold_Load();
            Equipments = Equipment_Load();
            LastEquipmentUid = Equipments.LastOrDefault().Key + 1;
        }
        private void Account_Gold_Save()
        {
            string filePath = "DB\\Account_Gold.txt";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Account_Golds, options);
            File.WriteAllText(filePath, jsonString);
        }
        private Dictionary<long, Account_Gold> Account_Gold_Load()
        {
            string filePath = "DB\\Account_Gold.txt";
            if (!File.Exists(filePath))
            {
                Account_Gold Account_Gold = new(1, 0);

                Account_Golds = new Dictionary<long, Account_Gold>();
                Account_Golds[1] = Account_Gold;

                Account_Gold_Save();
                return Account_Golds;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<long, Account_Gold>>(jsonString);
        }
        private void Equipment_Save()
        {
            string filePath = "DB\\Equipment.txt";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Equipments, options);
            File.WriteAllText(filePath, jsonString);
        }
        private Dictionary<long, Equipment> Equipment_Load()
        {
            string filePath = "DB\\Equipment.txt";
            if (!File.Exists(filePath))
            {
                Equipment Equipment = new(0,-1,-1,0);

                Equipments = new Dictionary<long, Equipment>();
                Equipments[LastEquipmentUid] = Equipment;

                Equipment_Save();
                return Equipments;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<long, Equipment>>(jsonString);
        }


        public bool Account_Gold_Update(long uid, long gold)
        {
            Account_Golds[uid].Gold = Account_Golds[uid].Gold + gold;

            Account_Gold_Save();
            Account_Golds = Account_Gold_Load();
            return true;
        }

        public Account_Gold GetAccount_Gold(long account_Uid)
        {
            return Account_Golds[account_Uid];
        }


        public Dictionary<long, Equipment> GetEquipment(long account_Uid)
        {
            return Equipments.Where(x => x.Value.Account_Uid == account_Uid).ToDictionary(x => x.Key, x => x.Value);
        }

        public bool Equipment_Insert(Equipment Equipment)
        {
            Equipment.Uid = LastEquipmentUid;
            Equipments[LastEquipmentUid] = Equipment;
            Equipment_Save();
            Equipments = Equipment_Load();
            LastEquipmentUid = LastEquipmentUid + 1;
            return true;
        }

        public bool Equipment_Update(Dictionary<long, Equipment> equipment)
        {
            foreach (long uid in equipment.Keys)
            {
                Equipments[uid] = equipment[uid];
            }
            Equipment_Save();
            Equipments = Equipment_Load();
            return true;
        }

        public bool Equipment_Delete(long uid)
        {            
            Equipments.Remove(uid);
            Equipment_Save();
            Equipments = Equipment_Load();
            return true;
        }

    }
}
