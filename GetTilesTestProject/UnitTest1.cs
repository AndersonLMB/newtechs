using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetTiles;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GetTilesTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }

    [TestClass]
    public class ScaleTests
    {
        [TestMethod]
        public void GoogleScalesTest()
        {
            var scale0_4 = GetTiles.Scales.GoogleScales(591657527.591555, 0, 4);
            var diff = scale0_4[4] - 36978595.474472;
            Trace.WriteLine(String.Format("{0:F20}", diff));
        }

        [TestMethod]
        public void DefaultGoogleScalesTest()
        {
            var defaultscale0_19 = GetTiles.Scales.GoogleScales();
            var testDataScales = new List<double>()
                {
                    591657527.591555,
                    295828763.795777,
                    147914381.897889,
                    73957190.948944,
                    36978595.474472,
                    18489297.737236,
                    9244648.868618,
                    4622324.434309,
                    2311162.217155,
                    1155581.108577,
                    577790.554289,
                    288895.277144,
                    144447.638572,
                    72223.819286,
                    36111.909643,
                    18055.954822,
                    9027.977411,
                    4513.988705,
                    2256.994353,
                    1128.497176,
                };
            foreach (var item in defaultscale0_19)
            {
                Trace.WriteLine(String.Format("Test {0:F20} Output {1:F20}", testDataScales[item.Key], item.Value));
            }
        }
    }

    [TestClass]
    public class ResolutionTests
    {
        [TestMethod]
        public void CalculateResolutionTest()
        {
            var scales = Scales.GoogleScales();
            var resolutions = Resolutions.GetResolutionsByScalesAndDpi(scales, 96.0);
            ;
        }
    }

    [TestClass]
    public class Downloader2TestClass
    {
        [TestMethod]
        public void CommonInitTest()
        {
            var td2 = new TileDownloader2();
            td2.CommonInit();
        }

        [TestMethod]
        public void DownloadLeveleTest()
        {
            var td2 = new TileDownloader2();
            td2.CommonInit();
            td2.TilesDownloadExtent = new DoubleExtent()
            {
                xMin = 7739000,
                yMin = 2169000,
                xMax = 14494000,
                yMax = 5711000
            };

            var tileIndexesExtent = td2.CalculateTileIndexesExtent(td2.Resolutions[5]);
            Trace.WriteLine(tileIndexesExtent);
        }
        [TestMethod]
        public void DownloadLevelByResolutionTest()
        {
            var td2 = new TileDownloader2();
            td2.CommonInit();
            //td2.TilesDownloadExtent = new DoubleExtent()
            //{
            //    xMin = 7739000,
            //    yMin = 2169000,
            //    xMax = 14494000,
            //    yMax = 5711000
            //};
            td2.DownloadLevelByResolution(td2.Resolutions[5], 5);

        }

        [TestMethod]
        public void DownloadByResolutionIndexRangeTest()
        {

            //http://t5.tianditu.com/DataServer?T=vec_w&x=0&y=0&l=1
            var td2 = new TileDownloader2();
            td2.CommonInit();
            td2.DownloadFolderPath = @"C:\test\td2";
            td2.UrlTemplate = "http://t{3}.tianditu.com/DataServer?T=vec_w&x={2}&y={1}&l={0}";
            td2.Servers = "0,1,2,3,4";
            var di = new DirectoryInfo(td2.DownloadFolderPath);
            foreach (var file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (var dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }

            //11208658.152227892
            //2912864.3309648014
            //11549272.899636596
            //3212118.7161881635

            //11288373.211211396, 3039098.0261044647, 11374899.927230217, 3104145.9371751496
            //11304730.735264424, 3049264.1508663935, 11362364.254591448, 3093062.5680737994
            //td2.TilesDownloadExtent = new DoubleExtent()
            //{
            //    xMin = 11288373,
            //    yMin = 3039098,
            //    xMax = 11374899,
            //    yMax = 3104145
            //};
            td2.TilesDownloadExtent = new DoubleExtent(11304730.735264424, 3049264.1508663935, 11362364.254591448, 3093062.5680737994);
            td2.DownloadByResolutionIndexRange(11, 17, false);
        }


        [TestMethod]
        public void FileDownTest()
        {



        }
    }

    [TestClass]
    public class DownloadTaskManagerTests
    {
        [TestMethod]
        public void SingleDownloadTaskTest()
        {

            TileDownloaderTask downloadTask = new TileDownloaderTask()
            {
                //C:\test\td2\Z16_Y27740_X51324.png
                //http://t2.tianditu.com/DataServer?T=vec_w&x=51324&y=27740&l=16
                Url = "http://t2.tianditu.com/DataServer?T=vec_w&x=51324&y=27740&l=16",
                Filename = @"C:\test\td2\Z16_Y27740_X51324.png"
            };
            downloadTask.OnDownloadTaskFinallyDownload += (a, b) =>
            {
                Trace.WriteLine(String.Format("{0} => {1}", a.Url, a.Filename));
                Trace.WriteLine(b);
            };
            var task = downloadTask.TryDownload();
            task.Wait();
        }
    }
}
