using AutoImageWaterMarker.Controls;
using AutoImageWaterMarker.Utilities;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoImageWaterMarker
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            ImageWatermarkerViewModel = new ImageWatermarkerViewModel();

            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(Theme.Dark);
            paletteHelper.SetTheme(theme);
        }

        public ImageWatermarkerViewModel ImageWatermarkerViewModel { get; set; }
    }
}
