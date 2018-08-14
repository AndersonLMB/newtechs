using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS;
using System.IO;

namespace AE
{
    class Program
    {
        static void Main(string[] args)
        {
            RuntimeManager.Bind(ProductCode.EngineOrDesktop);
            IMapDocument imd = new MapDocument();
            imd.Open(@"C:\test\pzhmxd\phq.mxd");
            var m0 = imd.Map[0];
            var emLayers = m0.Layers;

            ILayer layer;
            while ((layer = emLayers.Next()) != null)
            {
                Console.WriteLine(String.Format("{0} {1}", layer.Name, layer.Valid));
                //if(   (layer as IFeatureLayer2)  )
                //var ifl = ((IFeatureLayer)layer);
                //var fc = ifl.FeatureClass;
                //var igfl = layer as IGeoFeatureLayer;
                //var igflfc = igfl.FeatureClass;
                var dsn = ((layer as IDataLayer2).DataSourceName);
                //var ifc = (dsn.Open() as IFeatureClass);
                //var tryobj = dsn.Open().GetType();
                //Console.WriteLine(ifl.FeatureClass);
                //Console.WriteLine(fc.FeatureCount(null));
                //var ws = (layer as IGeoFeatureLayer).FeatureClass.FeatureDataset.Workspace;
                //Console.WriteLine(ws.ConnectionProperties.GetProperty("INSTANCE"));

            }



        }
    }
}
