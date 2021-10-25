using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagment.Businees.Models;
using UsersManagment.Core.Entities;

namespace UsersManagment.Businees.Mappers
{
    public class IEMapper : Profile
    {
        public IEMapper()
        {
            CreateMap<User, UserModel>().ReverseMap();
           
      

            //CreateMap<ProductModel, Product>().ForMember(entity => entity.CategoryId, model => model.MapFrom(soure => soure.Category.Id)).ForMember(entity => entity.DiscountId, model => model.MapFrom(soure => soure.Discount.Id));
            //CreateMap<AppUser, UserModel>().ReverseMap();

            //CreateMap<Application, Project>()
            //    .ForMember(d => d.ApplicationId, s => s.MapFrom(m => m.Id))
            //    .ForMember(d => d.ProjectAttachments, s => s.MapFrom(m => m.ApplicationAttachments));
            //.ForAllMembers(opt=> opt.Condition((src, dest, srcMember, destMember) => destMember != null));
        }
    }

    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<IEMapper>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }
}
