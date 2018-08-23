using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GetTiles
{
    public class TileDownloaderTasksManager
    {

    }

    public class TileDownloaderTask
    {

    }

    public class TileDownloader2
    {
        #region Fields
        private Dictionary<int, double> resolutions;
        private Dictionary<int, double> scales;
        private Projection projectionType;
        private string urlTemplate;
        private string downloadFolder;
        /// <summary>
        /// 坐标原点
        /// </summary>
        public CoordinateDouble Origin;
        public Size TileSize;
        private double dpi;
        private DoubleExtent tilesExtent;

        #endregion

        #region Properties
        public Dictionary<int, double> Resolutions { get => resolutions; set => resolutions = value; }
        public Projection ProjectionType { get => projectionType; set => projectionType = value; }
        /// <summary>
        /// 要获取的切片的范围,使用相应投影的单位
        /// </summary>
        public DoubleExtent TilesDownloadExtent { get => tilesExtent; set => tilesExtent = value; }
        /// <summary>
        /// 点距
        /// </summary>
        public double Dpi { get => dpi; set => dpi = value; }
        public Dictionary<int, double> Scales { get => scales; set => scales = value; }


        /// <summary>
        /// 
        /// </summary>
        public string UrlTemplate { get => urlTemplate; set => urlTemplate = value; }
        public string DownloadFolderPath { get => downloadFolder; set => downloadFolder = value; }

        #endregion

        #region Constructors

        #endregion

        #region Methods
        public void Start()
        {
            CoordinateDouble coordinateDouble = new CoordinateDouble();
        }

        public void DownloadAllFromService()
        {

        }

        public void CommonInit()
        {
            Origin.X = -20037508.342789244;
            Origin.Y = 20037508.342789244;
            TilesDownloadExtent = new DoubleExtent()
            {
                xMin = -20037508.342789244,
                yMin = -20037508.342789244,
                xMax = 20037508.342789244,
                yMax = 20037508.342789244
            };
            Scales = GetTiles.Scales.GoogleScales();
            Dpi = 96.0;
            Resolutions = GetTiles.Resolutions.GetResolutionsByScalesAndDpi(scales, Dpi);
            TileSize = new Size()
            {
                Width = 256,
                Height = 256
            };
        }

        public void AbsFloorExtent()
        {
            this.TilesDownloadExtent.xMin = AbsFloor(this.TilesDownloadExtent.xMin);
            this.TilesDownloadExtent.yMin = AbsFloor(this.TilesDownloadExtent.yMin);
            this.TilesDownloadExtent.xMax = AbsFloor(this.TilesDownloadExtent.xMax);
            this.TilesDownloadExtent.yMax = AbsFloor(this.TilesDownloadExtent.yMax);

        }

        private double AbsFloor(double input)
        {
            var output = input > 0 ? Math.Floor(input) : Math.Ceiling(input);
            return output;
        }




        /// <summary>
        /// 根据分辨率索引范围下载
        /// </summary>
        /// <param name="minLevel">最小等级</param>
        /// <param name="maxLevel">最大等级</param>
        public void DownloadByResolutionIndexRange(int minLevel, int maxLevel, bool createFolders)
        {
            for (int i = minLevel; i <= maxLevel; i++)
            {
                DownloadByResolutionIndex(i, createFolders);
            }
        }

        /// <summary>
        /// 根据分辨率索引下载
        /// </summary>
        /// <param name="resolutionIndex"></param>
        public void DownloadByResolutionIndex(int resolutionIndex, bool createFolders)
        {
            var resolution = this.Resolutions[resolutionIndex];
            DownloadLevelByResolution(resolution, resolutionIndex, createFolders);
        }

        public void DownloadLevelByResolution(double resolution, int z)
        {


            AbsFloorExtent();
            var indexExtent = CalculateTileIndexesExtent(resolution);



            if (!Directory.Exists(DownloadFolderPath)) Directory.CreateDirectory(DownloadFolderPath);
            var zDirectoryPath = Path.Combine(DownloadFolderPath, z.ToString());
            var zDirectoryInfo = new DirectoryInfo(zDirectoryPath);
            for (int y = indexExtent.yMin; y <= indexExtent.yMax; y++)
            {
                var yDirectoryPath = Path.Combine(zDirectoryPath, y.ToString());
                if (!Directory.Exists(yDirectoryPath)) Directory.CreateDirectory(yDirectoryPath);
                var yDirectoryInfo = new DirectoryInfo(yDirectoryPath);
                for (int x = indexExtent.xMin; x <= indexExtent.xMax; x++)
                {
                    var xFileFullName = Path.Combine(yDirectoryPath, String.Format("{0}.png", x.ToString()));
                    var server = "0";
                    var url = String.Format(this.UrlTemplate, z, y, x, server);
                    Trace.WriteLine(url);
                    //Trace.WriteLine(String.Format("Z:{0} Y:{1} X:{2}", z, y, x));

                }
            }
            //切片的索引范围
        }


        public void DownloadLevelByResolution(double resolution, int z, bool createFolders)
        {
            AbsFloorExtent();
            var indexExtent = CalculateTileIndexesExtent(resolution);
            if (createFolders)
            {
                if (!Directory.Exists(DownloadFolderPath)) Directory.CreateDirectory(DownloadFolderPath);
                var zDirectoryPath = Path.Combine(DownloadFolderPath, z.ToString());
                var zDirectoryInfo = new DirectoryInfo(zDirectoryPath);
                for (int y = indexExtent.yMin; y <= indexExtent.yMax; y++)
                {
                    var yDirectoryPath = Path.Combine(zDirectoryPath, y.ToString());
                    if (!Directory.Exists(yDirectoryPath)) Directory.CreateDirectory(yDirectoryPath);
                    var yDirectoryInfo = new DirectoryInfo(yDirectoryPath);
                    for (int x = indexExtent.xMin; x <= indexExtent.xMax; x++)
                    {
                        var xFileFullName = Path.Combine(yDirectoryPath, String.Format("{0}.png", x.ToString()));
                        var server = "0";
                        var url = String.Format(this.UrlTemplate, z, y, x, server);
                        Trace.WriteLine(url);
                    }
                }
            }
            else
            {

                for (int y = indexExtent.yMin; y <= indexExtent.yMax; y++)
                {

                    for (int x = indexExtent.xMin; x <= indexExtent.xMax; x++)
                    {
                        var server = "1";
                        var xFileFullName = Path.Combine(DownloadFolderPath, String.Format("Z{0}_Y{1}_X{2}.png", z, y, x));
                        Trace.WriteLine(xFileFullName);
                        var url = String.Format(this.UrlTemplate, z, y, x, server);
                        //var xFileFullName = Path.Combine(yDirectoryPath, String.Format("{0}.png", x.ToString()));
                        //var server = "0";
                        //var url = String.Format(this.UrlTemplate, z, y, x, server);
                        Trace.WriteLine(url);
                    }
                }
            }


            //切片的索引范围
        }


        public IntegerExtent CalculateTileIndexesExtent(double resolution)
        {
            IntegerExtent tileIndexesExtent = new IntegerExtent();
            //IntegerExtent tileExtent = new IntegerExtent();
            tileIndexesExtent.xMin = (int)Math.Floor((TilesDownloadExtent.xMin - Origin.X) / (this.TileSize.Width * resolution));
            tileIndexesExtent.yMin = (int)Math.Floor(-(TilesDownloadExtent.yMax - Origin.Y) / (this.TileSize.Height * resolution));
            tileIndexesExtent.xMax = (int)Math.Floor((TilesDownloadExtent.xMax - Origin.X) / (this.TileSize.Width * resolution));
            tileIndexesExtent.yMax = (int)Math.Floor(-(TilesDownloadExtent.yMin - Origin.Y) / (this.TileSize.Height * resolution));




            return tileIndexesExtent;
        }
        #endregion
    }

    public struct CoordinateDouble
    {
        public double X;
        public double Y;
    }

    public struct Size
    {
        public double Width;
        public double Height;

    }

    /// <summary>
    /// 投影类型
    /// </summary>
    public enum Projection
    {
        /// <summary>
        /// 经纬度
        /// </summary>
        LonLat,
        /// <summary>
        /// 球面墨卡托
        /// </summary>
        WebMercator
    }

}

