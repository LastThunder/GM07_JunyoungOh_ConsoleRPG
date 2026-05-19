using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
//  Nuget 에서 System.IdentityModel.Tokens.Jwt  검색해서 설치

using ConsoleRPG.DTO;


namespace ConsoleRPG.ApiGateway.User.DB
{
    class UserDB
    {
        public Dictionary<long,Account> Accounts { get; private set; }
        public Dictionary<long,Character> Characters { get; private set; }        
        private long LastAccountUid { get; set; }
        private long LastCharacterUid { get; set; }

        public Dictionary<string,Account> Accounts_search_Id { get; private set; }


        public UserDB()
        {
            Accounts = Account_Load();
            Characters = Character_Load();
            LastAccountUid = Accounts.LastOrDefault().Key+1;
            LastCharacterUid = Characters.LastOrDefault().Key+1;

            Accounts_search_Id = Accounts.Values.ToDictionary(x => x.Id, x => x);
            //Accounts의 아이디가 플랫폼별로 복수가 될 경우 플렛폼이름_ID 식으로 결함한 스트링을 key 에 입력
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
                Account account = new(LastAccountUid, "1", "1", "테스트", "1");

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
                Character character = new(LastCharacterUid, 1, 1001001, 10001, "주인공", 1, 0);

                Characters = new Dictionary<long, Character>();
                Characters[LastCharacterUid] = character;

                Character_Save();
                return Characters;
            }
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<long, Character>>(jsonString);
        }


        public Account Account_select(long uid)
        {
            return Accounts[uid];
        }
        public Account Account_select(string id) // 추후 플랫폼이 복수일때 플렛폼이름_ID 로 결합한 스트링을 탐색
        {
            return Accounts_search_Id.TryGetValue(id, out var account) ? account : null;
        }



        public Account Account_Insert (Account account)
        {
            account.Uid = LastAccountUid;
            Accounts[LastAccountUid] = account;
            Accounts_search_Id[account.Id] = account;

            LastAccountUid++;
            Account_Save();
            return account;
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
