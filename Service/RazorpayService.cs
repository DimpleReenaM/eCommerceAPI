using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System.Collections.Generic;

namespace server.Service;
public class RazorpayService
{
    private readonly string _key;
    private readonly string _secret;
    private readonly RazorpayClient _client;

    public RazorpayService(IConfiguration configuration)
    {
        _key = configuration["Razorpay:KeyId"];
        _secret = configuration["Razorpay:KeySecret"];

        _client = new RazorpayClient(_key, _secret);
    }

    // Verify payment signature from Razorpay
    public bool VerifyPaymentSignature(string orderId, string paymentId, string signature)
    {
        var client = new RazorpayClient(_key, _secret);
        var attributes = new Dictionary<string, string>
        {
            { "razorpay_order_id", orderId },
            { "razorpay_payment_id", paymentId },
            { "razorpay_signature", signature }
        };

        try
        {
            Razorpay.Api.Utils.verifyPaymentSignature(attributes);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class PaymentOrder
{
    public string OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}
