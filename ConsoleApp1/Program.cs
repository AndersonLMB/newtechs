using System;
using System.ComponentModel;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS;
using ESRI.ArcGIS.ADF;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ProductCode.EngineOrDesktop);
            Point point = new PointClass();

            point.PutCoords(120.35, 35.12);

            ISpatialReferenceFactory5 isrf = new SpatialReferenceEnvironmentClass();
            //isrf.CreateGeographicCoordinateSystem( (int)   esriSRGeoCSType.chin      )

            var isr = isrf.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984) as ISpatialReference;
            point.SpatialReference = isr;
            //Type t = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
            //System.Object obj = Activator.CreateInstance(t);
            //ISpatialReferenceFactory srFact = obj as ISpatialReferenceFactory;


        }
    }

}
