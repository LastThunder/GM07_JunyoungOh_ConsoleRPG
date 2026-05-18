using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.Json;

using ConsoleRPG.DTO;
using API = ConsoleRPG.ApiGateway;

namespace ConsoleRPG.Utility
{
    static class DataManager
    {        
        public static Dictionary<int, Character_Type> Char_Type = Util.Json_Data_Load<Character_Type>("Data\\Character_Type.txt");
        public static Dictionary<int, Character_Job> Char_Job = Util.Json_Data_Load<Character_Job>("Data\\Character_Job.txt");
        public static Dictionary<int, Item> Item = Util.Json_Data_Load<Item>("Data\\Item.txt");
        public static Dictionary<int, Skill> Skill = Util.Json_Data_Load<Skill>("Data\\Skill.txt");
        public static Dictionary<int, Skill_Effect> Skill_Effect = Util.Json_Data_Load<Skill_Effect>("Data\\Skill_Effect.txt");
        public static Dictionary<int, Condition> Condition = Util.Json_Data_Load<Condition>("Data\\Condition.txt");


        public static void Re_Inven(Player player, List<long> inven)
        {
            inven.Clear();
            foreach (long key in player.Equipments.Keys)    inven.Add(key);
        }

        public static void Re_Party(Player player, List<long> party)
        {
            party.Clear();
            foreach (long key in player.Characters.Keys) party.Add(key);
        }

        public static void Re_Status(Player player, int reset = 0) // 캐릭별 상세 정보 갱신
        {
            //캐릭터 타입 반영
            foreach (long key in player.Characters.Keys)
            {
                int tempNo = player.Characters[key].TypeNo;
                if (!player.Characters_Status.ContainsKey(key)) player.Characters_Status[key] = new();

                player.Characters_Status[key].Name = player.Characters[key].Name;

                player.Characters_Status[key].Hp_max = Char_Type[tempNo].Hp;
                player.Characters_Status[key].Mp_max = Char_Type[tempNo].Mp;
                player.Characters_Status[key].Str_total = Char_Type[tempNo].Str;
                player.Characters_Status[key].Wis_total = Char_Type[tempNo].Wis;
                player.Characters_Status[key].Agi_total = Char_Type[tempNo].Agi;
                player.Characters_Status[key].Dex_total = Char_Type[tempNo].Dex;
                player.Characters_Status[key].Element_Atk_total = Util.DeepCopy(Char_Type[tempNo].Element_Atk);
                player.Characters_Status[key].Element_Def_total = Util.DeepCopy(Char_Type[tempNo].Element_Def);
                player.Characters_Status[key].SkillList_total = Util.DeepCopy(Char_Type[tempNo].SkillList);

                player.Characters_Status[key].Cri_Atk_total = 0;
                player.Characters_Status[key].Cri_Def_total = 0;

                player.Characters_Status[key].Gold_total = Char_Type[tempNo].Gold;
                player.Characters_Status[key].Exp_total = Char_Type[tempNo].Exp;
            }

            //직업별 LV 상승치 적용
            foreach (long key in player.Characters.Keys)
            {
                int tempNo = player.Characters[key].JobNo;
                int tempLv = player.Characters[key].Level;

                player.Characters_Status[key].Hp_max += Char_Job[tempNo].Hp_perLv * tempLv;
                player.Characters_Status[key].Mp_max += Char_Job[tempNo].Mp_perLv * tempLv;
                player.Characters_Status[key].Str_total += Char_Job[tempNo].Str_perLv * tempLv;
                player.Characters_Status[key].Wis_total += Char_Job[tempNo].Wis_perLv * tempLv;
                player.Characters_Status[key].Agi_total += Char_Job[tempNo].Agi_perLv * tempLv;
                player.Characters_Status[key].Dex_total += Char_Job[tempNo].Dex_perLv * tempLv;

                for (int i = 0; i < 6; i++) {
                    player.Characters_Status[key].Element_Atk_total[i] += Char_Job[tempNo].Element_Atk[i];
                    player.Characters_Status[key].Element_Def_total[i] += Char_Job[tempNo].Element_Def[i];
                }//속성 개수가 늘어나면 나중에 추가

                player.Characters_Status[key].Gold_total *= (1 + Char_Job[tempNo].Gold_perLv * tempLv/100);
                player.Characters_Status[key].Exp_total *= (1 + Char_Job[tempNo].Exp_perLv * tempLv/100);
            }

            //장비 더하기
            foreach (var key in player.Characters.Keys)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (player.Characters_Equip[key].Equip_Equipment_Uid[i] > 0)
                    {
                        int tempNo = player.Equipments[player.Characters_Equip[key].Equip_Equipment_Uid[i]].ItemNo;

                        player.Characters_Status[key].Hp_max += Item[tempNo].Hp;
                        player.Characters_Status[key].Mp_max += Item[tempNo].Mp;
                        player.Characters_Status[key].Str_total += Item[tempNo].Str;
                        player.Characters_Status[key].Wis_total += Item[tempNo].Wis;
                        player.Characters_Status[key].Agi_total += Item[tempNo].Agi;
                        player.Characters_Status[key].Dex_total += Item[tempNo].Dex;


                        if (Item[tempNo].Element_Atk[0] != -1)
                        {                            
                            for (int j = 0; j < 6; j++)
                            {
                                player.Characters_Status[key].Element_Atk_total[j] += Item[tempNo].Element_Atk[j];
                            }
                        }
                        if (Item[tempNo].Element_Def[0] != -1)
                        {                            
                            for (int j = 0; j < 6; j++)
                            {
                                player.Characters_Status[key].Element_Def_total[j] += Item[tempNo].Element_Def[j];
                            }
                        }

                        if (Item[tempNo].Cri > 0) player.Characters_Status[key].Cri_Atk_total += Item[tempNo].Cri;
                        else player.Characters_Status[key].Cri_Def_total += Item[tempNo].Cri;
                    }//속성 개수가 늘어나면 나중에 추가                        
                }                
            }

