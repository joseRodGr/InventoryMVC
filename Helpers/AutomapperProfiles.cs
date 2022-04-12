using AutoMapper;
using InventoryMVC.Models;
using InventoryMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Helpers
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<Product, EditProductViewModel>().ReverseMap();

            CreateMap<CreateProductViewModel, Product>();

            CreateMap<Category, CategoryViewModel>();

            CreateMap<CreateCategoryViewModel, Category>();

            CreateMap<Category, EditCategoryViewModel>().ReverseMap();

            CreateMap<Supplier, SupplierViewModel>();

            CreateMap<CreateSupplierViewModel, Supplier>();

            CreateMap<Supplier, EditSupplierViewModel>().ReverseMap();
               
        }
    }
}
