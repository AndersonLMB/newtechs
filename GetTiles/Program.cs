using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading;

namespace GetTiles
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TMS切片下载器");

            if (args.Length > 0)
            {




            }
            else
            {
                Console.WriteLine("Input Url String");
                var urlString = Console.ReadLine();
                if (String.IsNullOrEmpty(urlString))
                {
                    urlString = "http://t0.tianditu.com/DataServer?T=vec_w&x={x}&y={y}&l={z}";
                }

                var newUrlString = UrlStringParser.XyzTo012(urlString);

                Console.WriteLine("Input Folder Directory");
                var folderString = Console.ReadLine();
                if (String.IsNullOrEmpty(folderString))
                {
                    folderString = @"C:\test\gt";
                }

                if (!Directory.Exists(folderString)) Directory.CreateDirectory(folderString);
                var folder = new DirectoryInfo(folderString);

                Console.WriteLine("Max Level");
                var levelString = Console.ReadLine();
                int level = 4;
                try
                {
                    level = int.Parse(levelString);

                }
                catch (Exception e)
                {
                    level = 4;
                }

                var a = new TileDownloader().DownloadToFolderAsync(newUrlString, folderString, level);
                //a.Start();




            }

            //var sf = String.Format("{0}{1}", "asd", "asd", "asde");
            //var xstr = String.Format("{x}", new { x = "sd" });
            //String.Format()
            //var str012 = UrlStringParser.XyzTo012("http://t{s}.tianditu.com/DataServer?T=vec_w&x={x}&y={y}&l={z}");






            //IFormatProvider formatProvider =new 
            Console.ReadLine();



            DownloadDataAsync();

            Console.ReadLine();

        }

        public static async Task DownloadDataAsync()
        {


            WebClient client = new WebClient();
            //Console.WriteLine(hcga);
            var ddta = await client.DownloadDataTaskAsync("http://t0.tianditu.com/DataServer?T=vec_w&x=0&y=0&l=1");


            Console.WriteLine(ddta.Length);

        }

    }

    public static class UrlStringParser
    {
        public static string XyzTo012(string xyzString)
        {
            var temp = xyzString;
            temp = temp.Replace(@"{x}", @"{0}");
            temp = temp.Replace(@"{y}", @"{1}");
            temp = temp.Replace(@"{z}", @"{2}");
            temp = temp.Replace(@"{s}", @"{3}");
            return temp;
        }

    }



}

