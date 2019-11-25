using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoImageWaterMarker.Extensions
{
    public static class NumericExtensions
    {
        public static int ToInt(this double num) => Convert.ToInt32(num);
        public static int ToInt(this decimal num) => Convert.ToInt32(num);
        public static int ToInt(this float num) => Convert.ToInt32(num);
    }
}
