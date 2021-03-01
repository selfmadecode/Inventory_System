using System;
using System.Collections.Generic;
using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppWebUi.Models;

namespace inventoryAppWebUi.Infrastructures
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<EditProfileViewModel, Pharmacist>()
                .ForMember(pharmacist => pharmacist.ApplicationUser, act => act.Ignore())
                .ForMember(pharmacist => pharmacist.ApplicationUserId, act => act.Ignore());
            
            Mapper.CreateMap<EditProfileViewModel, StoreManager>()
                .ForMember(pharmacist => pharmacist.ApplicationUser, act => act.Ignore())
                .ForMember(pharmacist => pharmacist.ApplicationUserId, act => act.Ignore());

            Mapper.CreateMap<SupplierViewModel, Supplier>()
               .ForMember(s => s.Status, s => s.Ignore());

             Mapper.CreateMap<Supplier, SupplierViewModel>();
        }
    }
}