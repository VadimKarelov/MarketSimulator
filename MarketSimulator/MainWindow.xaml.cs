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

namespace MarketSimulator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // agents
        private List<Client> clients = new List<Client>();
        private List<Seller> sellers = new List<Seller>();

        // list contain points for drawing graphics
        private List<int> graphicAverage = new List<int>();
        private List<int> graphicMax = new List<int>();

        private bool isDayRunning = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        // 
        private async Task Simulation()
        {
            while (button_Pause.Content.ToString() == "Пауза" && graphicAverage.Count < 1)
            {
                Day();

                // check for end simulation for avoid multiplying calling this method
                if (button_Pause.Content.ToString() == "Старт")
                {
                    isDayRunning = false;
                    label_Warning.Content = "Симуляция остановлена";
                }
            }    
        }

        // method contain actions for one day
        private void Day()
        {
            // increase day counter
            label_Day.Content = "День: " + (int.Parse(label_Day.Content.ToString().Remove(0, 6)) + 1);

            // need update, if user change number of agents
            ChangeConditions();

            // after changing number of clients and sellers, coordinates have to be changed
            UpdateAgentsData();

            // update data on graphics
            DrawGraphics();
            DrawField();

            // play animation and start simulation
            Trading();

            // finish day with changing prices
            UpdateSellersPrices();
        }

        //check number of sellers and clients
        private void ChangeConditions()
        {
            // update number of elements in lists
            // add sellers
            for (int i = sellers.Count; i < int.Parse(label_SellersCounter.Content.ToString()); i++)
                sellers.Add(new Seller());
            // remove sellers
            for (int i = sellers.Count; i > int.Parse(label_SellersCounter.Content.ToString()); i--)
                sellers.RemoveAt(sellers.Count - 1);
            // add clients
            for (int i = clients.Count; i < int.Parse(label_ClientsCounter.Content.ToString()); i++)
                clients.Add(new Client());
            // remove clients
            for (int i = clients.Count; i > int.Parse(label_ClientsCounter.Content.ToString()); i--)
                clients.RemoveAt(clients.Count - 1);
        }

        private void UpdateAgentsData()
        {
            // sellers
            for (int i = 0; i < sellers.Count; i++)
            {
                sellers[i].X = 0;
                sellers[i].Y = Agent.Size * (i - 1) * 2;
                sellers[i].HasProduct = true;
            }
            // clients
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].X = (int)image_SimulatorArea.Width - Agent.Size;
                clients[i].Y = Agent.Size * (i - 1) * 2;
                clients[i].HasProduct = false;
            }
        }

        // ==== simulation part ====
        private void Trading()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                Client currentClient = clients[i];
                Random rn = new Random();

                // need to remember start position of client
                int startX = currentClient.X;
                int startY = currentClient.Y;

                // remember index of sellers who has been visited
                List<int> visited = new List<int>();

                while (!currentClient.HasProduct && visited.Count < sellers.Count)
                {
                    // decide arrival seller
                    int ind = rn.Next(0, sellers.Count);
                    while (visited.FindIndex(x => x == ind) > -1)
                    {
                        ind = rn.Next(0, sellers.Count);
                    }

                    // remeber seller
                    visited.Add(ind);

                    Animation(ref currentClient, sellers[ind].X, sellers[ind].Y);

                    // deal (trade)
                    if (sellers[ind].Price < currentClient.MaxPrice)
                    {
                        currentClient.HasProduct = true;
                        sellers[ind].HasProduct = false;
                    }

                    Animation(ref currentClient, startX, startY);
                }
            }
        }

        // after finish trading sellers have to change prices
        private void UpdateSellersPrices()
        {
            for (int i = 0; i < sellers.Count; i++)
            {
                sellers[i].IsPriceUp(!sellers[i].HasProduct);
            }
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

        private async void ButtonPauseClicked(object sender, RoutedEventArgs e)
        {
            if (button_Pause.Content.ToString() == "Старт")
            {
                button_Pause.Content = "Пауза";
                isDayRunning = true;
                label_Warning.Content = "Симуляция запущена";
                await Simulation();
            }
            else
            {
                button_Pause.Content = "Старт";
                label_Warning.Content = "Ждите завершения дня";
            }

        }

        // ==== drawing part ====
        // updating ui
        private void DrawField()
        {
            // width and height of sellers and clients on field
            int w = Agent.Size;
            int h = Agent.Size;

            Bitmap btmp = new Bitmap(int.Parse(image_SimulatorArea.Width.ToString()), int.Parse(image_SimulatorArea.Height.ToString()));
            Graphics gr = Graphics.FromImage(btmp);

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

        private void UpdateGraphics()
        {
            int max = 0;
            int average = 0;

            foreach (var s in sellers)
            {
                average += s.Price;
                if (s.Price > max)
                    max = s.Price;
            }
            average /= sellers.Count;

            graphicAverage.Add(average);
            graphicMax.Add(max);

            if (graphicAverage.Count > 40)
                graphicAverage.RemoveAt(0);
            if (graphicMax.Count > 40)
                graphicMax.RemoveAt(0);
        }

        private void DrawGraphics()
        {
            var btmpAverage = new Bitmap(int.Parse(image_GraphicAverage.Width.ToString()), int.Parse(image_GraphicAverage.Height.ToString()));
            var btmpMax = new Bitmap(int.Parse(image_GraphicMaxPrice.Width.ToString()), int.Parse(image_GraphicMaxPrice.Height.ToString()));
            var grA = Graphics.FromImage(btmpAverage);
            var grM = Graphics.FromImage(btmpMax);

            UpdateGraphics();

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

        // animation
        private void Animation(ref Client agent, int arrX, int arrY)
        {
            while (agent.X != arrX && agent.Y != arrY)
            {
                int dx = CountDelta(arrX, agent.X);
                int dy = CountDelta(arrY, agent.Y);
                agent.X += dx;
                agent.Y += dy;
                DrawField();
                Thread.Sleep(10);
            }
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

        // needs to animation
        private int CountDelta(int n1, int n2)
        {
            int m = 100;
            if (n1 > n2)
            {
                return Math.Min(m, n1 - n2);
            }
            else if (n1 < n2)
            {
                return Math.Min(-m, n1 - n2);
            }
            else return 0;
        }
    }
}
