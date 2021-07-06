using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MarketSimulator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // timers for drawing
        private DispatcherTimer timerDraw;

        // simulation class
        Computing sim;

        // thread for computing
        Thread thread;

        public MainWindow()
        {
            InitializeComponent();

            sim = new Computing(int.Parse(image_SimulatorArea.Width.ToString()),
                int.Parse(image_SimulatorArea.Height.ToString()),
                int.Parse(label_ClientsCounter.Content.ToString()),
                int.Parse(label_ClientsCounter.Content.ToString()));

            thread = new Thread(new ThreadStart(sim.StartDay));

            // timer for drawing
            timerDraw = new DispatcherTimer();
            timerDraw.Interval = System.TimeSpan.FromMilliseconds(10);
            timerDraw.Tick += UpdateUi;
            timerDraw.Start();
        }

        private void CheckThreadState()
        {
            if (thread != null && button_Pause != null)
            {
                // day is running and user want to stop it
                if (thread.IsAlive && button_Pause.Content.ToString() == "Старт")
                {
                    label_Warning.Content = "Ждите окончания дня";
                }
                // day is running and user dont want to stop simulation
                else if (thread.IsAlive && button_Pause.Content.ToString() == "Пауза")
                {
                    label_Warning.Content = "Симуляция запущена";
                }
                // day isnt running but user want to continue simulation
                else if (!thread.IsAlive && button_Pause.Content.ToString() == "Пауза")
                {
                    // start new day
                    thread = new Thread(new ThreadStart(sim.StartDay));
                    thread.Start();

                    // update day counter
                    label_Day.Content = "День: " + sim.GetNumberOfDays();
                }
                else
                {
                    label_Warning.Content = "Симуляция остановлена";
                }
            }
        }

        // ==== event part ====
        private void SliderNumber_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // update info on labels
            if (slider_Clients != null)
            {
                slider_Sellers.Value = Math.Round(slider_Sellers.Value);
                slider_Clients.Value = Math.Round(slider_Clients.Value);
                label_SellersCounter.Content = slider_Sellers.Value.ToString();
                label_ClientsCounter.Content = slider_Clients.Value.ToString();

                if (sim != null)
                {
                    sim.ChangeNumberOfAgents((int)slider_Sellers.Value, (int)slider_Clients.Value);
                }
            }
        }

        private void SliderSpeed_ValueCahnges(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sim != null)
            {
                slider_Speed.Value = (int)Math.Round(slider_Speed.Value);
                sim.SetSpeed((int)slider_Speed.Value);
            }
        }

        private void ButtonPauseClicked(object sender, RoutedEventArgs e)
        {
            // change button text
            if (button_Pause.Content.ToString() == "Старт")
            {
                button_Pause.Content = "Пауза";
            }
            else
            {
                button_Pause.Content = "Старт";
            }

            // start day if previous day end
            if (thread != null && !thread.IsAlive)
            {
                // start new day
                thread = new Thread(new ThreadStart(sim.StartDay));
                thread.Start();

                // update day counter
                label_Day.Content = "День: " + sim.GetNumberOfDays();
            }
        }

        // ==== drawing part ====
        // updating ui
        private void UpdateUi(object sender, EventArgs e)
        {
            DrawField();
            DrawGraphics();
            CheckThreadState();
            UpdateLists();
        }

        private void DrawField()
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

        private void DrawGraphics()
        {
            // lists of graphics
            var graphicAverage = sim.GetAverageGraphic();
            var graphicMax = sim.GetMaxGraphic();

            int step = 10;
            int scale = 2;

            if (graphicAverage.Count > 1)
            {
                // prepare bitmaps
                var btmpAverage = new Bitmap(int.Parse(image_GraphicAverage.Width.ToString()), int.Parse(image_GraphicAverage.Height.ToString()));
                var btmpMax = new Bitmap(int.Parse(image_GraphicMaxPrice.Width.ToString()), int.Parse(image_GraphicMaxPrice.Height.ToString()));
                var grA = Graphics.FromImage(btmpAverage);
                var grM = Graphics.FromImage(btmpMax);

                // white background
                grA.Clear(System.Drawing.Color.White);
                grM.Clear(System.Drawing.Color.White);

                for (int i = 0; i < graphicAverage.Count - 1; i++)
                {
                    grA.DrawLine(new System.Drawing.Pen(System.Drawing.Color.Blue), step * i, btmpAverage.Height - graphicAverage[i] * scale,
                        step * (i + 1), btmpAverage.Height - graphicAverage[i + 1] * scale);
                    grM.DrawLine(new System.Drawing.Pen(System.Drawing.Color.DarkGreen), step * i, btmpAverage.Height - graphicMax[i] * scale,
                        step * (i + 1), btmpAverage.Height - graphicMax[i + 1] * scale);
                }

                // set result to images
                image_GraphicAverage.Source = Bitmap2BitmapImage(btmpAverage);
                image_GraphicMaxPrice.Source = Bitmap2BitmapImage(btmpMax);

                // set result to labels
                label_Average.Content = "Средняя цена товара: " + graphicAverage[graphicAverage.Count - 1];
                label_MaxPrice.Content = "Максимальная цена товара: " + graphicMax[graphicMax.Count - 1];
            }
        }

        private void UpdateLists()
        {
            var sellers = sim.GetSellersList();
            var clients = sim.GetClientsList();

            string listOfSellers = "Список продавцов:";
            string listOfClients = "Список покупателей:";

            for (int i = 0; i < sellers.Count; i++)
            {
                listOfSellers += "\nПродавец " + (i + 1).ToString() + ", цена: " + sellers[i].Price;
            }

            for (int i = 0; i < clients.Count; i++)
            {
                listOfClients += "\nПокупатель " + (i + 1).ToString() + ", max цена: " + clients[i].MaxPrice;
            }

            label_ListOfSellers.Content = listOfSellers;
            label_ListOfClients.Content = listOfClients;
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
