using AutoMapper;
using GeekShopping.ProductAPI.Models;
using GeekShopping.ProductAPI.ValueObjects;

namespace GeekShopping.ProductAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductVO, Product>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
