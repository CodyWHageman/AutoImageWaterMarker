using AutoImageWaterMarker.Models;
using System.Drawing;
using System.Windows;

namespace AutoImageWaterMarker.Watermark
{
    public class WatermarkerConfig : ModelBase
    {
        public WatermarkerConfig()
        {
            SetDefaults();
        }
        private float _opacity;

        public float Opacity
        {
            get => _opacity;
            set => SetProperty(ref _opacity, value);
        }

        private WatermarkPosition _position;

        public WatermarkPosition Position
        {
            get => _position;
            set => SetProperty(ref _position, value, callBack: () => NotifyPropertyChanged(nameof(IsPositionAbsolute)));
        }

        public bool IsPositionAbsolute => Position == WatermarkPosition.Absolute;

        private int _positionX;
        public int PositionX
        {
            get => _positionX;
            set => SetProperty(ref _positionX, value);
        }

        private int _positionY;
        public int PositionY
        {
            get => _positionY;
            set => SetProperty(ref _positionY, value);
        }

        private Color _transparentColor;
        public Color TransparentColor
        {
            get => _transparentColor;
            set => SetProperty(ref _transparentColor, value);
        }

        private RotateFlipType _rotateFlip;
        public RotateFlipType RotateFlip
        {
            get => _rotateFlip;
            set => SetProperty(ref _rotateFlip, value);
        }

        private Thickness _margin;
        public Thickness Margin
        {
            get => _margin;
            set => SetProperty(ref _margin, value);
        }

        private float _scaleRatio;
        public float ScaleRatio
        {
            get => _scaleRatio;
            set => SetProperty(ref _scaleRatio, value);
        }

        private void SetDefaults()
        {
            Opacity = 0.2f;
            Position = WatermarkPosition.Center;
            PositionX = 0;
            PositionY = 0;
            TransparentColor = Color.Empty;
            RotateFlip = RotateFlipType.RotateNoneFlipNone;
            Margin = new Thickness(0);
            ScaleRatio = 1.0f;
        }

    }
}
