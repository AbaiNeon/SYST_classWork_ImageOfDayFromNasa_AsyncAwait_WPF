using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncAwait
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnLoadClick(object sender, RoutedEventArgs e)
        {
            progressBar.IsIndeterminate = true;
            Data jsonObj = await GetList(dataPicker.SelectedDate.Value.ToString("yyyy-MM-dd"));

            string imageUrl = jsonObj.url;

            //--------------------------------------------------------------------------------
            if (jsonObj.media_type == "image")
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imageUrl, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();

                image.Source = bitmap;
            }
            else
            {

            }
            
            //--------------------------------------------------------------------------------

            progressBar.IsIndeterminate = false;

        }

        private Task<Data> GetList(string tmp)
        {
            return Task.Run(() =>
                {
                    string url = @"https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY";
                    string date = "&date=" + tmp;
                    url += date;
                    string json;

                    using (WebClient wc = new WebClient())
                    {
                        wc.Encoding = Encoding.UTF8;
                        json = wc.DownloadString(url);
                    }

                    Data jsonObj = JsonConvert.DeserializeObject<Data>(json);

                    return jsonObj;
                });
        }
    }
}
