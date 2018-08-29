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
            Console.WriteLine(StringUtils.SizeToReadable(4294967296));
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

                var newUrlString = StringUtils.XyzTo012(urlString);

                Console.WriteLine("Input Folder Directory");
                var folderString = Console.ReadLine();
                if (String.IsNullOrEmpty(folderString))
                {
                    folderString = @"D:\test\tdt";
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
                Console.WriteLine();
                var td = new TileDownloader();
                td.DownloadToFolder(newUrlString, folderString, level ,1);

                //a.Start();




            }
            Console.ReadLine();



            DownloadDataAsync();

            Console.ReadLine();

        }

        public static async Task DownloadDataAsync()
        {
            WebClient client = new WebClient();
            var ddta = await client.DownloadDataTaskAsync("http://t0.tianditu.com/DataServer?T=vec_w&x=0&y=0&l=1");
            Console.WriteLine(ddta.Length);
        }
    }
}

