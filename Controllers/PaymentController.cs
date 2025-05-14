using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using server.Dto;
using server.Interface.Service;
using server.Service;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _razorpayService;
    private readonly IEmailService _emailService;

    public PaymentController(IPaymentService razorpayService, IEmailService emailService)
    {
        _razorpayService = razorpayService;
        _emailService = emailService;
    }

    // // Endpoint to create Razorpay order
    // [HttpPost("create-order")]
    // public IActionResult CreateOrder([FromBody] decimal amount)
    // {
    //     var order = _razorpayService.CreateOrder(amount);
    //     return Ok(new { orderId = order.OrderId, amount = order.Amount, currency = order.Currency });
    // }

    // Endpoint to verify Razorpay payment
    // [HttpPost("verify-payment")]
    // public IActionResult VerifyPayment([FromBody] PaymentVerificationRequest verificationRequest)
    // {
    //     var isVerified = _razorpayService.VerifyPaymentSignature(
    //         verificationRequest.OrderId,
    //         verificationRequest.PaymentId,
    //         verificationRequest.Signature
    //     );

    //     return isVerified ? Ok("Payment verified successfully") : BadRequest("Payment verification failed");
    // }

    [HttpPost("update-payment")]
    public async Task<IActionResult> UpdatePayment([FromBody] PaymentVerificationRequest verificationRequest)
    {
        await _razorpayService.VerifyPaymentSignature(
            verificationRequest.OrderId,
            verificationRequest.PaymentId,
            verificationRequest.Signature
        );
        // Send Payment Success Email
        if (!string.IsNullOrEmpty(verificationRequest.Email))
        {
            await _emailService.SendEmailAsync(
                verificationRequest.Email,
                "Payment Successful",
                $"<h2>Thank you for your payment!</h2><p>Your payment ID is <b>{verificationRequest.PaymentId}</b>.</p>"
            );
        }

        ResponseDto res =new ResponseDto();
        return Ok(res.success("Payment Updated"));
    }
}

public class PaymentVerificationRequest
{
    public string OrderId { get; set; }
    public string PaymentId { get; set; }
    public string Signature { get; set; }
    public string Email { get; set; }
}
