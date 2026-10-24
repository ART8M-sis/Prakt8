using AutoMapper;
using Classwork_17_10_2026.Data.Models;
using Classwork_17_10_2026.DTO.Product;
using Classwork_17_10_2026.DTO.User;

namespace Classwork_17_10_2026.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>();

            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.Name} {src.LastName}"));

            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.AvatarURL, opt => opt.Ignore());
        }
    }
}