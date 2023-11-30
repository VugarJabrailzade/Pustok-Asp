using Pustok.Database;
using System;

namespace Pustok.Services.Concretes
{
    public class OrderTrackingCode
    {
        private readonly PustokDbContext pustokDbContext;

        public OrderTrackingCode(PustokDbContext pustokDbContext)
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
