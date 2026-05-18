using ConsoleRPG.DTO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Dm = ConsoleRPG.Utility.DataManager;

//┌ , ─ ,  ┬ ,  ┐ , │ , ├ , ┼ , ┤ , └ ,  ─ ,  ┴ ,  ┘

namespace ConsoleRPG.UI
{
    static class UI
    {
        public static List<string> eqiup_parts = ["무기", "투구", "갑옷", "신발", "망토"];
        public static List<string> eqiup_rarity = [" N", " R", " SR", "UR", "LR"];
        public static List<string> character_elements = ["물리", "불", "얼음", "전기", "빛", "소리"];
        public static List<string> item_status = ["Hp", "Mp", "힘", "지능", "속도", "기술", "치명(%)"];

        public static Action Draw_screen { get; set; }
        public static Player Player { get; set; }
        public static List<long> Inven { get; set; }
        public static List<long> Party { get; set; }


        public static void Info_Player()
        {
            Console.WriteLine($"[ {Player.Account.Name} ]           골드 : {Player.Gold.Gold}         보석 : {Player.Jewel.Jewel_Free + Player.Jewel.Jewel_Money}");
            foreach (long key in Player.Characters.Keys)
            {
                Console.Write($"LV.{Player.Characters[key].Level} {Player.Characters[key].Name} ({Player.Characters_Status[key].Hp_cur}/{Player.Characters_Status[key].Hp_max})     ");
            }
            Console.WriteLine();
        }

        public static void Info_Character()
        {
            int count = 1;

            foreach (long key in Player.Characters.Keys)
            {
                Console.WriteLine($"[맴버 {count}] {Player.Characters[key].Name}   직업 : {Dm.Char_Job[Player.Characters[key].JobNo].Name}   LV : {Player.Characters[key].Level} ( {Player.Characters[key].Exp_Cur} )");
                //능력치 추가 필요

                Console.WriteLine($"Hp : {Player.Characters_Status[key].Hp_cur} / {Player.Characters_Status[key].Hp_max}" +
                    $"  힘 : {Player.Characters_Status[key].Str_total}  지능 : {Player.Characters_Status[key].Wis_total}" +
                    $"  속도 : {Player.Characters_Status[key].Agi_total}  기술 : {Player.Characters_Status[key].Dex_total}" +
                    $"  치명(%) : (공) {Player.Characters_Status[key].Cri_Atk_total} / (방) {Player.Characters_Status[key].Cri_Def_total}");

                Console.Write("[속성치]  ");
                for (int i = 0; i < 5; i++)
                {
                    Console.Write($"{character_elements[i]} : {Player.Characters_Status[key].Element_Atk_total[i]} / {Player.Characters_Status[key].Element_Def_total[i]}  ");
                }
                Console.WriteLine("\n");

                for (int i = 0; i < 5; i++)
                {
                    Console.Write($"({eqiup_parts[i]}) ");
                    if (Player.Characters_Equip[key].Equip_Equipment_Uid[i] == -1) Console.Write("없음   ");
                    else Console.Write($"{Dm.Item[Player.Equipments[Player.Characters_Equip[key].Equip_Equipment_Uid[i]].ItemNo].Name}  ");
                }
                Console.WriteLine("\n\n");
                count = count + 1;
            }
            count = 1;

        }

        public static void Info_Character(long key)
        {
            Console.WriteLine($"이름 : {Player.Characters[key].Name}   직업 : {Dm.Char_Job[Player.Characters[key].JobNo].Name}   LV : {Player.Characters[key].Level} ( {Player.Characters[key].Exp_Cur} )");
            //능력치 추가 필요

            Console.WriteLine($"Hp : {Player.Characters_Status[key].Hp_cur} / {Player.Characters_Status[key].Hp_max}" +
                $"  힘 : {Player.Characters_Status[key].Str_total}  지능 : {Player.Characters_Status[key].Wis_total}" +
                $"  속도 : {Player.Characters_Status[key].Agi_total}  기술 : {Player.Characters_Status[key].Dex_total}" +
                $"  치명(%) : (공) {Player.Characters_Status[key].Cri_Atk_total} / (방) {Player.Characters_Status[key].Cri_Def_total}");

            Console.Write("[속성치]  ");
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"{character_elements[i]} : {Player.Characters_Status[key].Element_Atk_total[i]} / {Player.Characters_Status[key].Element_Def_total[i]}  ");
            }
            Console.WriteLine();

            for (int i = 0; i < 5; i++)
            {
                Console.Write($"({eqiup_parts[i]}) ");
                if (Player.Characters_Equip[key].Equip_Equipment_Uid[i] == -1) Console.Write("없음   ");
                else Console.Write($"{Dm.Item[Player.Equipments[Player.Characters_Equip[key].Equip_Equipment_Uid[i]].ItemNo].Name}  ");
            }
            Console.WriteLine("\n");
        }

        public static void Info_Inventory()
        {
            int count = 1;

            Console.WriteLine($"[인벤토리 정보]");

            if (Player.Equipments.Count < 1) Console.WriteLine("없음");
            else
            {
                foreach (long key in Player.Equipments.Keys)
                {
                    Console.Write("[");
                    if (count < 10) Console.Write(" ");
                    Console.Write($"{count}]  {Dm.Item[Player.Equipments[key].ItemNo].Name}   부위 : {eqiup_parts[Player.Equipments[key].Equip_Slot - 1]}   가격 : {Dm.Item[Player.Equipments[key].ItemNo].Gold}");
                    if (Player.Equipments[key].Equip_Character_Uid != -1) Console.Write($"  [E] [{Player.Characters[Player.Equipments[key].Equip_Character_Uid].Name}]");
                    Console.WriteLine();
                    count = count + 1;
                }
            }
        }

        public static void Info_Item(List<int> item)
        {
            int count = 1;

            foreach(int temp in item)
            {
                Console.Write("[");
                if (count < 10) Console.Write(" ");
                Console.Write($"{count}]  {Dm.Item[temp].Name}   부위 : {eqiup_parts[Dm.Item[temp].Eqiup_Slot-1]}   ");
                if (Dm.Item[temp].Hp > 0) Console.Write($"{item_status[0]} +{Dm.Item[temp].Hp}   ");
                if (Dm.Item[temp].Mp > 0) Console.Write($"{item_status[1]} +{Dm.Item[temp].Mp}   ");
                if (Dm.Item[temp].Str > 0) Console.Write($"{item_status[2]} +{Dm.Item[temp].Str}   ");
                if (Dm.Item[temp].Wis > 0) Console.Write($"{item_status[3]} +{Dm.Item[temp].Wis}   ");
                if (Dm.Item[temp].Agi > 0) Console.Write($"{item_status[4]} +{Dm.Item[temp].Agi}   ");
                if (Dm.Item[temp].Dex > 0) Console.Write($"{item_status[5]} +{Dm.Item[temp].Dex}   ");
                Console.WriteLine($"가격: {Dm.Item[temp].Gold}");
                count++;
            }
            Console.WriteLine();
        }

        public static void MainMenu()
        {
            Console.WriteLine($"1. 캐릭터 (&장착)\n");
            Console.WriteLine($"2. 인벤토리 (&판매)\n");
            Console.WriteLine($"3. 상점\n");
            Console.WriteLine($"4. 가챠 & 보석충전\n");
            Console.WriteLine($"5. 게임종료\n");
        }



    }
}
