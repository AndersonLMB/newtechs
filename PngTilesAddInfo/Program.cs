using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace PngTilesAddInfo
{
    class Program
    {
        //public Texture2D sourceTex;
        //public Rect sourceRect;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                //C:\test\tdt1
                Console.WriteLine(String.Format("Input Folder ( {0} )", @"C:\test\tdt1"));
                var inputFolderString = Console.ReadLine();
                if (String.IsNullOrEmpty(inputFolderString)) inputFolderString = @"C:\test\tdt1";

                var pngFolderDirectoryInfo = new DirectoryInfo(inputFolderString);
                //var files = pngFolderDirectoryInfo.EnumerateFiles();
                var _files = pngFolderDirectoryInfo.GetFiles("*_.png", SearchOption.AllDirectories);
                foreach (var file in _files)
                {
                    file.Delete();
                }

                var files = pngFolderDirectoryInfo.GetFiles("*.png", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    //var fileData = File.ReadAllBytes(file.FullName);

                    //var tex = new Texture2D(256, 256);
                    //tex.LoadImage(fileData);
                    //Image image = Image.FromFile(file.FullName);
                    //Graphics graphics = Graphics.FromImage(image);
                    //graphics.DrawString(file.FullName, new Font("Consoles", 12), new SolidBrush(Color.Black), 2, 2);
                    //Bitmap bitmap = new Bitmap(file.FullName);
                    //Image image = Image.FromFile(file.FullName);
                    var newFilename = Path.Combine(file.Directory.FullName, file.Name.Replace(".png", "_.png"));
                    //File.Create(Path.Combine(file.Directory.FullName, file.Name.Replace(".png", "_.png")));
                    if (File.Exists(newFilename)) File.Delete(newFilename);
                    File.Create(newFilename).Close();
                    //Image.FromFile(file.FullName);
                    Image img = Image.FromFile(file.FullName);
                    //var p128 = bitmap.GetPixel(128, 128);
                    Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                    using (Graphics graphics = Graphics.FromImage(bmp))
                    {
                        graphics.DrawString(file.FullName, new Font("新宋体", 9), new SolidBrush(Color.Black), 2, 2);

                    }
                    for (var y = 0; y < bmp.Height; y++)
                    {
                        for (var x = 0; x < bmp.Width; x++)
                        {
                            if (x == 0 || x == bmp.Width - 1 || y == 0 || y == bmp.Height - 1)
                            {
                                bmp.SetPixel(x, y, Color.FromArgb(127, 0, 0, 0));
                            }
                        }
                    }

                    bmp.Save(newFilename, ImageFormat.Png);
                    //Graphics graphics = Graphics.FromImage(image);


                }
                ;
                Console.ReadLine();




            }
            else
            {

            }
        }
    }
}
