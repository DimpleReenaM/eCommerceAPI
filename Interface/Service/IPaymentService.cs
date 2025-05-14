using server.Entities;

namespace server.Interface.Service
{
    public interface IPaymentService
    {
        Task<PaymentDetails> InitializePayment(int userId,int orderId,decimal amount,string currency="INR");
        Task VerifyPaymentSignature(string orderId, string paymentId, string signature);

    }
}