using System;
using System.Collections.Generic;
using System.Text;

using ConsoleRPG.DTO;
using ConsoleRPG.Utility;


namespace ConsoleRPG.ApiGateway.Stage
{
    class StageAS
    {
        public long Account_Uid { get; set; }
        private DB.StageDB GDB { get; set; }

        public StageAS(long account_Uid)
        {
            GDB = new();
            Account_Uid = account_Uid;
        }

        public Account_Location GetAccount_Location()
        {
            return GDB.GetAccount_Location(Account_Uid);
        }

        public Account_Location Location_Update(int location)
        {
            GDB.Account_Location_Update(Account_Uid, location);
            return GetAccount_Location();
        }


        public Character_Equip GetCharacter_Equip(long character_Uid)
        {
            return GDB.GetCharacter_Equip(character_Uid);
        }

        public Character_Equip Character_Equip_Update(Character_Equip equip)
        {
            GDB.Character_Equip_Update(equip.Character_Uid, equip);
            return GetCharacter_Equip(equip.Character_Uid);
        }

        public Character_Equip Character_Equip_Add(Character character)
        {            
            return GDB.Character_Equip_Insert(character.Uid);
        }



        public List<Monster> GetMonster_List(int battleNo) // 전투 발생 시, 몬스터 리스트 제공
        {

            List<Monster> monsters = new();

            for (int i = 0; i < 3; i++)
            {
                monsters.Add(new Monster(i+1,2010001, 20001, 1));
            }

            return monsters;
        }
                
        public (int, int, int, List<int>) Getbattle_Reward(int battleNo) // 클리어 보상 처리
        {
            List<int> equipments = new();

            List<int> chance = new List<int> { 12500, 25000, 37500, 50000, 62500, 75000, 87500, 100000 };
            List<int> itemNo = new List<int> { 10201001, 10202001, 10203001, 10204001, 20200001, 30200001, 40200001, 50200001 };

            equipments.Add(itemNo[Util.Gacha(chance)]);
            equipments.Add(itemNo[Util.Gacha(chance)]);

            return (1000,100, 0, equipments);
        }


        public bool Equip_Character(long uid, Equipment equipment) // 캐릭터별 장비 장착/해제 처리
        {
            Character_Equip temp = GetCharacter_Equip(uid);
                        
            if (temp.Equip_Equipment_Uid[equipment.Equip_Slot-1] == equipment.Uid)
            {
                temp.Equip_Equipment_Uid[equipment.Equip_Slot-1] = -1;
                GDB.Character_Equip_Update(uid, temp);
                return true;
            }
            else if (temp.Equip_Equipment_Uid[equipment.Equip_Slot-1] == -1)
            {
                temp.Equip_Equipment_Uid[equipment.Equip_Slot-1] = equipment.Uid;
                GDB.Character_Equip_Update(uid, temp);
                return true;
            }
            return false;
            
        }

    }
}
