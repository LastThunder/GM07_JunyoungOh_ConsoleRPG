using ConsoleRPG.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG.DTO
{
    class Character // 플레이어블 캐릭터 정보 (계정)
    {
        public long Uid { get; set; }
        public long Account_Uid { get; set; }
        public int TypeNo { get; set; }
        public int JobNo { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Exp_Cur { get; set; }

        public Character(long uid, long account_Uid, int typeNo, int jobNo, string name, int level, int exp_Cur)
        {
            Uid = uid;
            Account_Uid = account_Uid;
            TypeNo = typeNo;
            JobNo = jobNo;
            Name = name;
            Level = level;
            Exp_Cur = exp_Cur;
        }
        
    }

    class Character_Equip // 각 캐릭터별 장비 현황 (스테이지)
    {
        public long Character_Uid { get; set; }
        public List<long> Equip_Equipment_Uid { get; set; }

        public Character_Equip() {}
        
    }

    class Character_Status // 캐릭터 상세 정보 (클라)
    {
        public string Name { get; set; }
        public int Hp_cur { get; set; }
        public int Hp_max { get; set; }
        public int Mp_cur { get; set; }
        public int Mp_max { get; set; }
        public int Str_total { get; set; }
        public int Wis_total { get; set; }
        public int Agi_total { get; set; }
        public int Dex_total { get; set; }
        public int Cri_Atk_total { get; set; }
        public int Cri_Def_total { get; set; }
        public List<int> Element_Atk_total { get; set; }
        public List<int> Element_Def_total { get; set; }
        public List<int> SkillList_total { get; set; }

        public int Gold_total { get; set; }
        public int Exp_total { get; set; }

        public Character_Status() { }
    }


    class Character_Type : IGetNoable<int> // 캐릭터 타입 상세 정보 (data)
    {
        public int No { get; set; }
        public string Name { get; set; }
        public int JobNo { get; set; }
        public int Lv { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Str { get; set; }
        public int Wis { get; set; }
        public int Agi { get; set; }
        public int Dex { get; set; }
        public List<int> Element_Atk { get; set; }
        public List<int> Element_Def { get; set; }
        public List<int> EquipList { get; set; }
        public List<int> SkillList { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }

        public Character_Type() { }

        public int GetNo()
        {
            return No;
        }
    }

    class Character_Job : IGetNoable<int> // 캐릭터 직업 정보 (data)
    {
        public int No { set; get; }
        public string Name { set; get; }
        public int Hp_perLv { get; set; }
        public int Mp_perLv { get; set; }
        public int Str_perLv { get; set; }
        public int Wis_perLv { get; set; }
        public int Agi_perLv { get; set; }
        public int Dex_perLv { get; set; }

        public List<int> Element_Atk { get; set; }
        public List<int> Element_Def { get; set; }

        public int Exp_perLv { get; set; }
        public int Gold_perLv { get; set; }

        public Character_Job() { }

        public int GetNo()
        {
            return No;
        }
    }

    
}
