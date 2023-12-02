namespace Pustok.Services.Abstract
{
    public interface  IOrderService
    {
        string GenerateTrackingCode();
        bool DoesCodeExist(string code);
    }
}
