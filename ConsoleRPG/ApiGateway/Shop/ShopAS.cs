using ConsoleRPG.DTO;
using ConsoleRPG.Utility;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleRPG.ApiGateway.Shop
{
    internal class ShopAS
    {
        public long Account_Uid { get; set; }
        private DB.ShopDB SDB { get; set; }

        public ShopAS(long account_Uid)
        {
            SDB = new();
            Account_Uid = account_Uid;
        }

        public Account_Jewel GetAccount_Jewel()
        {
            return SDB.GetAccount_Jewel(Account_Uid);
        }

        public bool Jewel_Free_Add(int free)
        {
            return SDB.Account_Jewel_Free_Update(Account_Uid, free);
        }

        private bool Jewel_Money_Add(int money)
        {
            return SDB.Account_Jewel_Money_Update(Account_Uid, money);
        }


        public bool Buy_Jewel(int productNo) // 과금 재화 충전
        {
            if (true) // 결제 인증 확인
            {             
                int money = 1000; // 추후 해당 프로덕트No의 충전량 확인
                Jewel_Money_Add(money);
                return true;
            }
            return false;
        }

        public List<int> Buy_Gacha(int productNo) // 가챠 구매
        {
            List<int> equipments = new();
            Account_Jewel temp = GetAccount_Jewel();

            int price = 500; // 추후 해당 가챠의 가격 확인

            if ( (temp.Jewel_Money + temp.Jewel_Free) >= price && temp.Account_Uid == Account_Uid)
            {
                if (temp.Jewel_Free >= price) Jewel_Free_Add(-price);
                else
                {
                    int remain = price - temp.Jewel_Free;
                    Jewel_Free_Add(-temp.Jewel_Free);
                    Jewel_Money_Add(-remain);
                }
                
                List<int> chance = new List<int> { 12500, 25000, 37500, 50000, 62500, 75000, 87500, 100000 };
                List<int> itemNo = new List<int> { 10401001, 10402001, 10403001, 10404001, 20400001, 30400001, 40400001, 50400001 };
                //추후 해당 가챠 확률 & 구성 불러옴

                for (int i = 0; i < 10; i++) equipments.Add(itemNo[Util.Gacha(chance)]); // 추후 해당 가챠 횟수 확인
            }
            else
            {
                Console.WriteLine("주얼이 부족합니다.");
            }

            return equipments;
        }


    }
}
