using ConsoleRPG.DTO;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dm=ConsoleRPG.Utility.DataManager;

namespace ConsoleRPG.ApiGateway.User
{
    class UserAS
    {
        private DB.UserDB UDB { get; set; }

        private readonly string secretKey = "ConsoleRPG에오신것을환영합니다ConsoleRPG에오신것을환영합니다"; // 원하는거 아무거나 (32byte 이상)
        private readonly string issuer = "서버에서 셋팅한 issuer"; // 원하는거 아무거나 (중복 안되는게 좋음 ex. 도메인 등)
        private readonly string audience = "서버에서 셋팅한 Audience";  // 상동


        public UserAS ()
        {
            UDB = new();
        }

        private string GenerateJwtToken(string userId, string role) //role 은 계정 권한
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId), // 유저 uid
                new Claim(ClaimTypes.Role, role) // 계정 권한 수준
            }),
                Expires = DateTime.UtcNow.AddDays(7), // 모바일 게임 환경을 고려한 만료 시간 설정
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
           

        public string Account_Login(Account account)
        {
            Account temp = UDB.Account_select(account.Id) ?? UDB.Account_Insert(account);
            if (temp.Pw == account.Pw)
            {
                return GenerateJwtToken(temp.Uid.ToString(), "player"); // 계정 권한 수준
            }
            return "-1";
        }


        public Dictionary<long, Character> GetCharacter(long account_Uid)
        {            
            return UDB.GetCharacter(account_Uid);
        }

        public Character Character_Add(long account_Uid, int typeNo = 1002001, string name ="")
        {
            if (name == "") name = Dm.Char_Type[typeNo].Name; // 추후 디폴트 찾아서 입력

            Character character = new(-1, account_Uid, typeNo, Dm.Char_Type[typeNo].JobNo, name, 1, 0);
            return UDB.Character_Insert(character);
        }

        public Dictionary<long, Character> Exp_up(long account_Uid, int exp)
        {
            int exp_Lv = 0;
            Dictionary<long, Character> characters = GetCharacter(account_Uid);

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
            return GetCharacter(account_Uid);
        }
    }
}
