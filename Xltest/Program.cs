using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;


namespace Xltest
{
    class Program
    {
        static void Main(string[] args)
        {
            string bmppath = "", xlspath = "";
            if (args.Length < 2)
            {
                Console.WriteLine("Path of BMP file");
                bmppath = Console.ReadLine();
                Console.WriteLine("Path of Output Excel file");
                xlspath = Console.ReadLine();
            }
            else
            {
                bmppath = args[0];
                xlspath = args[1];
            }


            //List<Person> students = new List<Person>()
            //{
            //   new Person (){Name="Anderson" ,Age=30},
            //   new Person (){Name="Tantan", Age =20}
            //};
            //SaveListAsExcel(students, String.Format(@"C:\test\xl{0}.xls", DateTime.Now.ToBinary()));
            //Console.ReadLine();


            SaveBmpAsExcel(bmppath, xlspath);
            //SaveBmpAsExcel(@"C:\test\nbm.bmp", String.Format(@"C:\test\xl{0}.xls", DateTime.Now.ToBinary()));
            Console.ReadLine();

        }

        static void SaveListAsExcel(List<Person> people, string path)
        {
            var app = new Excel.Application();
            var workbook = app.Workbooks.Add();
            FileInfo fileInfo = null;
            var sheet = workbook.Worksheets[1];
            Console.WriteLine((sheet as Worksheet).Name);
            var worksheet = (sheet as Worksheet);
            var cells = worksheet.Cells;
            var rows = worksheet.Rows;
            worksheet.Cells[1, 1] = "kaxvckal";
            var rowIndex = 1;
            foreach (var person in people)
            {
                worksheet.Cells[rowIndex, 1] = person.Name.ToString();
                worksheet.Cells[rowIndex, 2] = person.Age.ToString();
                rowIndex++;
            }
            Console.WriteLine(workbook.Worksheets.Count);
            workbook.SaveAs(path, Type.Missing);
            workbook.Close();
            app.Workbooks.Close();
            app.Quit();


        }
        static void SaveBmpAsExcel(string bmpPath, string xlsPath)
        {
            var app = new Excel.Application();
            var workbook = app.Workbooks.Add();
            var sheet = workbook.Worksheets[1];
            var worksheet = (sheet as Worksheet);
            Bitmap bitmap = new Bitmap(bmpPath);
            int total = bitmap.Width * bitmap.Height;
            int progress = 0;
            Console.WriteLine();
            for (var j = 0; j < bitmap.Height; j++)
            {
                for (var i = 0; i < bitmap.Width; i++)
                {
                    var color = bitmap.GetPixel(i, j);
                    var cell = worksheet.Cells[j + 1, i + 1] as Range;
                    cell.Interior.Color = color;
                    progress++;
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine("                                   ");
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine(String.Format("{0} / {1}", progress, total));

                }
            }
            workbook.SaveAs(xlsPath);
            workbook.Close();
            app.Workbooks.Close();
            app.Quit();
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }





}
