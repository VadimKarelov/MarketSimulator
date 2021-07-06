using System;

namespace MarketSimulator
{
    class Agent
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool HasProduct { get; set; }
        public readonly static int Size = 20;
    }

    class Seller : Agent
    {
        public int Price { get; set; }        
        private int dPrice = 1;

        public Seller(Random seed)
        {
            HasProduct = true;
            Price = seed.Next(10, 100);
        }

        // seller need to change price
        public void IsPriceUp(bool f)
        {
            if (f)
                Price += dPrice;
            else
                Price -= dPrice;
        }
    }

    class Client : Agent
    {
        public int MaxPrice {get; set;}

        public Client(Random seed)
        {
            HasProduct = false;
            MaxPrice = seed.Next(50, 100);
        }
    }
}
