using ConsoleRPG.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG.DTO
{
    class Equipment // 생성 장비 정보 (아이템)
    {
        public long Uid { get; set; }
        public long Account_Uid { get; set; }
        public int ItemNo { get; set; }        
        public int Equip_Slot { get; set; }
        public long Equip_Character_Uid { get; set; }

        public Equipment(long uid, long account_Uid, int itemNo, int equip_slot)
        {
            Uid = uid;
            Account_Uid = account_Uid;
            ItemNo = itemNo;
            Equip_Slot = equip_slot;
            Equip_Character_Uid = -1;
        }
    }

    class Item : IGetNoable<int> // 각 장비별 세부 정보 (csv)
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int Limit_Lv { get; set; }
        public int Limit_JobNo { get; set; }
        public int Grade { get; set; }
        public int Eqiup_Slot { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Str { get; set; }
        public int Wis { get; set; }
        public int Agi { get; set; }
        public int Dex { get; set; }
        public List<int> Element_Atk { get; set; }
        public List<int> Element_Def { get; set; }
        public int Cri {  get; set; }
        public int Gold { get; set; }

        public Item() { }

        public int GetNo()
        {
            return No;
        }

    }


}
