using System.Collections.Generic;

namespace GetTiles
{
    public static class Scales
    {
        //public static List<double> GoogleScales()
        //{
        //    return new List<double>()
        //    {
        //591657527.591555,
        //295828763.795777,
        //147914381.897889,
        //73957190.948944,
        //36978595.474472,
        //18489297.737236,
        //9244648.868618,
        //4622324.434309,
        //2311162.217155,
        //1155581.108577,
        //577790.554289,
        //288895.277144,
        //144447.638572,
        //72223.819286,
        //36111.909643,
        //18055.954822,
        //9027.977411,
        //4513.988705,
        //2256.994353,
        //1128.497176,
        //    };
        //}

        public static Dictionary<int, double> GoogleScales()
        {
            var returnScales = GoogleScales(591657527.591555, 0, 19);
            return returnScales;
        }

        /// <summary>
        /// 已经知道零级比例尺
        /// </summary>
        /// <param name="zeroLevelResolution"></param>
        /// <param name="minLevel"></param>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public static Dictionary<int, double> GoogleScales(double zeroLevelResolution, int minLevel, int maxLevel)
        {
            var returnScales = new Dictionary<int, double>();
            List<double> scales = new List<double>();
            //scales[0] = zeroLevelResolution;
            scales.Add(zeroLevelResolution);
            for (int i = 1; i <= maxLevel; i++)
            {
                scales.Add(scales[i - 1] / 2.0);
            }
            for (int i = minLevel; i <= maxLevel; i++)
            {
                returnScales.Add(i, scales[i]);
            }
            return returnScales;
        }



    }

    public static class Resolutions
    {
        public static Dictionary<int, double> GetResolutionsByScalesAndDpi(Dictionary<int, double> scales, double dpi)
        {
            var returnResolutions = new Dictionary<int, double>();
            foreach (var scale in scales)
            {
                returnResolutions[scale.Key] = scale.Value / (dpi / 0.0254);
            }
            return returnResolutions;
        }

        public static Dictionary<int, double> GetLonlatScaleResolutions()
        {
            var returnResolutions = new Dictionary<int, double>();

            returnResolutions.Add(1, 0.703125);
            for (var i = 2; i <= 20; i++)
            {
                returnResolutions.Add(i, returnResolutions[i - 1] / 2);
            }
            return returnResolutions;

        }
    }

}

