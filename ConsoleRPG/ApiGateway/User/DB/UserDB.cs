using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using ConsoleRPG.DTO;


namespace ConsoleRPG.ApiGateway.User.DB
{
    class UserDB
    {
        public Dictionary<long,Account> Accounts { get; private set; }
        public Dictionary<long,Character> Characters { get; private set; }        
        private long LastAccountUid { get; set; }
        private long LastCharacterUid { get; set; }

        public UserDB()
        {
            Accounts = Account_Load();
            Characters = Character_Load();
            LastAccountUid = Accounts.LastOrDefault().Key+1;
            LastCharacterUid = Characters.LastOrDefault().Key+1;
        }
        private void Account_Save()
        {
            string filePath = "DB\\Account.txt";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Accounts, options);
            File.WriteAllText(filePath, jsonString);
        }
        private Dictionary<long, Account> Account_Load()
        {
            string filePath = "DB\\Account.txt";
            if (!File.Exists(filePath))
            {
                LastAccountUid = 1;
                Account account = new(LastAccountUid,"1","1","테스트","1");

                Accounts = new Dictionary<long, Account>();
                Accounts[LastAccountUid] = account;

                Account_Save();
                return Accounts;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<long, Account>>(jsonString);
        }
        private void Character_Save()
        {
            string filePath = "DB\\Character.txt";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(Characters, options);
            File.WriteAllText(filePath, jsonString);
        }
        private Dictionary<long, Character> Character_Load()
        {
            string filePath = "DB\\Character.txt";
            if (!File.Exists(filePath))
            {
                LastCharacterUid = 1;
                Character character = new(LastCharacterUid, 1, 1001001, 10001,"주인공", 1, 0);

                Characters = new Dictionary<long, Character>();
                Characters[LastCharacterUid] = character;

                Character_Save();
                return Characters;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<long, Character>>(jsonString);
        }


        public long GetAccountUid(string hash)
        {
            return Accounts[Accounts.FirstOrDefault(x => x.Value.Hash == hash).Key].Uid;
        }

        public Dictionary<long, Character> GetCharacter(long account_Uid)
        {
            return Characters.Where(x=>x.Value.Account_Uid==account_Uid).ToDictionary(x=>x.Key,x=>x.Value);
        }

        public Character Character_Insert(Character character)
        {
            character.Uid = LastCharacterUid;
            Characters[LastCharacterUid] = character;            
            Character_Save();
            Characters = Character_Load();
            LastCharacterUid = LastCharacterUid + 1;
            return Characters[LastCharacterUid-1];
        }

        public bool Character_Update(Dictionary<long, Character> character)
        {
            foreach (long uid in character.Keys) {
                Characters[uid] = character[uid];
            }
            Character_Save();
            Characters = Character_Load();
            return true;
        }
    }
}
