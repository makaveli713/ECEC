using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Art713.Project713.Cryptography;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace Art713.Project713
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Encryption obj;
        public MainPage()
        {
            this.InitializeComponent();
            //obj = new Encryption();
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
            obj = new Encryption(); //Encryption();
            //Console.WriteLine(obj.Encrypt("Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn"));
            //obj.Encrypt("Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn Artem Trubitsyn");
            //obj.Encrypt("й");
            //obj.Encrypt("Артём Трубицын");
            // абвгдеёжзийклмнопрстуфхцчшщъыьэюя АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ 1234567890 !№;%:?*()_+=-.,\\/
            //var a = obj.Encrypt(TextToEncrypt.Text);
            //var b = a.ToString("x");

            //CryptographicBuffer.EncodeToHexString()
            EncryptedTextBlock.Text = obj.Encrypt(TextToEncrypt.Text);
            //obj = new Encryption();
            //obj.Encrypt("Artem Trubitsyn");
            //var obj = new Encrypt.Encrypt(713030391);
        }

        private void DecryptBtn_OnClick(object sender, RoutedEventArgs e)
        {
            DecryptedTextBlock.Text = obj.Decrypt(EncryptedTextBlock.Text);
        }
    }
}
