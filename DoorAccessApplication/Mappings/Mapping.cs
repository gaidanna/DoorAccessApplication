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
        }
    }
}
