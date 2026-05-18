using System;
using System.Collections.Generic;
using System.Text;

using ConsoleRPG.DTO;
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

        public ApiGateway(string hash)
        {
            UAS = new(hash);
            IAS = new(UAS.Account_Uid);
            GAS = new(UAS.Account_Uid);
            SAS = new(UAS.Account_Uid);
        }

        public void Player_Init(Player player) // 플레이어 정보 최신화
        {
            player.Gold = IAS.GetAccount_Gold();
            player.Jewel = SAS.GetAccount_Jewel();
            player.Location = GAS.GetAccount_Location();
            player.Characters = UAS.GetCharacter();

            player.Characters_Equip = new();
            foreach (long key in player.Characters.Keys)
            {
                player.Characters_Equip[key] = GAS.GetCharacter_Equip(key);
            }
            player.Equipments = IAS.GetEquipment();
        }

        public void PartyJoin(int typeNo = 1002001, string name ="") // 파티에 캐릭터 추가
        {
            Character temp = UAS.Character_Add(typeNo, name);
            GAS.Character_Equip_Add(temp);
        }


        public void Item_Equip(long equip_Character_Uid, long item_Uid) //장비 장착
        {
            if (GAS.Equip_Character(equip_Character_Uid, IAS.GetEquipment()[item_Uid])) IAS.Equip_Item(item_Uid, equip_Character_Uid);
        }

        public (Account_Gold, Dictionary<long, Equipment>) Item_Sell(long item_Uid) // 선택한 아이템 판매
        {
            return IAS.Sell_Equip(item_Uid);
        }
        public (Account_Gold, Dictionary<long, Equipment>) Item_buy(int item_No)
        {
            return (IAS.Gold_Update(-Dm.Item[item_No].Gold), IAS.Equipment_Insert(item_No));
        }


        public Account_Jewel Buy_Product(int productNo=1)
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
        
        public void BattleClear(int battleNo = 3010101)
        {
            var (clear_gold, clear_exp, clear_jewel_free, equipments) = GAS.Getbattle_Reward(battleNo);
            //스테이지 서버에 클리어 보상 요청
            
            //각 서버에 보상 지급 요청
            UAS.Exp_up(clear_exp);
            IAS.Gold_Update(clear_gold);

            if (clear_jewel_free>0) SAS.Jewel_Free_Add(clear_jewel_free); // 최초클리어시 무료 보석

            foreach (var item in equipments)
            {
                IAS.Equipment_Insert(item);
            }

        }
        
    }
}
