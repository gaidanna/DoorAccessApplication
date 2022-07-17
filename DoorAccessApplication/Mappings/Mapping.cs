using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using DoorAccessApplication.Api.Models;
using DoorAccessApplication.Api.ValueTypes;
using DoorAccessApplication.Core.Extensions;
using DoorAccessApplication.Core.Models;
using DoorAccessApplication.Core.ValueTypes;

namespace DoorAccessApplication.Api.Mappings
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateLockRequest, Lock>();
            CreateMap<UpdateLockRequest, Lock>();
            CreateMap<Lock, LockResponse>();
            
            CreateMap<Lock, LockWithUsersResponse>()
                .ForMember(s => s.Users, c => c.MapFrom(m => m.Users))
                .ForSourceMember(x => x.HistoryEntries, opt => opt.DoNotValidate());
            CreateMap<User, UserResponse>();


            //CreateMap<LockStatusType, StatusType>()
            //.ConvertUsingEnumMapping(opt => opt.MapByName()).ReverseMap();

            CreateMap<LockHistoryEntry, LockHistoryEntryResponse>()
                .ForMember(x => x.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            //CreateMap<LockHistoryEntryResponse, LockHistoryEntry>()
            //    .ForMember(x => x.Status, opt => opt.MapFrom(src => src.Status.GetStatus()));
        }    
    }
}
