using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;

namespace SFSPostDeleteTD
{
    class Program
    {
        static void Main(string[] args)
        {
            string method, url, tds;
            try
            {
                method = args[0].ToString().ToUpper();
            }
            catch (Exception e)
            {
                e = new ArgumentException("未输入方法参数或方法不对");
                throw e;
            }
            try
            {
                url = args[1];
            }
            catch (Exception e)
            {
                e = new ArgumentException("未输入url参数或地址不对");
                throw e;
            }
            try
            {
                tds = args[2];
            }
            catch (Exception e)
            {
                e = new ArgumentException("未输入td参数或td不对");
                throw e;
            }

            switch (method)
            {
                case "RMTD":
                    RemoveTds(url, tds);
                    break;
                default:
                    break;
            }
            Console.ReadLine();

        }
        public static void RemoveTds(string url, string tdsstring)
        {
            var tds = tdsstring.Split(',');
            foreach (var td in tds)
            {
                WebClient webClient = new WebClient();
                byte[] bytearr = Encoding.UTF8.GetBytes(td);
                webClient.UploadDataAsync(new Uri(url), "POST", bytearr);
                webClient.UploadDataCompleted += WebClient_UploadDataCompleted;
            }



            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            //httpWebRequest.Method = "POST";
            //httpWebRequest.GetRequestStream();

        }

        private static void WebClient_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine(e.Error.Message);
            }
            else
            {
                if (e.Result != null)
                {
                    Console.WriteLine(String.Format("{0} 结果", Encoding.Default.GetString(e.Result)));
                }
            }
            //throw new NotImplementedException();
        }
    }



    public class Args
    {
        public string Tds { get; set; }
    }


}
