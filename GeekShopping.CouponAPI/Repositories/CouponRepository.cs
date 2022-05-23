﻿using AutoMapper;
using GeekShopping.CouponAPI.Interfaces.Repositories;
using GeekShopping.CouponAPI.Models.Context;
using GeekShopping.CouponAPI.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly MySQLContext _context;
        private IMapper _mapper;

        public CouponRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CouponVO> GetCouponByCouponCode(string couponCode)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
            return _mapper.Map<CouponVO>(coupon);
        }
    }
}
