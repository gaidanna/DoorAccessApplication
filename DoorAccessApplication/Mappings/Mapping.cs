using AutoMapper;
using DoorAccessApplication.Api.Models;
using DoorAccessApplication.Core.Models;

namespace DoorAccessApplication.Api.Mappings
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateLockRequest, Lock>();
            CreateMap<UpdateLockRequest, Lock>();
            CreateMap<Lock, LockResponse>()
                .ForMember(s => s.Status, c => c.MapFrom(m => m.Status.ToString()));

            CreateMap<Lock, LockWithUsersResponse>()
                .ForMember(s => s.Users, c => c.MapFrom(m => m.Users));
            
            CreateMap<User, UserResponse>();

            CreateMap<LockHistoryEntry, LockHistoryEntryResponse>()
                .ForMember(x => x.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }    
    }
}
