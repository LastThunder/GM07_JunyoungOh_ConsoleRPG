using ConsoleRPG.DTO;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Dm = ConsoleRPG.Utility.DataManager;
using GS = ConsoleRPG.ApiGateway.Stage;
using IS = ConsoleRPG.ApiGateway.Item;
using SS = ConsoleRPG.ApiGateway.Shop;
using US = ConsoleRPG.ApiGateway.User;


namespace ConsoleRPG.ApiGateway
{
    class ApiGateway
    {
        public US.UserAS UAS { get; set; }
        public IS.ItemAS IAS { get; set; }
        public GS.StageAS GAS { get; set; }
        public SS.ShopAS SAS { get; set; }

        public Dictionary<string, (long uid, string role)> Auth_Pass_List { get; set; }

        private readonly string secretKey = "ConsoleRPG에오신것을환영합니다ConsoleRPG에오신것을환영합니다"; // 원하는거 아무거나 (32byte 이상)
        private readonly string issuer = "서버에서 셋팅한 issuer"; // 원하는거 아무거나 (중복 안되는게 좋음 ex. 도메인 등)
        private readonly string audience = "서버에서 셋팅한 Audience";  // 상동

        public ApiGateway()
        {
            UAS = new();
            Auth_Pass_List = new();
        }

        public Account Player_Account_Init(Account account)
        {
            if (!Auth_Pass_List.ContainsKey(account.Hash)) account.Hash = UAS.Account_Login(account);            
            account.Uid = Player_Check_Hash(account.Hash);
            IAS = new(account.Uid);
            GAS = new(account.Uid);
            SAS = new(account.Uid);
            Console.WriteLine($"비밀번호가 맞지 않습니다.");
            return account;
        }

        public long Player_Check_Hash(string token) // 토큰 인증 (문제 없으면 uid 추출)
        {
            if (Auth_Pass_List.ContainsKey(token)) return Auth_Pass_List[token].uid;
            else
            {
                var principal = DecodeJwtToken(token);
                if (principal == null)
                {
                    Auth_Pass_List.Remove(token);
                    return -1;  // 타이틀 (토큰 생성 화면) 로 날려버림
                }
                
                string account_Uid = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value; // UserId 추출
                long uid = long.Parse(account_Uid);                
                string role = principal.FindFirst(ClaimTypes.Role)?.Value; // 권한 추출 (ClaimTypes.Role)

                Auth_Pass_List[token] = (uid, role);

                Console.WriteLine($"UserId: {uid}, Role: {role}");

                return uid;
            }
        }
        private ClaimsPrincipal DecodeJwtToken(string token) //인증 해쉬 (토큰) 복호
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true, // 만료 시간(Expires) 검증 활성화
                    ClockSkew = TimeSpan.Zero // 시간 오차 허용 안 함 (즉시 만료 처리)
                };

                // 토큰 검증 및 복호화 (유효하지 않으면 예외 발생)
                SecurityToken validatedToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                Console.WriteLine("토큰이 만료되었습니다.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"유효하지 않은 토큰입니다: {ex.Message}");
                return null;
            }
        }





        public void Player_Init(Player player) // 플레이어 정보 최신화
        {
            player.Gold = IAS.GetAccount_Gold();
            player.Jewel = SAS.GetAccount_Jewel();
            player.Location = GAS.GetAccount_Location();
            player.Characters = UAS.GetCharacter(Player_Check_Hash(player.Account.Hash));

            player.Characters_Equip = new();
            foreach (long key in player.Characters.Keys)
            {
                player.Characters_Equip[key] = GAS.GetCharacter_Equip(key);
            }
            player.Equipments = IAS.GetEquipment();
        }


        public void PartyJoin(string token, int typeNo = 1002001, string name ="") // 파티에 캐릭터 추가
        {
            Character temp = UAS.Character_Add(Player_Check_Hash(token), typeNo, name);
            GAS.Character_Equip_Add(temp);
        }
        public void Item_Equip(long equip_Character_Uid, long item_Uid) //장비 장착
        {
            if (GAS.Equip_Character(equip_Character_Uid, IAS.GetEquipment()[item_Uid])) IAS.Equip_Item(item_Uid, equip_Character_Uid);
        }


        public (Account_Gold, Dictionary<long, Equipment>) Item_Sell(long item_Uid) // 아이템 판매
        {
            return IAS.Sell_Equip(item_Uid);
        }
        public (Account_Gold, Dictionary<long, Equipment>) Item_buy(int item_No) // 아이템 구매
        {
            return (IAS.Gold_Update(-Dm.Item[item_No].Gold), IAS.Equipment_Insert(item_No));
        }


        public Account_Jewel Buy_Product(int productNo=1) //상점 (유료)
        {
            if (productNo == 1) SAS.Buy_Jewel(productNo);
            else {
                foreach (var item in SAS.Buy_Gacha(productNo))
                {
                    IAS.Equipment_Insert(item);
                }                
            }
            //추후 상점 상품 목록에서 가챠인지 보석충전 상품인지 판별

            return SAS.GetAccount_Jewel();
        }


        public List<Monster> GetBattleInfo(int battleNo = 3010101)
        {
            return GAS.GetMonster_List(battleNo);
        }
        public void BattleClear(string token, int battleNo = 3010101)
        {
            var (clear_gold, clear_exp, clear_jewel_free, equipments) = GAS.Getbattle_Reward(battleNo);
            //스테이지 서버에 클리어 보상 요청
            
            //각 서버에 보상 지급 요청
            UAS.Exp_up(Player_Check_Hash(token),clear_exp);
            IAS.Gold_Update(clear_gold);

            if (clear_jewel_free>0) SAS.Jewel_Free_Add(clear_jewel_free); // 최초클리어시 무료 보석

            foreach (var item in equipments)
            {
                IAS.Equipment_Insert(item);
            }

        }
        
    }
}
