using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Seller()
        {
            Random rn = new Random();
            HasProduct = true;
            Price = rn.Next(50, 100);
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

        public Client()
        {
            Random rn = new Random();
            HasProduct = false;
            MaxPrice = rn.Next(30, 70);
        }
    }
}
