using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Drawing;
using System.IO;

namespace ImgToConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var cc = ColorToConsoleColor(Color.FromArgb(255, 121, 0, 0));
            //var t = Math.Pow(8, (1.0 / 3));
            //var path = Console.ReadLine();
            //var c = HexStringToColor("#000080");

            var c = ConColorToColor[ConsoleColor.Green];

            ColorToConsoleColor(Color.Pink);

            var path = @"C:\test\nbm.bmp";
            Bitmap bitmap = null;
            try
            {
                bitmap = new Bitmap(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var map = new Dictionary<Color, ConsoleColor>();

            int w = bitmap.Width;
            int h = bitmap.Height;
            var p00 = bitmap.GetPixel(0, 0);

            for (var j = 0; j < h; j++)
            {
                for (var i = 0; i < w; i++)
                {
                    var color = bitmap.GetPixel(i, j);
                    //Console.Write(color);
                    Console.BackgroundColor = ColorToConsoleColor(color);
                    Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    //ConsoleColor.Blue.
                }
                Console.Write("\n");
            }


            //colo
            Console.ReadLine();


            //Bitmap bitmap =new Bitmap()
        }


        static ConsoleColor ColorToConsoleColor(Color color)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;
            double minD = -1;
            ConsoleColor returnConsoleColor = ConsoleColor.Black;

            foreach (var kv in ConColorToColor)
            {
                int dR = r - kv.Value.R;
                int dG = g - kv.Value.G;
                int dB = b - kv.Value.B;
                double distance = Math.Pow((Math.Pow(dR, 2) + Math.Pow(dG, 2) + Math.Pow(dB, 2)), 1.0 / 2);
                if (minD < 0)
                {
                    minD = distance;
                    returnConsoleColor = kv.Key;
                }
                else
                {
                    if (distance < minD)
                    {
                        minD = distance;
                        returnConsoleColor = kv.Key;
                    }
                }


                //Console.WriteLine("k:{0} v:{1}", kv.Key, kv.Value);
            }
            return returnConsoleColor;


            throw new Exception();

            return 0;
        }

        static Dictionary<ConsoleColor, Color> ConColorToColor = new Dictionary<ConsoleColor, Color>()
        {
            {ConsoleColor.Black, Color.Black},
            {ConsoleColor.DarkBlue, Color.DarkBlue},
            {ConsoleColor.DarkGreen, Color.DarkGreen},
            {ConsoleColor.DarkCyan, Color.DarkCyan},
            {ConsoleColor.DarkRed, Color.DarkRed},
            {ConsoleColor.DarkMagenta, Color.DarkMagenta},
            {ConsoleColor.DarkYellow, Color.FromArgb(255,128,128,128)},
            {ConsoleColor.Gray, Color.Gray},
            {ConsoleColor.DarkGray, Color.DarkGray},
            {ConsoleColor.Blue, Color.Blue},
            {ConsoleColor.Green, Color.Green},
            {ConsoleColor.Cyan, Color.Cyan},
            {ConsoleColor.Red, Color.Red},
            {ConsoleColor.Magenta, Color.Magenta},
            {ConsoleColor.Yellow, Color.Yellow},
            {ConsoleColor.White, Color.White}
        };

        static Color HexStringToColor(string hexString)
        {





            Int32 iColorInt = Convert.ToInt32(hexString.Substring(1), 16);
            Color curveColor = Color.FromArgb(iColorInt);
            return curveColor;
        }


        //static Dictionary<Color, ConsoleColor> C2CC = new Dictionary<Color, ConsoleColor>()
        //{
        //};
    }
}
