using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace zjgFolders
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootfolder = String.Empty;
            Console.WriteLine("根目录");

            rootfolder = args[0];

            DirectoryInfo directoryInfo = new DirectoryInfo(rootfolder);
            var dirs = directoryInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                DoWithFolder(dir);
            }

        }

        static void DoWithFolder(DirectoryInfo directoryInfo)
        {
            var folders = new List<string>()
            {
                Path.Combine(directoryInfo.Name, "文件"),
                Path.Combine(directoryInfo.Name, "图片")
            };

            foreach (var folder in folders)
            {
                if (Directory.Exists(folder))
                {

                }
                else
                {
                    Directory.CreateDirectory(folder);
                }



            }




        }
    }
}
