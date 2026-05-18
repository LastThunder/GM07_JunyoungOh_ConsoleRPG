using System;
using System.Collections.Generic;
using System.Text;

using ConsoleRPG.DTO;
using ConsoleRPG.Scene;
using API = ConsoleRPG.ApiGateway;
using Ui = ConsoleRPG.UI.UI;
using Dm = ConsoleRPG.Utility.DataManager;
using System.Runtime.InteropServices;

namespace ConsoleRPG.Scene
{
    internal class Scene_Menu
    {
        public static bool Menu_Open(Player player, API.ApiGateway api, List<long> party, List<long> inven)
        {
            bool exit = false;
            ConsoleKeyInfo keyInfo;

            while (!exit)
            {
                api.Player_Init(player);
                Dm.Re_Status(player);
                Dm.Re_Inven(player, inven);
                Dm.Re_Party(player, party);

                Ui.Info_Player();

                Console.WriteLine();
                Console.WriteLine();
                Ui.MainMenu();

                Console.WriteLine();
                Console.Write($"메뉴를 선택 하세요. (취소 esc)");
                keyInfo = Console.ReadKey(true); // 입력받기

                if (keyInfo.Key == ConsoleKey.D1) Scene_PartyInfo.Equip_Start(player, api, party, inven);
                if (keyInfo.Key == ConsoleKey.D2) Scene_Inventory.Sell_Start(player, api, inven);
                if (keyInfo.Key == ConsoleKey.D3)
                {
                    Scene_Shop.Buy_Start(player, api);
                    Dm.Re_Inven(player, inven);
                }
                if (keyInfo.Key == ConsoleKey.D4)
                {
                    Console.WriteLine();
                    api.Buy_Product(1);
                    api.Buy_Product(2);
                    Console.WriteLine($"보석 충전 후 10회 가챠 진행하였습니다. 인벤토리를 확인해 주세요.");
                    Thread.Sleep(1000);
                }
                if (keyInfo.Key == ConsoleKey.D5)
                {
                    exit = true;
                    break;
                }
                if (keyInfo.Key == ConsoleKey.Escape) break;
                Console.Clear();
            }
            Console.Clear();
            return exit;            
        }
    }
}
