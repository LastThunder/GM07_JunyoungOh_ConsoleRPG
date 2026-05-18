using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Dm = ConsoleRPG.Utility.DataManager;
using ConsoleRPG.DTO;

namespace ConsoleRPG.ApiGateway.Item
{
    class ItemAS
    {
        public long Account_Uid { get; set; }
        private DB.ItemDB IDB { get; set; }

        public ItemAS (long account_Uid)
        {
            IDB = new();
            Account_Uid = account_Uid;
        }


        public Account_Gold GetAccount_Gold()
        {
            return IDB.GetAccount_Gold(Account_Uid);
        }

        public Account_Gold Gold_Update (long gold)
        {
            IDB.Account_Gold_Update(Account_Uid, gold);

            return GetAccount_Gold();
        }


        public Dictionary<long, Equipment> GetEquipment()
        {
            return IDB.GetEquipment(Account_Uid);
        }


        public Dictionary<long, Equipment> Equipment_Insert(int ItemNo = 10100001)
        {
            Equipment equipment = new(-1, Account_Uid, ItemNo, ItemNo/10000000);
            IDB.Equipment_Insert(equipment);
            return GetEquipment();
        }

        public Dictionary<long, Equipment> Equip_Item(long uid, long Equip_Character_Uid)
        {
            Dictionary<long, Equipment> equipments = GetEquipment();

            if (equipments[uid].Account_Uid == Account_Uid) {
                if (equipments[uid].Equip_Character_Uid == Equip_Character_Uid) equipments[uid].Equip_Character_Uid = -1;
                else equipments[uid].Equip_Character_Uid = Equip_Character_Uid;
            }           

            IDB.Equipment_Update(equipments);
            return GetEquipment();
        }

        public (Account_Gold, Dictionary<long, Equipment>) Sell_Equip(long uid)
        {
            if (IDB.Equipments[uid].Account_Uid == Account_Uid && IDB.Equipments[uid] != null)
            {
                Gold_Update(Dm.Item[IDB.Equipments[uid].ItemNo].Gold / 2);
                IDB.Equipment_Delete(uid);                
            }
            return (GetAccount_Gold(), GetEquipment());
        }

        
    }
}
