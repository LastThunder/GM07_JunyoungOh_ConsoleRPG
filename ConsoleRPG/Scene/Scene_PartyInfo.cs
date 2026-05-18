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
    static class Scene_PartyInfo
    {
        public static void Equip_Start(Player player, API.ApiGateway api, List<long> party, List<long> inven)
        {

            int select = -1;
            int select_item = -1;

            while (true)
            {
                Console.Clear();
                Ui.Info_Player();
                Console.WriteLine();

                Console.SetCursorPosition(0, 6);
                Ui.Info_Character();

                Console.SetCursorPosition(0, 0);
                Console.SetCursorPosition(0, 3);
                Console.Write("장착을 원하는 캐릭터 숫자 입력. (그외는 나가기)  ");
                int.TryParse(Console.ReadLine(), out select);

                if (0 < select && select <= party.Count)
                {
                    Console.Clear();
                    while (true)
                    {
                        Ui.Info_Player();
                        Console.WriteLine();

                        Ui.Info_Character(party[select-1]);
                        Console.WriteLine();

                        Console.SetCursorPosition(0, 11);
                        Ui.Info_Inventory();

                        Console.SetCursorPosition(0, 0);
                        Console.SetCursorPosition(0, 9);
                        Console.Write("장착/해제 을 원하는 아이템 숫자 입력. (그외는 나가기)  ");
                        int.TryParse(Console.ReadLine(), out select_item);

                        if (0 < select_item && select_item <= inven.Count)
                        {
                            if (player.Equipments[inven[select_item-1]].Equip_Character_Uid == party[select-1])
                            {
                                api.Item_Equip(party[select - 1], inven[select_item-1]);                                
                            }
                            else if (player.Equipments[inven[select_item-1]].Equip_Character_Uid == -1 )
                            {
                                if( player.Characters_Equip[party[select-1]].Equip_Equipment_Uid[ Dm.Item[ player.Equipments[inven[select_item-1]].ItemNo ].Eqiup_Slot-1 ] > 0)
                                {
                                    api.Item_Equip(party[select - 1], player.Characters_Equip[party[select - 1]].Equip_Equipment_Uid[Dm.Item[player.Equipments[inven[select_item-1]].ItemNo].Eqiup_Slot - 1]);
                                } // 기존 슬롯에 다른 장비가 이미 있을 경우, 그 장비 해제

                                api.Item_Equip(party[select - 1], inven[select_item-1]);
                            }
                            else
                            {
                                Console.WriteLine("다른 캐릭터가 장비 중인 아이템 입니다.");
                                Thread.Sleep(750);
                            }
                            api.Player_Init(player);
                            Dm.Re_Status(player,1);
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
