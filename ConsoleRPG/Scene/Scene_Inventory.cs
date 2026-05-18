using ConsoleRPG.DTO;
using ConsoleRPG.UI;
using System;
using System.Collections.Generic;
using System.Text;

using API = ConsoleRPG.ApiGateway;
using Ui = ConsoleRPG.UI.UI;
using Dm = ConsoleRPG.Utility.DataManager;

namespace ConsoleRPG.Scene
{
    static class Scene_Inventory
    {
        public static void Sell_Start(Player player, API.ApiGateway api, List<long> inven)
        {
            int select = -1;

            while (true)            
            {
                Console.Clear();
                Ui.Info_Player();

                Console.SetCursorPosition(0, 7);
                Ui.Info_Inventory();

                Console.SetCursorPosition(0, 0);
                Console.SetCursorPosition(0, 4);
                Console.Write("판매를 원하는 아이템의 숫자 입력. (그외는 나가기)  ");
                int.TryParse(Console.ReadLine(), out select);

                if (0 < select && select <= inven.Count)
                {
                    if (player.Equipments[inven[select-1]].Equip_Character_Uid == -1)                    
                    {
                        (player.Gold, player.Equipments) = api.Item_Sell(inven[select-1]);
                        Dm.Re_Inven(player, inven);
                    }
                    else
                    {
                        Console.WriteLine("장비 중인 아이템은 팔수 없습니다.");
                        Thread.Sleep(750);
                    }
                    Console.Clear();
                }
                else break;
            }
        }
    }
}