            if (reset > 0)
            {
                foreach (var key in player.Characters.Keys)
                {
                    player.Characters_Status[key].Hp_cur = player.Characters_Status[key].Hp_max;
                    player.Characters_Status[key].Mp_cur = player.Characters_Status[key].Mp_max;
                }
            }
            //현재 hp,mp max에 맞출지 말지
        }

        public static void Monster_Status(Dictionary<long,Character_Status> status, List<Monster> monsters) //몬스터 상세 정보 갱신
        {
            status.Clear();

            for (int i = 0; i < monsters.Count; i++)
            {
                status[monsters[i].Uid] = new();

                int tempNo = monsters[i].Character_TypeNo;
                int tempJobNo = monsters[i].JobNo;
                int tempLv = monsters[i].Level;

                status[monsters[i].Uid].Name = Char_Type[tempNo].Name;

                status[monsters[i].Uid].Hp_max = Char_Type[tempNo].Hp + Char_Job[tempJobNo].Hp_perLv * tempLv;
                status[monsters[i].Uid].Mp_max = Char_Type[tempNo].Mp + Char_Job[tempJobNo].Mp_perLv * tempLv;
                status[monsters[i].Uid].Str_total = Char_Type[tempNo].Str + Char_Job[tempJobNo].Str_perLv * tempLv;
                status[monsters[i].Uid].Wis_total = Char_Type[tempNo].Wis + Char_Job[tempJobNo].Wis_perLv * tempLv;
                status[monsters[i].Uid].Agi_total = Char_Type[tempNo].Agi + Char_Job[tempJobNo].Agi_perLv * tempLv;
                status[monsters[i].Uid].Dex_total = Char_Type[tempNo].Dex + Char_Job[tempJobNo].Dex_perLv * tempLv;
                status[monsters[i].Uid].Element_Atk_total = Util.DeepCopy(Char_Type[tempNo].Element_Atk);
                status[monsters[i].Uid].Element_Def_total = Util.DeepCopy(Char_Type[tempNo].Element_Def);
                status[monsters[i].Uid].SkillList_total = Util.DeepCopy(Char_Type[tempNo].SkillList);

                status[monsters[i].Uid].Gold_total = Char_Type[tempNo].Gold * (1+ Char_Job[tempJobNo].Gold_perLv * tempLv/100);
                status[monsters[i].Uid].Exp_total = Char_Type[tempNo].Exp * (1+ Char_Job[tempJobNo].Exp_perLv * tempLv/100);

                for (int j = 0; j < 6; j++)
                {
                    status[monsters[i].Uid].Element_Atk_total[j] += Char_Job[tempJobNo].Element_Atk[j];
                    status[monsters[i].Uid].Element_Def_total[j] += Char_Job[tempJobNo].Element_Def[j];
                }//속성 개수가 늘어나면 나중에 추가

                status[monsters[i].Uid].Cri_Atk_total = 0;
                status[monsters[i].Uid].Cri_Def_total = 0;


                for (int k =0; k < 5; k++)
                {
                    int tempItemNo = Char_Type[tempNo].EquipList[k];

                    if (tempItemNo != 0)
                    {
                        status[monsters[i].Uid].Hp_max += Item[tempItemNo].Hp;
                        status[monsters[i].Uid].Mp_max += Item[tempItemNo].Mp;
                        status[monsters[i].Uid].Str_total += Item[tempItemNo].Str;
                        status[monsters[i].Uid].Wis_total += Item[tempItemNo].Wis;
                        status[monsters[i].Uid].Agi_total += Item[tempItemNo].Agi;
                        status[monsters[i].Uid].Dex_total += Item[tempItemNo].Dex;

                        if (Item[tempItemNo].Element_Atk[0] != -1)
                        {
                            for (int j = 0; j < 6; j++) status[monsters[i].Uid].Element_Atk_total[j] += Item[tempItemNo].Element_Atk[j];
                        }

                        if (Item[tempItemNo].Element_Def[0] != -1)
                        {
                            for (int j = 0; j < 6; j++) status[monsters[i].Uid].Element_Def_total[j] += Item[tempItemNo].Element_Def[j];
                        }

                        if (Item[tempItemNo].Cri > 0) status[monsters[i].Uid].Cri_Atk_total += Item[tempItemNo].Cri;
                        else status[monsters[i].Uid].Cri_Def_total += Item[tempItemNo].Cri;
                    }
                }

                status[monsters[i].Uid].Hp_cur = status[monsters[i].Uid].Hp_max;
                status[monsters[i].Uid].Mp_cur = status[monsters[i].Uid].Mp_max;

            }
        }


    }
}