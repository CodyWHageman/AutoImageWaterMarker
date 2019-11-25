using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoImageWaterMarker.Extensions
{
    public static class ThicknessExtensions
    {
        public static bool AllEqual(this Thickness thickness, int number)
        {
            return thickness.Left == number && thickness.Top == number && thickness.Right == number && thickness.Bottom == number;
        }
    }
}
