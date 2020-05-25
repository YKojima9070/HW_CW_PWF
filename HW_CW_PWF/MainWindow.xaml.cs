
using System.Windows;
using System.Windows.Media;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;


namespace HW_CW_PWF
{ 

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
 

    ///Partialクラス、FormsやPWFは上位Xamlと分割して使用する際に
    ///宣言したクラスの定義を複数のファイルで使用する際のキーワード
    public partial class MainWindow : System.Windows.Window
    {
        /// INT型で画像サイズを定義
 
        int WIDTH = 640;
        int HEIGHT = 480;

        public virtual void Capture(object state)
        //public void Capture()

        {
            var cap = new VideoCapture(0);

            if (!cap.IsOpened())
            {
                MessageBox.Show("camera was not found!!!");
                this.Close();
            }

            cap.FrameWidth = WIDTH;
            cap.FrameHeight = HEIGHT;

            using (var frame = new Mat())

            {
                while (true)
                {
                    cap.Read(frame);

                    if (frame.Empty())
                    {
                        break;
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        this.image.Source = frame.ToWriteableBitmap();

                    });
                }
                 
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();

            Thread cap_thread = new Thread(this.Capture);

            cap_thread.Start();

            Debug.WriteLine("test222");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var bmp_image = (WriteableBitmap)this.image.Source;



            using (var fs = new System.IO.FileStream("hoge.png", System.IO.FileMode.Create))
            {
                var enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bmp_image));

                enc.Save(fs);

                MessageBox.Show("保存しました");


            }


        }



    }
}


