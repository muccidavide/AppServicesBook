using AutoMapper;
using AutoMapper.Internal;
using Northwind.EntityModels;
using Northwind.ViewModels;

namespace MappingObjects.Mappers
{
    public static class CartToSummaryMapper
    {
        public static MapperConfiguration GetMapperConfiguration()
        {
            MapperConfiguration config = new(cfg =>
            {
                cfg.Internal().MethodMappingEnabled = false;
                // To fix an issue with the MaxInteger method:
                // https://github.com/AutoMapper/AutoMapper/issues/3988
                cfg.Internal().MethodMappingEnabled = false;
                // Configure the mapper using projections.
                cfg.CreateMap<Cart, Summary>()
                // Map the first and last names formatted to the full name.
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                string.Format("{0} {1}",
                src.Customer.FirstName,
                src.Customer.LastName)
                ))
                // We have removed a semi-colon from here.
                  // Map the sum of items to the Total member.
                .ForMember(dest => dest.Total, opt => opt.MapFrom(
                src => src.Items.Sum(item => item.UnitPrice * item.Quantity))); ;
            });
            return config;
        }
    }
}
