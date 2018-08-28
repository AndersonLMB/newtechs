using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.ServiceModel.Web;
using System.Web.Routing;
using System.ServiceModel.Activation;
using System.ServiceModel.Routing;
using System.Web;
using GetTiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel.Description;

namespace GetTileService
{
    public partial class GetTileService : ServiceBase
    {
        public GetTileService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            //Thread.Sleep(10000);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Start.");
            }

            using (var host = new ServiceHost(typeof(ProxyDownloader)))
            {

                host.AddServiceEndpoint(typeof(ProxyDownloader), new WSHttpBinding(), "http://127.0.0.1:1994/proxydownloader");
                if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = new Uri("http://127.0.0.1:1994/proxydownloader/metadata");
                    host.Description.Behaviors.Add(behavior);
                    host.Opened += delegate
                      {
                          using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\test\serv.txt", true))
                          {
                              sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " .Opened");
                          }
                      };
                    host.Open();
                    //Thread.Sleep(100000000);
                }

                //WebServiceHostFactory webServiceHostFactory = new WebServiceHostFactory();
                //RouteTable.Routes.Add(        )
                //    //host.AddServiceEndpoint( typeof(ProxyDownloader)  )
            }

            //TileDownloaderTasksManager tileDownloaderTasksManager = new TileDownloaderTasksManager();
            //using(  new ServiceHost() )

        }

        protected override void OnStop()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Stop.");
            }
        }
    }
    [ServiceContract(Name = "ProxyDownloader")]
    public class ProxyDownloader
    {
        [OperationContract]
        [WebGet(UriTemplate = "/download")]
        public void Download()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\test\serv.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " download");
            }
        }
    }

    //[ServiceContract(Name = "ProxyDownloader")]
    //public interface IProxyDownloader
    //{
    //    [OperationContract]
    //    [WebGet(UriTemplate = "/download")]
    //    void Download();
    //}
}
