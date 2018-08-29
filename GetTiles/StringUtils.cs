using System;
using System.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Net.Http;

namespace GetTiles
{
    public static class StringUtils
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
        public static string SizeToReadable(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = size;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            return result;
        }

    }

    public static class GeoUtils
    {
        public static string GetLonLatExtentStringFromAdministrativeByTdt(string admName)
        {
            var returnStr = "";
            //var tdtAdmRestApi = ConfigurationManager.AppSettings["tdtAdmRestApi"].ToString();
            var tdtAdmRestApi = "http://api.tianditu.gov.cn/administrative";
            Trace.WriteLine(tdtAdmRestApi);
            var requestUrl = String.Format("{0}?postStr={{\"searchWord\":\"{1}\",\"searchType\":\"1\",\"needSubInfo\":\"false\",\"needAll\":\"false\",\"needPolygon\":\"true\",\"needPre\":\"true\"}}", tdtAdmRestApi, admName);
            //var requestUrl = String.Format("{0}?postStr={%22searchWord%22:%22{1}%22,%22searchType%22:%221%22,%22needSubInfo%22:%22false%22,%22needAll%22:%22false%22,%22needPolygon%22:%22true%22,%22needPre%22:%22true%22}",tdtAdmRestApi);
            //requestUrl = "http://localhost/PZH.GisService/rest/services/data/config/all";
            Trace.WriteLine(requestUrl);
            WebClient client = new WebClient();
            var result = client.DownloadData(new Uri(requestUrl));
            var myString = Encoding.UTF8.GetString(result);
            var obj = JsonConvert.DeserializeObject<AdmResponse>(myString);
            returnStr = obj.data[0].Bound.ToString();
            var split = returnStr.Split(',');
            returnStr = String.Format("{0},{1},{2},{3}", split[0], split[3], split[2], split[1]);
            //task.ContinueWith();
            //task.Wait();
            return returnStr;
        }

        public static string LonLatExtentStringToWebMctExtentString(string lonLatExtentString)
        {
            string returnString;
            var split = lonLatExtentString.Split(',');
            var lb = new CoordinateDouble()
            {
                X = Convert.ToDouble(split[0]),
                Y = Convert.ToDouble(split[1])
            };
            var rt = new CoordinateDouble()
            {
                X = Convert.ToDouble(split[2]),
                Y = Convert.ToDouble(split[3])
            };
            WebClient webClient = new WebClient();

            List<CoordinateDouble> coordinateDoubles = new List<CoordinateDouble>()
            {
                lb,rt
            };

            List<double> vs = new List<double>();

            foreach (var item in coordinateDoubles)
            {
                WebClient wc = new WebClient();
                var downStr = wc.DownloadString(String.Format("http://epsg.io/trans?x={0}&y={1}&z=0&s_srs=4490&t_srs=3857", item.X, item.Y));
                var mct = JsonConvert.DeserializeObject<CoordinateDouble>(downStr);
                vs.Add(mct.X);
                vs.Add(mct.Y);
            }
            returnString = String.Format("{0},{1},{2},{3}", vs[0], vs[1], vs[2], vs[3]);
            return returnString;

            var mctlb = new CoordinateDouble();
            var mctrt = new CoordinateDouble();
            var taskOfLb = new WebClient().
                DownloadStringTaskAsync(new Uri(String.Format("http://epsg.io/trans?x={0}&y={1}&z=0&s_srs=4490&t_srs=3857", lb.X, lb.Y))).
                ContinueWith((res) =>
                {
                    mctlb = JsonConvert.DeserializeObject<CoordinateDouble>(res.Result);

                    ;
                });
            var taskOfRt = new WebClient().
                DownloadStringTaskAsync(new Uri(String.Format("http://epsg.io/trans?x={0}&y={1}&z=0&s_srs=4490&t_srs=3857", rt.X, rt.Y))).
                ContinueWith((res) =>
                {
                    mctrt = JsonConvert.DeserializeObject<CoordinateDouble>(res.Result);
                });
            //var mctlb =
            taskOfLb.Wait();
            taskOfRt.Wait();
            returnString = String.Format("{0},{1},{2},{3}", mctlb.X, mctlb.Y, mctrt.X, mctrt.Y);
            return returnString;

        }

        public static CoordinateDouble EpsgTransform(CoordinateDouble sourceCoordinate, string sourceProjectionCode, string targetProjectionCOde)
        {
            var returnCoordinate = new CoordinateDouble();
            return new CoordinateDouble();

        }
    }

    [DataContract]
    public class AdmResponse
    {
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public List<AdmResponseData> data { get; set; }
        public string returnCode { get; set; }
    }

    [DataContract]
    public class AdmResponseData
    {
        [DataMember]
        public double lnt { get; set; }
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public int level { get; set; }
        [DataMember]
        public string adminType { get; set; }
        [DataMember]
        public string englishabbrevation { get; set; }
        [DataMember]
        public string nameabbrevation { get; set; }
        [DataMember]
        public string cityCode { get; set; }
        [DataMember]
        public string Bound { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string english { get; set; }


    }




}

