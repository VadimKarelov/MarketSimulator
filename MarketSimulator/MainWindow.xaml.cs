using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarketSimulator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDayRunning = false;

        // timers for drawing
        private DispatcherTimer timerDraw;
        private DispatcherTimer timerGraph;

        // simulation class
        Computing sim;

        public MainWindow()
        {
            InitializeComponent();

            sim = new Computing(int.Parse(image_SimulatorArea.Width.ToString()),
                int.Parse(image_SimulatorArea.Height.ToString()),
                int.Parse(label_ClientsCounter.Content.ToString()),
                int.Parse(label_ClientsCounter.Content.ToString()));
            
            // timer for drawing
            timerDraw = new DispatcherTimer();
            timerDraw.Interval = System.TimeSpan.FromMilliseconds(10);
            timerDraw.Tick += DrawField;
            timerDraw.Start();

            // timer for graphics
            timerGraph = new DispatcherTimer();
            timerGraph.Interval = System.TimeSpan.FromMilliseconds(10);
            timerGraph.Tick += DrawGraphics;
            timerGraph.Start();
        }

        // ==== event part ====
        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // update info on labels
            if (slider_Clients != null)
            {
                slider_Sellers.Value = Math.Round(slider_Sellers.Value);
                slider_Clients.Value = Math.Round(slider_Clients.Value);
                label_SellersCounter.Content = slider_Sellers.Value.ToString();
                label_ClientsCounter.Content = slider_Clients.Value.ToString();
            }
        }

        private void ButtonPauseClicked(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(sim.StartDay));
            thread.Start();
        }

        // ==== drawing part ====
        // updating ui
        private void DrawField(object sender, EventArgs e)
        {
            // lists of agents
            var sellers = sim.GetSellersList();
            var clients = sim.GetClientsList();

            // width and height of sellers and clients on field
            int w = Agent.Size;
            int h = Agent.Size;

            Bitmap btmp = new Bitmap(int.Parse(image_SimulatorArea.Width.ToString()), int.Parse(image_SimulatorArea.Height.ToString()));
            Graphics gr = Graphics.FromImage(btmp);
            gr.Clear(System.Drawing.Color.White);

            // draw sellers
            foreach (var s in sellers)
            {
                gr.FillRectangle(new SolidBrush(System.Drawing.Color.Green), s.X, s.Y, w, h);
            }

            // draw clients
            foreach (var c in clients)
            {
                gr.FillRectangle(new SolidBrush(System.Drawing.Color.LightBlue), c.X, c.Y, w, h);
            }

            // draw products
            foreach (var s in sellers)
            {
                if (s.HasProduct)
                {
                    gr.FillEllipse(new SolidBrush(System.Drawing.Color.Red), s.X, s.Y, w / 2, h / 2);
                }
            }
            foreach (var c in clients)
            {
                if (c.HasProduct)
                {
                    gr.FillEllipse(new SolidBrush(System.Drawing.Color.Red), c.X, c.Y, w / 2, h / 2);
                }
            }

            image_SimulatorArea.Source = Bitmap2BitmapImage(btmp);
        }

        private void DrawGraphics(object sender, EventArgs e)
        {
            // lists of graphics
            var graphicAverage = sim.GetAverageGraphic();
            var graphicMax = sim.GetMaxGraphic();

            var btmpAverage = new Bitmap(int.Parse(image_GraphicAverage.Width.ToString()), int.Parse(image_GraphicAverage.Height.ToString()));
            var btmpMax = new Bitmap(int.Parse(image_GraphicMaxPrice.Width.ToString()), int.Parse(image_GraphicMaxPrice.Height.ToString()));
            var grA = Graphics.FromImage(btmpAverage);
            var grM = Graphics.FromImage(btmpMax);

            //UpdateGraphics();

            if (graphicAverage.Count > 1)
            {
                for (int i = 0; i < graphicAverage.Count - 1; i++)
                {
                    grA.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Blue), 10 * i, btmpAverage.Height - graphicAverage[i] * 10,
                        10 * (i + 1), btmpAverage.Height - graphicAverage[i + 1] * 10);
                    grM.DrawLine(new System.Drawing.Pen(System.Drawing.Color.DarkGreen), 10 * i, btmpAverage.Height - graphicMax[i] * 10,
                        10 * (i + 1), btmpAverage.Height - graphicMax[i + 1] * 10);
                }
            }

            image_GraphicAverage.Source = Bitmap2BitmapImage(btmpAverage);
            image_GraphicMaxPrice.Source = Bitmap2BitmapImage(btmpMax);
        }

        // ==== extra methods ====
        // converting bitmap to bitmapImage
        public BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
