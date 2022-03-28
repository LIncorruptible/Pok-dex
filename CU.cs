using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokédex
{
    class CU
    {
        public static class ConsoleUtility
        {
            public static float compteur = 0;
            const char _block = '■';
            const string _back = "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b";
            const string _twirl = "-\\|/";
            public static void WriteProgressBar(int percent, bool update = false)
            {
                if (update)
                    Console.Write(_back);
                Console.Write("[");
                var p = (int)((percent / 10f) + .5f);
                for (var i = 0; i < 10; ++i)
                {
                    if (i >= p)
                        Console.Write(' ');
                    else
                        Console.Write(_block);
                }
                Console.Write("] {0,3:##0}%", percent);
            }

            public static void WriteProgressBar(bool update = false)
            {
                if (update)
                    Console.Write(_back);
                Console.Write("[");
                var p = (int)((compteur/ 89.8f*2) + .5f);
                for (var i = 0; i < 20; ++i)
                {
                    if (i >= p)
                        Console.Write(' ');
                    else
                        Console.Write(_block);
                }
                Console.Write("] {0,3:##0}%", compteur/890*100);
            }

            public static void WriteProgress(int progress, bool update = false)
            {
                if (update)
                    Console.Write("\b");
                Console.Write(_twirl[progress % _twirl.Length]);
            }
        }
    }
}
