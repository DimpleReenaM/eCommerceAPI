
using server.Entities;

namespace server.Interface.Repository
{
    public interface IPaymentDetailRepository:IGenericRepository<PaymentDetails>
    {
        Task<PaymentDetails?> GetPaymentDetailsByRPId(string razorpayOrderId);
        Task<PaymentDetails?> GetPaymentDetailsByOrderId(int orderId);
    }
}