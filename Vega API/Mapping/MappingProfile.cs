using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Vega_API.Controllers.Resources;
using Vega_API.Models;

namespace Vega_API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //domain to api
            CreateMap<Make, MakeResource>();
            CreateMap<Make, KeyValuePairResource>();
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<Vehicle, SaveVehicleResource>()
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr=>vr.Make,opt=>opt.MapFrom(v=>v.Model.Make))
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => new KeyValuePairResource { Id = vf.Feature.Id,Name=vf.Feature.Name})));



            //api to domain
            CreateMap<SaveVehicleResource, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
                .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
                .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
                .ForMember(v => v.Features, opt => opt.Ignore())
                
                .AfterMap((vr, v) => {

                    //remove un selected

                    var removedFeature = v.Features.Where(f => !vr.Features.Contains(f.FeatureId));
                    foreach (var f in removedFeature)
                        v.Features.Remove(f);

                    //add new feature
                    
                    var addedFeature = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id=>new VehicleFeature { FeatureId = id });
                    foreach (var f in addedFeature)
                        v.Features.Add(f);

                });
        }        
    }
}

    
