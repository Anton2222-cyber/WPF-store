using AutoMapper;
using Data.Entities;
using Data.Entities.Identity;
using WebApi.Models.Account;
using WebApi.Models.Areas;
using WebApi.Models.Basket;
using WebApi.Models.Category;
using WebApi.Models.Order;
using WebApi.Models.PostOffice;
using WebApi.Models.Product;
using WebApi.Models.Settlement;
using WebApi.Seeders.PostOfficeHelpers.Response;
using WebApi.Seeders.PostOfficeHelpers.ViewModels;

namespace WebApi.Mapper;

public class AppMapProfile : Profile {
	public AppMapProfile() {
		CreateMap<AreaData, AreaVm>();
		CreateMap<SettlementData, SettlementVm>();
		CreateMap<PostOfficeData, PostOfficeVm>();



		CreateMap<Category, CategoryItemViewModel>();
		CreateMap<CategoryCreateViewModel, Category>();

		CreateMap<RegisterViewModel, User>();

		CreateMap<ProductCreateViewModel, Product>()
			.ForMember(p => p.Images, opt => opt.Ignore());
		CreateMap<ProductPutViewModel, Product>()
			.ForMember(p => p.Images, opt => opt.Ignore());
		CreateMap<Product, ProductItemViewModel>()
			.ForMember(
				dest => dest.Images,
				opt => opt.MapFrom(
					src => src.Images
						.Select(i => i.Name)
						.ToArray()
				)
			);

		CreateMap<BasketProduct, BasketItemViewModel>();
		CreateMap<BasketProduct, OrderedProduct>()
			.ForMember(
				dest => dest.UnitPrice,
				opt => opt.MapFrom(src => src.Product.Price)
			);

		CreateMap<OrderStatus, OrderStatusItemViewModel>();
		CreateMap<Order, OrderItemViewModel>();
		CreateMap<User, CustomerInfo>();
		CreateMap<Order, CustomerOrderItemViewModel>()
			.ForMember(
				dest => dest.Customer,
				opt => opt.MapFrom(
					src => src.User
				)
			);
		CreateMap<OrderedProduct, OrderedProductItemViewModel>();

		CreateMap<CreateAreaViewModel, Area>();
		CreateMap<Area, AreaItemViewModel>();

		CreateMap<CreateSettlementViewModel, Settlement>();
		CreateMap<Settlement, SettlementItemViewModel>();

		CreateMap<CreatePostOfficeViewModel, PostOffice>();
		CreateMap<PostOffice, PostOfficeItemViewModel>();
	}
}
