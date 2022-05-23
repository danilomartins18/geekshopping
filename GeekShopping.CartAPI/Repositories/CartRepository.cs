using AutoMapper;
using GeekShopping.CartAPI.Interfaces.Repositories;
using GeekShopping.CartAPI.Models;
using GeekShopping.CartAPI.Models.Context;
using GeekShopping.CartAPI.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly MySQLContext _context;
        private readonly IMapper _mapper;

        public CartRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            var header = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
            if (header != null)
            {
                header.CouponCode = couponCode;
                _context.CartHeaders.Update(header);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Clear(string userId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
            if (cartHeader != null)
            {
                _context.CartDetails.RemoveRange(_context.CartDetails.Where(x => x.CartHeaderId == cartHeader.Id));
                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CartVO> FindCartByUserId(string userId)
        {
            Cart cart = new Cart
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId) ?? new CartHeader(),
            };
            cart.CartDetails = _context.CartDetails.Include(x => x.Product).Where(x => x.CartHeaderId == cart.CartHeader.Id);
            return _mapper.Map<CartVO>(cart);
        }

        public async Task<CartVO> InsertOrUpdate(CartVO vo)
        {
            var cart = _mapper.Map<Cart>(vo);
            var productId = vo.CartDetails.FirstOrDefault().ProductId;
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }

            var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);
            if (cartHeader == null)
            {
                _context.CartHeaders.Add(cart.CartHeader);
                await _context.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                var cartDetail = await _context.CartDetails.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProductId == cart.CartDetails.FirstOrDefault().ProductId && x.CartHeaderId == cartHeader.Id);

                if (cartDetail == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeader.Id;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
                    cart.CartDetails.FirstOrDefault().Id += cartDetail.Id;
                    cart.CartDetails.FirstOrDefault().CartHeaderId += cartDetail.CartHeaderId;
                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartVO>(cart);
        }

        public async Task<bool> Remove(long cartDetailId)
        {
            try
            {
                var cartDetail = await _context.CartDetails.FirstOrDefaultAsync(x=> x.Id == cartDetailId);
                int total = _context.CartDetails.Where(x => x.CartHeaderId == cartDetail.CartHeaderId).Count();
                _context.CartDetails.Remove(cartDetail);
                if (total == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(x => x.Id == cartDetail.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            var header = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
            if (header != null)
            {
                header.CouponCode = "";
                _context.CartHeaders.Update(header);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
