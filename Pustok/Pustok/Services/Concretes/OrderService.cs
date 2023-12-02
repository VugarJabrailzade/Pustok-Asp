using Pustok.Database;
using Pustok.Services.Abstract;
using System;

namespace Pustok.Services.Concretes
{
    public class OrderService : IOrderService
    {
        private readonly PustokDbContext pustokDbContext;

        public OrderService(PustokDbContext pustokDbContext)
        {
            this.pustokDbContext = pustokDbContext;
        }

        public string GenerateTrackingCode()
        {
            var random = new Random();
            string code;
            string numberPart;


            do
            {
                numberPart = random.Next(1, 100000).ToString();
                code = $"OR{numberPart.PadLeft(5, '0')}";
            } while (DoesCodeExist(code));

            return code;
        }

        public bool DoesCodeExist(string code)
        {
            return false;
        }


    }
}
