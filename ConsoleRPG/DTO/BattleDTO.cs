using ConsoleRPG.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleRPG.DTO
{
    class Monster // 전투 용 몬스터 기본 정보 (스테이지)
    {
        public long Uid { get; set; }
        public int Character_TypeNo { get; set; }
        public int JobNo { get; set; }
        public int Level { get; set; }

        public Monster(long uid, int character_typeNo, int jobNo, int level)
        {
            Uid = uid;
            Character_TypeNo = character_typeNo;
            JobNo = jobNo;
            Level = level;
        }
    }

    class Battle_Character
    {
        public long Uid { get; set; }
        public Character_Status Stat { get; set; }
        public int Faction { get; set; }

        public int Condition { get; set; }
        public int Condition_turn { get; set; }

        public List<int> Buff_No { get; set; }
        public List<int> Buff_turn { get; set; }

        public Battle_Character (long uid, Character_Status stat, int faction)
        {
            Uid = uid;
            Stat = stat;
            Faction = faction;

            if (Stat.Hp_cur > 0) Condition = 1;
            else Condition = 0;

            Condition_turn = 0;

            Buff_No = new List<int>();
            Buff_turn = new List<int>();
        }

    }

    class Skill : IGetNoable<int>
    {
        public int No { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int Limit_Lv { get; set; }
        public int Limit_JobNo { get; set; }
        public int Target { get; set; }
        public int Range { get; set; }
        public int Mp_use { get; set; }

        public List<int> Atk_per { get; set; }
        public int Cri_Atk { get; set; }

        public List<int> Effect_Chance { get; set; }
        public List<int> Skill_EffectNo { get; set; }

        public Skill() { }
        public int GetNo()
        {
            return No;
        }
    }


    class Skill_Effect : IGetNoable<int>
    {
        public int No { get; set; }
        public string Name { get; set; }

        public int Type { get; set; }
        public int Turn { get; set; }
        public int ConditionNo { get; set; }

        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Str { get; set; }
        public int Wis { get; set; }
        public int Agi { get; set; }
        public int Dex { get; set; }

        public Skill_Effect() { }

        public int GetNo()
        {
            return No;
        }
    }

    class Condition : IGetNoable<int>
    {
        public int No { get; set; }
        public string Name { get; set; }

        public int IsMove { get; set; }

        public int Damage_per { get; set; }

        public List<int> Element_Def { get; set; }

        public Condition() { }

        public int GetNo()
        {
            return No;
        }
    }


}
