using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG.DTO
{
    class Account // 계정(로그인 관련) 정보 (유저)
    {
        public long Uid { get; set; }
        public string Id { get; set; }
        public string Pw { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }

        public Account(long uid, string id, string pw, string name, string hash)
        {
            Uid = uid;
            Id = id;
            Pw = pw;
            Name = name;
            Hash = hash;
        }
    
    }

    class Account_Jewel // 과금재화 정보 (상점)
    {
        public long Account_Uid { get; set; }
        public int Jewel_Free { get; set; }
        public int Jewel_Money { get; set; }

        public Account_Jewel(long account_Uid, int jewel_free, int jewel_money)
        {
            Account_Uid = account_Uid;
            Jewel_Free = jewel_free;
            Jewel_Money = jewel_money;
        }
        
    }

    class Account_Gold // 게임재화 정보 (아이템)
    {
        public long Account_Uid { get; set; }
        public long Gold { get; set; }

        public Account_Gold(long account_Uid, long gold)
        {
            Account_Uid = account_Uid;
            Gold = gold;
        }        
    }

    class Account_Location // 진행 위치 정보 (스테이지)
    {
        public long Account_Uid { get; set; }
        public int StageNo { get; set; }

        public Account_Location(long account_Uid, int stageNo)
        {
            Account_Uid = account_Uid;
            StageNo = stageNo;
        }
    }
    
        
    class Player // 플레이용 통합 정보 (클라)
    {
        public Account Account { get; set; }
        public Account_Jewel Jewel { get; set; }
        public Account_Gold Gold { get; set; }
        public Account_Location Location { get; set; }
        public Dictionary<long, Character> Characters { get; set; }
        public Dictionary<long, Character_Equip> Characters_Equip { get; set; }
        public Dictionary<long, Character_Status> Characters_Status { get; set; }
        public Dictionary<long, Equipment> Equipments { get; set; }        

        public Player() { }
    }


}
