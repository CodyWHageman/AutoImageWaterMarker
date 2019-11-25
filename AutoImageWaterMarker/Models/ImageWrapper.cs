using AutoImageWaterMarker.Utilities;
using AutoImageWaterMarker.Watermark;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AutoImageWaterMarker.Models
{
    public class ImageWrapper : ModelBase
    {
        public event EventHandler Updated;
        private const string SaveDirectory = @"C:\WatermarkedImages";
        
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
            if (!Directory.Exists(SaveDirectory)) Directory.CreateDirectory(SaveDirectory);

            var newFileName = $@"\marked_image_{NextNumber().ToString().PadLeft(6,'0')}.jpg";
            var fullPath = $@"{SaveDirectory}\{newFileName}";


            return fullPath;
        }

        private int NextNumber()
        {
            var files = Directory.GetFiles(SaveDirectory);
            if (files.Length == 0) return 1;
            return files.Select(file =>
                           file.Substring(file.Length - (6 + (System.IO.Path.GetExtension(file).Length)), 6))
                       .Max(Convert.ToInt32) + 1;
        }

        private void OnWatermarkerConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Remark();
        }

    }
}
