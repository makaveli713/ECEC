using Art713.Project713.Cryptography;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

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
        }
    }
}
