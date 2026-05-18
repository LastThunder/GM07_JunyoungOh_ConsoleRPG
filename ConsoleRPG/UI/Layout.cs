using System;
using System.Collections.Generic;
using System.Text;


//┌ , ─ ,  ┬ ,  ┐ , │ , ├ , ┼ , ┤ , └ ,  ─ ,  ┴ ,  ┘

namespace ConsoleRPG.UI
{
    static class Layout
    {
        public static void Field()
        {
            Draw_Width(3);
            Draw_Width(24,1);

            Draw_Height(0);
            Draw_Height(39,1);
        }

        private static void Draw_Width(int a, int b =0)
        {
            for (int i = 0; i < 40; i++)
            {
                Console.SetCursorPosition(i, a);
                if (0 < i && i < 39) Console.Write("─");
                else if (i == 0)
                {
                    if (b==0) Console.Write("┌");
                    else Console.Write("└");
                }
                else
                {
                    if (b==0) Console.Write("┐");
                    else Console.Write("┘");
                }
            }
        }

        private static void Draw_Height(int a, int b=0)
        {
            for (int i = 3; i < 25; i++)
            {
                Console.SetCursorPosition(a, i);
                if (3 < i && i < 24) Console.Write("│");
                else if (i == 3)
                {
                    if (b == 0) Console.Write("┌");
                    else Console.Write("┐");
                }
                else
                {
                    if (b == 0) Console.Write("└");
                    else Console.Write("┘");
                }
            }
        }

    }
}
