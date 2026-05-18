using ConsoleRPG.DTO;
using System;
using System.Collections.Generic;
using System.Text;

using API = ConsoleRPG.ApiGateway;
using Ui = ConsoleRPG.UI.UI;
using Dm = ConsoleRPG.Utility.DataManager;


namespace ConsoleRPG.Scene
{
    static class Scene_Shop
    {
        public static void Buy_Start(Player player, API.ApiGateway api)
        {
            int select = -1;
            int select_num = -1;
            List<int> item = new();

            while (true)
            {
                Console.Clear();
                Ui.Info_Player();
                Console.WriteLine();

                Console.WriteLine("1.무기    2.투구    3.갑옷    4.신발    5.망토");
                Console.Write("원하는 아이템의 종류를 골라 주세요. (그외는 나가기)  ");
                int.TryParse(Console.ReadLine(), out select);

                if (0 < select && select < 6)
                {
                    item.Clear();
                    foreach (int temp in Dm.Item.Keys)
                    {
                        if (Dm.Item[temp].Eqiup_Slot== select)   item.Add(temp);
                    }

                    Console.Clear();
                    while (true)
                    {
                        Ui.Info_Player();
                        Console.SetCursorPosition(0, 7);
                        Ui.Info_Item(item);

                        Console.SetCursorPosition(0, 0);
                        Console.SetCursorPosition(0, 4);
                        Console.Write("구매할 장비의 번호를 선택해 주세요. (그외는 나가기)  ");
                        int.TryParse(Console.ReadLine(), out select_num);

                        if (0 < select_num && select_num <= item.Count)
                        {
                            if (player.Gold.Gold >= Dm.Item[item[select_num-1]].Gold)
                            {
                                (player.Gold, player.Equipments) = api.Item_buy(item[select_num - 1]);
                                Console.WriteLine($"{Dm.Item[item[select_num-1]].Name}  구매 완료! 인벤토리에서 확인해 주세요.");
                                Thread.Sleep(750);
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족 합니다.");
                                Thread.Sleep(750);
                            }
                            Console.Clear();
                        }
                        else break;
                        Console.Clear();
                    }
                    Console.Clear();
                }
                else break;
            }

        }
    }
}
