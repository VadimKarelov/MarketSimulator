using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketSimulator
{
    class Computing
    {
        // agents
        private List<Client> clients = new List<Client>();
        private List<Seller> sellers = new List<Seller>();

        // list contain points for drawing graphics
        private List<int> graphicAverage = new List<int>();
        private List<int> graphicMax = new List<int>();

        // ui parameters
        private int fieldHeight;
        private int fieldWidth;

        // simulation parameters
        private int numberOfSellers;
        private int numberOfClients;

        // extra
        Random rn = new Random();

        // ==== methods ====
        public Computing(int w, int h, int nSellers, int nClients)
        {
            // set data
            fieldWidth = w;
            fieldHeight = h;

            numberOfClients = nClients;
            numberOfSellers = nSellers;
        }

        public void ChangeNumberOfAgents(int nSellers, int nClients)
        {
            numberOfClients = nClients;
            numberOfSellers = nSellers;
        }

        // just start day
        public void StartDay()
        {
            // start simulation
            SimulateDay();
        }

        // method contain actions for one day
        private void SimulateDay()
        {
            // need update, if user change number of agents
            ChangeConditions();

            // after changing number of clients and sellers, coordinates have to be changed
            UpdateAgentsData();

            // update data on graphics
            UpdateGraphics();

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
            for (int i = sellers.Count; i < numberOfSellers; i++)
                sellers.Add(new Seller(rn));
            // remove sellers
            for (int i = sellers.Count; i > numberOfSellers; i--)
                sellers.RemoveAt(sellers.Count - 1);
            // add clients
            for (int i = clients.Count; i < numberOfClients; i++)
                clients.Add(new Client(rn));
            // remove clients
            for (int i = clients.Count; i > numberOfClients; i--)
                clients.RemoveAt(clients.Count - 1);
        }

        private void UpdateAgentsData()
        {
            // sellers
            for (int i = 0; i < sellers.Count; i++)
            {
                sellers[i].X = 0;
                sellers[i].Y = Agent.Size * i * 2;
                sellers[i].HasProduct = true;
            }
            // clients
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].X = fieldWidth - Agent.Size;
                clients[i].Y = Agent.Size * i * 2;
                clients[i].HasProduct = false;
            }
        }

        private void UpdateGraphics()
        {
            if (sellers.Count > 0)
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
        }

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
                    if (sellers[ind].HasProduct && sellers[ind].Price < currentClient.MaxPrice)
                    {
                        currentClient.HasProduct = true;
                        sellers[ind].HasProduct = false;
                    }

                    Animation(ref currentClient, startX, startY);
                }
            }
        }

        // animation
        private void Animation(ref Client agent, int arrX, int arrY)
        {
            while (!(agent.X == arrX && agent.Y == arrY))
            {
                int dx = CountDelta(arrX, agent.X);
                int dy = CountDelta(arrY, agent.Y);
                agent.X += dx;
                agent.Y += dy;
                Thread.Sleep(5);
            }
        }

        // needs to animation
        private int CountDelta(int n1, int n2)
        {
            int m = 5;
            if (n1 > n2)
            {
                return Math.Min(m, n1 - n2);
            }
            else if (n1 < n2)
            {
                if (m < Math.Abs(n1 - n2))
                    return -m;
                else
                    return n1 - n2;
            }
            else return 0;
        }

        // after finish trading sellers have to change prices
        private void UpdateSellersPrices()
        {
            for (int i = 0; i < sellers.Count; i++)
            {
                sellers[i].IsPriceUp(!sellers[i].HasProduct);
            }
        }

        // ==== methods to return actual lists ====
        public List<Seller> GetSellersList()
        {
            return sellers;
        }

        public List<Client> GetClientsList()
        {
            return clients;
        }

        public List<int> GetAverageGraphic()
        {
            return graphicAverage;
        }

        public List<int> GetMaxGraphic()
        {
            return graphicMax;
        }
    }
}
