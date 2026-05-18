using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using ConsoleRPG.DTO;

namespace ConsoleRPG.Utility
{
    interface IGetNoable<T> // 제너릭 메서드에서 클래스 내 uid를 불러오는 용도
    {
        public T GetNo();
    }

    static class Util
    {
        public static int Gacha(List<int> chance)
        {
            Random rand = new();
            int left = 0;
            int right = chance.Count - 1;

            int index = 0;
            int temp = rand.Next(1, chance[chance.Count - 1] +1);

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (mid == 0 || mid == chance.Count - 1)
                {
                    index = mid;
                    break;
                }
                else if (chance[mid-1] < temp && temp <= chance[mid])
                {
                    index = mid;
                    break;
                }
                else if (chance[mid] < temp)    left = mid + 1; // 목표값이 더 크면 오른쪽 영역 탐색                
                else right = mid ; // 목표값이 더 작으면 왼쪽 영역 탐색
            }
            return index;
        }
        
        public static Dictionary<int, T> Json_Data_Load<T>(string filePath) where T : IGetNoable<int>
        {
            List<T> list = new();
            string jsonString = File.ReadAllText(filePath);
            list = JsonSerializer.Deserialize<List<T>>(jsonString);

            Dictionary<int, T> dic = new();
            foreach (T temp in list)    dic[temp.GetNo()] = temp;
            return dic;
        }

        public static T DeepCopy<T>(this T source)
        {
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<T>(json);
        }


        

    }
}
