using System;
using System.Text;
using Art713.Project713.Cryptography;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
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
                }
            }        
        }
    }
}
