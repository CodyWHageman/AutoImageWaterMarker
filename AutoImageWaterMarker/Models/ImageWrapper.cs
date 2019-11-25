using AutoImageWaterMarker.Utilities;
using AutoImageWaterMarker.Watermark;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace AutoImageWaterMarker.Models
{
    public class ImageWrapper : ModelBase
    {
        public event EventHandler Updated;

        private Random _random;
        private Image _markedImage;
        private string _originalPath;

        private string _displayPath;

        public string DisplayPath
        {
            get => _displayPath;
            set => SetProperty(ref _displayPath, value);
        }

        public ImageWrapper(string path)
        {
            Path = path;
            _originalPath = path;
            WatermarkerConfig = new WatermarkerConfig();
            _random = new Random();
            GenerateDisplayFile();
        }


        private string _path;

        public string Path
        {
            get => _path;
            set => SetProperty(ref _path, value);
        }

        private WatermarkerConfig _watermarkerConfig;

        public WatermarkerConfig WatermarkerConfig
        {
            get => _watermarkerConfig;
            set => SetProperty(ref _watermarkerConfig, value);
        }

        private void GenerateDisplayFile()
        {
            DisplayPath = $"{System.IO.Path.GetTempPath()}AutoImageWaterMarker\\{Guid.NewGuid()}.BMP";
            Directory.CreateDirectory(new FileInfo(DisplayPath).Directory.FullName);
            File.Copy(Path,DisplayPath,true);
        }
        public void Mark()
        {
            _markedImage = Watermarker.DrawOn(Path, WatermarkerConfig);
            Path = GenerateNewPath();
            Save();
        }
        
        public void Remark()
        {
            _markedImage.Dispose();
            _markedImage = null;
            DeleteMarkedImage();
            _markedImage = Watermarker.DrawOn(_originalPath, WatermarkerConfig);
            Save();
        }

        private void Save()
        {
            _markedImage.Save(Path);
            GenerateDisplayFile();
        }

        private void DeleteMarkedImage()
        {
            if (Path == _originalPath) return;

            if (File.Exists(Path))
            {
                var markedImagePath = Path;
                Path = null;
                File.Delete(markedImagePath);
                Path = markedImagePath;
            }
        }

        private string GenerateNewPath()
        {
            var newPath = @"C:\WatermarkedImages";
            if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);

            var newFileName = $@"\marked_image_{NextNumber()}.jpg";
            var fullPath = $@"{newPath}\{newFileName}";
            var fileExists = File.Exists(fullPath);

            if (fileExists)
            {
                while(fileExists)
                {
                    newFileName = $@"\marked_image_{NextNumber()}.jpg";
                    fullPath = $@"{newPath}\{newFileName}";

                    fileExists = File.Exists(fullPath);
                }
            }

            return fullPath;
        }

        private int NextNumber() => _random.Next(minValue: 10000, maxValue: 99999);
        
        private void OnWatermarkerConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Remark();
        }

    }
}
