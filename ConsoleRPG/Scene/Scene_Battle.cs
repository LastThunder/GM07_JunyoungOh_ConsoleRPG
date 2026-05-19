using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using ConsoleRPG.DTO;
using ConsoleRPG.UI;
using ConsoleRPG.Utility;

using API = ConsoleRPG.ApiGateway;
using Dm = ConsoleRPG.Utility.DataManager;
using Ui = ConsoleRPG.UI.UI;

namespace ConsoleRPG.Scene
{
    static class Scene_Battle
    {
        public static void Battle_Start(Player player, API.ApiGateway api, int battleNo = 3010101)
        {
            //초기화            
            List<Monster> monsters = api.GetBattleInfo(battleNo);
            Dictionary<long, Character_Status> Monsters_status = new();
            Dm.Monster_Status(Monsters_status, monsters);

            List<Battle_Character> units = new();
            foreach (var key in player.Characters_Status.Keys) units.Add(new Battle_Character(key, Util.DeepCopy(player.Characters_Status[key]), 1));
            foreach (var key in Monsters_status.Keys) units.Add(new Battle_Character(key, Util.DeepCopy(Monsters_status[key]), 2));
                       

            Stack<int> move_units = new();
            List<int> select_units = new();

            int isTurn = 2;
            int isPlayer = 0;
            int isClear = Battle_Clear_Check(units);
            Random rand = new();


            Console.Clear();
            Ui.Info_Player();

            while (isClear > 1)
            {
                Console.SetCursorPosition(0,5);
                Battle_Map(units);

                if (move_units.Count == 0) // 아군/적군 턴에 따른 동작 순위 선택
                {                    
                    if (isTurn > 1) isTurn = 1;
                    else isTurn = 2;                    

                    foreach (var pair in units.Select((unit, index) => (unit, index))
                          .Where(p => p.unit.Stat.Hp_cur > 0 && p.unit.Faction == isTurn).OrderBy(p => p.unit.Stat.Agi_total))
                        { move_units.Push(pair.index); } // 민첩이 빠른 순서로 동작 (민첩성 내림차순을 스텍 추가)

                }

                Console.SetCursorPosition(0, 20);
                if (isTurn > 1) Console.WriteLine("[적군 턴]");
                else Console.WriteLine("[아군 턴]");                

                isPlayer = move_units.Pop(); //플레이 유닛 선택
                if (units[isPlayer].Faction < 2)
                {
                    //행동선택
                    //for (int i = 0; i < units[isPlayer].Stat.SkillList_total.Count; i++)
                    //{
                    //    Console.Write($"{units[isPlayer].Stat.SkillList_total[i]}    ");                        
                    //}
                    //Console.ReadLine(); // 추후 스킬 넘버 선택

                    select_units.Clear();
                    select_units.AddRange(units.Select((unit, index) => (unit, index))
                        .Where(x => x.unit.Stat.Hp_cur > 0 && x.unit.Faction != units[isPlayer].Faction).Select(x => x.index));

                    //행동실행                    
                    Attack(units, isPlayer, select_units[rand.Next(select_units.Count)]);                    
                }
                else //npc 동작
                {
                    //추후 스킬 자동 선택

                    select_units.Clear();
                    select_units.AddRange(units.Select((unit, index) => (unit, index))
                        .Where(x => x.unit.Stat.Hp_cur > 0 && x.unit.Faction != units[isPlayer].Faction).Select(x => x.index));
                                        
                    Attack(units, isPlayer, select_units[rand.Next(select_units.Count)]);
                }
                Thread.Sleep(750);

                isClear = Battle_Clear_Check(units);
            }



            //승패 처리
            if (isClear > 0)
            {
                Console.WriteLine("승리!! 보상을 획득 하였습니다.");

                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Faction == 1)
                    {
                        player.Characters_Status[units[i].Uid].Hp_cur = units[i].Stat.Hp_cur;
                    }
                }
                api.BattleClear(player.Account.Hash,battleNo);
                api.Player_Init(player);
                Dm.Re_Status(player);
            }
            else
            {
                Console.WriteLine("패배하였습니다.");
            }
            Thread.Sleep(1000);

        }


        private static int Battle_Clear_Check(List<Battle_Character> units)
        {
            int count_ally_live = 0;
            int count_enemy_live = 0;

            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Stat.Hp_cur > 0)
                {
                    if (units[i].Faction == 1) count_ally_live++;
                    else count_enemy_live++;
                }
            }

            if (count_enemy_live < 1) return 1;
            else if (count_ally_live > 0) return 2;
            return 0;
        }

        private static void Attack(List<Battle_Character> units, int a, int b)
        {
            int damage = units[a].Stat.Str_total * (1 + (units[a].Stat.Element_Atk_total[0] - units[b].Stat.Element_Def_total[0]) / 100);

            Console.WriteLine();
            Console.WriteLine($"{units[a].Stat.Name} 이  {units[b].Stat.Name} 를 공격합니다.");
            Console.Write($"{damage} 의 공격!!  {units[b].Stat.Name}의 체력이  {units[b].Stat.Hp_cur} -> ");            
            
            units[b].Stat.Hp_cur = units[b].Stat.Hp_cur - damage;
            if (units[b].Stat.Hp_cur < 0) units[b].Stat.Hp_cur = 0;

            Console.WriteLine($"{units[b].Stat.Hp_cur} 이 되었습니다.");
            Console.WriteLine();
        }

        private static void Battle_Map(List<Battle_Character> units)
        {
            int ally_x = 2, ally_y = 5;
            int enemy_x = 30, enemy_y = 5;

            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Faction == 1)
                {
                    Console.SetCursorPosition(ally_x, ally_y);
                    Console.Write($"{units[i].Stat.Name}");
                    Console.SetCursorPosition(ally_x, ally_y + 1);
                    Console.Write($"({units[i].Stat.Hp_cur}/{units[i].Stat.Hp_max})");

                    ally_y += 4;
                }
                else
                {
                    Console.SetCursorPosition(enemy_x, enemy_y);
                    Console.Write($"{units[i].Stat.Name}");
                    Console.SetCursorPosition(enemy_x, enemy_y + 1);
                    Console.Write($"({units[i].Stat.Hp_cur}/{units[i].Stat.Hp_max})");

                    enemy_y += 4;
                }
            }
        }


    }
}
