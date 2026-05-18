using System;
using System.Collections.Generic;
using System.Text;

using ConsoleRPG.DTO;
using Dm=ConsoleRPG.Utility.DataManager;

namespace ConsoleRPG.ApiGateway.User
{
    class UserAS
    {
        public long Account_Uid { get; set; }
        private DB.UserDB UDB { get; set; }        

        public UserAS (string hash)
        {
            UDB = new();
            Account_Uid = UDB.GetAccountUid(hash);
        }

        public Dictionary<long, Character> GetCharacter()
        {            
            return UDB.GetCharacter(Account_Uid);
        }

        public Character Character_Add(int typeNo = 1002001, string name ="")
        {
            if (name == "") name = Dm.Char_Type[typeNo].Name; // 추후 디폴트 찾아서 입력

            Character character = new(-1, Account_Uid, typeNo, Dm.Char_Type[typeNo].JobNo, name, 1, 0);            
            return UDB.Character_Insert(character);
        }

        public Dictionary<long, Character> Exp_up(int exp)
        {
            int exp_Lv = 0;
            Dictionary<long, Character> characters = GetCharacter();

            foreach (var character in characters) {
                exp_Lv = Dm.Char_Type[characters[character.Key].TypeNo].Exp * (1+characters[character.Key].Level*Dm.Char_Job[characters[character.Key].JobNo].Exp_perLv/100) ;
                characters[character.Key].Exp_Cur = characters[character.Key].Exp_Cur + exp;

                while (characters[character.Key].Exp_Cur >= exp_Lv)                    
                {
                    characters[character.Key].Exp_Cur -= exp_Lv;
                    characters[character.Key].Level++;
                }
            }
            UDB.Character_Update(characters);
            return GetCharacter();
        }
    }
}
