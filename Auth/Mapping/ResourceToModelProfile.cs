using Auth.Controllers.Resources;
using Auth.Core.Models;
using AutoMapper;

namespace Auth.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile() 
        {
            CreateMap<UserCredentialResource, User>();
        }
    }
}
