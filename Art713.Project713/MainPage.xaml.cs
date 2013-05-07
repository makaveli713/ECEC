using System;
using System.Text;
using Art713.Project713.Cryptography;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Buffer = System.Buffer;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace Art713.Project713
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage
    {
        public Encryption EncryptionObj;
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные о событиях, описывающие, каким образом была достигнута эта страница.  Свойство Parameter
        /// обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void EncryptBtn_OnClick(object sender, RoutedEventArgs e)
        {
            EncryptionObj = new Encryption();
            EncryptedTextBlock.Text = EncryptionObj.Encrypt(TextToEncrypt.Text);
        }

        private void DecryptBtn_OnClick(object sender, RoutedEventArgs e)
        {
            DecryptedTextBlock.Text = EncryptionObj.Decrypt(EncryptedTextBlock.Text);
            DecryptBtn.Foreground = DecryptedTextBlock.Text == TextToEncrypt.Text ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }

        private async void OpenBtn_OnClick(object sender, RoutedEventArgs e)
        {      
            var openPicker = new FileOpenPicker {SuggestedStartLocation = PickerLocationId.Desktop};
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                if (file.FileType == ".jpg")
                {
                    var bitmapImage = new BitmapImage();
                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        bitmapImage.SetSource(fileStream);
                        var image = new Image
                            {
                                Source = bitmapImage,
                                Width = 100,
                                Height = 100,
                                Visibility = Visibility.Visible,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top
                            };
                        var thickness = new Func<Thickness>(()=> ImagesGrid.Children.Count*100 < 400 ? new Thickness(ImagesGrid.Children.Count*100, 0, 0, 0) : new Thickness(0, 100, 0, 0)); // CHANGE!
                        image.Margin = thickness();                    
                        ImagesGrid.Children.Insert(ImagesGrid.Children.Count, image);

                    }
                }
                if (file.FileType == ".txt")
                {
                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        var dataReader = new DataReader(fileStream);
                        await dataReader.LoadAsync((uint)fileStream.Size);
                        var buffer = new byte[(int)fileStream.Size];
                        dataReader.ReadBytes(buffer);
                        var encoding = Encoding.UTF8;
                        TextToEncrypt.Text = encoding.GetString(buffer, 0, buffer.Length);
                    }
                }
            }


            /*
            if (file != null)
            {
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);

                var dataReader = new DataReader(stream);
                    await dataReader.LoadAsync((uint)stream.Size);
                var buffer = new byte[(int)stream.Size];
                dataReader.ReadBytes(buffer);

                if (file.FileType == ".txt")
                {
                    var encoding = Encoding.UTF8;
                    TextToEncrypt.Text = encoding.GetString(buffer, 0, buffer.Length);
                }

                if (file.FileType == ".jpg")
                {                    
                    var bitmap = new BitmapImage();
                    bitmap.SetSource(stream);                  

                    MyImage.Visibility = Visibility.Visible;
                    MyImage.Stretch = Stretch.None;
                    MyImage.Source = bitmap;

                   

                    var image = new Image
                        {
                            Source = bitmap,
                            Width = 200,
                            Height = 200,
                            Visibility = Visibility.Visible,
                            Margin = new Thickness(10, 10, 0, 0)
                        };

                    //image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    //image.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    //Point tappedPoint = e.GetPosition(contentGrid);
                    //contentGrid.Children.Add(image);

                    //MainGrid.Children.Add(image);
                      
                    
                    //MainGrid.Children.Insert(0,image);
                }
             
            }
             */
        }
    }
}
