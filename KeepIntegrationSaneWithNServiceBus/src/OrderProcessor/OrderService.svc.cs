using System;
using System.Threading;

namespace OrderProcessor
{
    public class OrderService : IOrderService
    {
        public int CreateOrder(string name, decimal amount)
        {
            return (int) (DateTime.Now.Ticks % 1000000);
        }
    }
}
