using GeekShopping.CartAPI.Interfaces.Repositories;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.MessageSender;
using GeekShopping.CartAPI.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IRabbitMQMessageSender _rabbitMQSender;

        public CartController(ICartRepository cartRepository, ICouponRepository couponRepository, IRabbitMQMessageSender rabbitMQSender)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
            _rabbitMQSender = rabbitMQSender ?? throw new ArgumentNullException(nameof(rabbitMQSender));
        }

        [HttpGet("find-cart/{userId}")]
        public async Task<ActionResult<IEnumerable<CartVO>>> FindAll(string userId)
        {
            var cart = await _cartRepository.FindCartByUserId(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<IEnumerable<CartVO>>> Update(CartVO vo)
        {
            var cart = await _cartRepository.InsertOrUpdate(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<IEnumerable<CartVO>>> Insert(CartVO vo)
        {
            var cart = await _cartRepository.InsertOrUpdate(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }
        
        [HttpDelete("remove-cart")]
        public async Task<ActionResult<IEnumerable<CartVO>>> Remove(int id)
        {
            var status = await _cartRepository.Remove(id);
            if (!status) return BadRequest();
            return Ok(status);
        }


        [HttpPost("apply-coupon")]
        public async Task<ActionResult<IEnumerable<CartVO>>> ApplyCoupon(CartVO vo)
        {
            var status = await _cartRepository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CouponCode);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartVO>> ApplyCoupon(string userId)
        {
            var status = await _cartRepository.RemoveCoupon(userId);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderMessage>> Checkout(CheckoutHeaderMessage message)
        {
            string token = Request.Headers["Authorization"];
            if (message?.UserId == null) return BadRequest();
            var cart = await _cartRepository.FindCartByUserId(message.UserId);
            if (cart == null) return NotFound();
            if (!string.IsNullOrEmpty(message.CouponCode))
            {
                CouponVO coupon = await _couponRepository.GetCoupon(message.CouponCode, token);
                if (message.DiscountAmount != coupon.DiscountAmount)
                {
                    return StatusCode(412);
                }
            }

            message.CartDetails = cart.CartDetails;
            message.DateTime = DateTime.Now;

            _rabbitMQSender.SendMessage(message, "checkoutqueue");

            await _cartRepository.Clear(message.UserId);

            return Ok(message);
        }
    }
}
