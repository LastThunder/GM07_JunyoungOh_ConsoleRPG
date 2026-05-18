using ConsoleRPG.DTO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ConsoleRPG.ApiGateway.Stage.DB
{    

    class StageDB
    {
        public Dictionary<long, Account_Location> Account_Locations { get; private set; }
        public Dictionary<long, Character_Equip> Character_Equips { get; private set; }

        public StageDB()
        {
            Account_Locations = Account_Location_Load();
            Character_Equips = Character_Equip_Load();
        }

        private void Account_Location_Save()
        {
            string filePath = "DB\\Account_Location.txt";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Account_Locations, options);
            File.WriteAllText(filePath, jsonString);
        }
        private Dictionary<long, Account_Location> Account_Location_Load()
        {
            string filePath = "DB\\Account_Location.txt";
            if (!File.Exists(filePath))
            {
                Account_Location Account_Location = new(1, 20101);

                Account_Locations = new Dictionary<long, Account_Location>();
                Account_Locations[1] = Account_Location;

                Account_Location_Save();
                return Account_Locations;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<long, Account_Location>>(jsonString);
        }
        
        private void Character_Equip_Save()
        {
            string filePath = "DB\\Character_Equip.txt";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Character_Equips, options);
            File.WriteAllText(filePath, jsonString);
        }
        private Dictionary<long, Character_Equip> Character_Equip_Load()
        {
            string filePath = "DB\\Character_Equip.txt";
            if (!File.Exists(filePath))
            {
                Character_Equip character_Equip = new();
                character_Equip.Character_Uid = 1;
                character_Equip.Equip_Equipment_Uid = [-1, -1, -1, -1, -1];

                Character_Equips = new Dictionary<long, Character_Equip>();
                Character_Equips[character_Equip.Character_Uid] = character_Equip;

                Character_Equip_Save();
                return Character_Equips;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<long, Character_Equip>>(jsonString);
        }


        public Account_Location GetAccount_Location(long account_Uid)
        {
            return Account_Locations[account_Uid];
        }

        public bool Account_Location_Update(long uid, int stageNo)
        {
            Account_Locations[uid].StageNo = stageNo;

            Account_Location_Save();
            Account_Locations = Account_Location_Load();
            return true;
        }

        public Character_Equip GetCharacter_Equip(long uid)
        {
            return Character_Equips[uid];
        }

        public bool Character_Equip_Update(long uid, Character_Equip equip)
        {
            Character_Equips[uid] = equip;
            Character_Equip_Save();
            Character_Equips = Character_Equip_Load();
            return true;
        }

        public Character_Equip Character_Equip_Insert(long uid)
        {
            Character_Equip character_Equip = new();
            character_Equip.Character_Uid = uid;
            character_Equip.Equip_Equipment_Uid = [-1, -1, -1, -1, -1];

            Character_Equips[uid] = character_Equip;
            Character_Equip_Save();
            Character_Equips = Character_Equip_Load();
            return Character_Equips[uid];
        }


    }

    //이후 클리어 여부, 랭킹 등등 추가
}
