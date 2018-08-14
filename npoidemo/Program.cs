using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI;


using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace npoidemo
{
    class Program
    {
        static void Main(string[] args)
        {

            HSSFWorkbook workbook2003 = new HSSFWorkbook(); //新建xls工作簿
            workbook2003.CreateSheet("Sheet1");  //新建3个Sheet工作表
            HSSFSheet SheetOne = (HSSFSheet)workbook2003.GetSheet("Sheet1");

            SheetOne.CreateRow(7);
            HSSFRow SheetRow = (HSSFRow)SheetOne.GetRow(7);
            SheetRow.CreateCell(0);
            var cell0 = SheetRow.GetCell(0);
            cell0.SetCellValue("HelloWorld");
            //HSSFCell[] SheetCell = new HSSFCell[10];
            //SheetRow.CreateCell(0);


            FileStream file2003 = new FileStream(@"C:\test\Excel2003.xls", FileMode.Create);

            workbook2003.Write(file2003);
            file2003.Close();  //关闭文件流
            workbook2003.Close();




        }
    }
}
