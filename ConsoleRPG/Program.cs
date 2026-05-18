using ConsoleRPG.DTO;
using ConsoleRPG.Utility;
using System.Threading.Channels;

using ConsoleRPG.Scene;
using API=ConsoleRPG.ApiGateway;
using Ui=ConsoleRPG.UI.UI;
using Dm=ConsoleRPG.Utility.DataManager;
using System.Runtime.Intrinsics.X86;

namespace ConsoleRPG
{
    internal class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            if (!Directory.Exists("DB\\")) Directory.CreateDirectory("DB\\");

            Player player = new();
            player.Account = new(1, "1", "1", "테스트", "1");
            player.Characters_Status = new();
            List<long> inven = new();
            List<long> party = new();

            API.ApiGateway api = new(player.Account.Hash);
            Ui.Player = player;
            Ui.Inven = inven;
            Ui.Party = party;

            api.Player_Init(player);
            Dm.Re_Status(player, 1);
            Dm.Re_Party(player, party);


            // 최초 실행시 동료 추가 여부 (주석으로 알아서 조절) (추후 매뉴화)
            if (player.Characters.Count < 2)
            {
                api.PartyJoin(1002001);
                api.PartyJoin(1003001);
                //api.PartyJoin(1004001);
                api.Player_Init(player);
                Dm.Re_Status(player, 1);
                Dm.Re_Party(player, party);
            }

            Random rand = new Random();
            bool exit = false;
            int chance_battle = 0;

            int x = 10, y = 10;
            ConsoleKeyInfo keyInfo;

            while (!exit)
            {
                Console.SetCursorPosition(0, 0);
                Ui.Info_Player();
                UI.Layout.Field();

                Console.SetCursorPosition(0, 25);
                Console.WriteLine("(방향키: 움직임 / 메뉴 : esc)");

                Console.SetCursorPosition(x, y);
                Console.Write("★");

                keyInfo=Console.ReadKey(true); // 입력받기
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    exit = Scene_Menu.Menu_Open(player, api, party, inven);
                    Console.Clear();
                }

                // 이전 위치 지우기
                Console.SetCursorPosition(x, y);
                Console.Write("  ");

                // 키에 따른 좌표 이동
                if (keyInfo.Key == ConsoleKey.UpArrow) if(y>5) y--;
                if (keyInfo.Key == ConsoleKey.DownArrow) if(y<23) y++;
                if (keyInfo.Key == ConsoleKey.LeftArrow) if(x>1) x--;
                if (keyInfo.Key == ConsoleKey.RightArrow) if(x<38) x++;                    

                // 새로운 위치에 그리기
                Console.SetCursorPosition(x, y);
                Console.Write("★");

                chance_battle = rand.Next(100);
                if (chance_battle > 90 && keyInfo.Key != ConsoleKey.Escape)
                {
                    Console.SetCursorPosition(4, y-1);
                    Console.WriteLine($"몬스터가 나타났다!!");
                    Thread.Sleep(1000);

                    Scene_Battle.Battle_Start(player, api, 3010101);
                    Console.Clear();
                }
            }
        }

    }
}
