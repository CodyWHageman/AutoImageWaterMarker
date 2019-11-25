using AutoImageWaterMarker.Models;
using AutoImageWaterMarker.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace AutoImageWaterMarker.Controls
{
    public class ImageWatermarkerViewModel : ViewModelBase
    {
        public ImageWatermarkerViewModel()
        {
            Images = new ObservableCollection<ImageWrapper>();
            ClearTempFiles();
        }
        
        private ObservableCollection<ImageWrapper> _images;

        public ObservableCollection<ImageWrapper> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        private void ClearTempFiles()
        {
            var tempDirectory = $"{System.IO.Path.GetTempPath()}AutoImageWaterMarker\\";
            if (Directory.Exists(tempDirectory))
                Directory.Delete(tempDirectory,true);
        }
        public RelayCommand LoadImagesCommand => new RelayCommand(LoadImages);

        private void LoadImages(object obj)
        {
            var imgExtensions = new List<string>()
            {
                ".jpg",
                ".png",
                ".jpeg",
                ".gif"
            };

            using (var dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;

                var result = dialog.ShowDialog();

                if (result == DialogResult.OK && dialog.FileNames.Length > 0)
                {
                    foreach (var path in dialog.FileNames)
                    {
                        var ext = Path.GetExtension(path);

                        if (!imgExtensions.Contains(ext))
                        {
                            continue;
                        }

                        Images.Add(new ImageWrapper(path));
                    }
                }
            }

        }

        public RelayCommand MarkImagesCommand => new RelayCommand(MarkImages);

        public void MarkImages(object obj)
        {
            foreach (var image in Images)
            {
                image.Mark();
            }

            Images = new ObservableCollection<ImageWrapper>(Images);
        }

        public RelayCommand UpdateCommand => new RelayCommand(Update);

        private void Update(object obj)
        {
            var image = (ImageWrapper)obj;
            var imageIndex = Images.IndexOf(image);

            Images.Remove(image);
            Images = new ObservableCollection<ImageWrapper>(Images);
            image.Remark();
            Images.Insert(imageIndex, image);
            Images = new ObservableCollection<ImageWrapper>(Images);
        }
    }
}
