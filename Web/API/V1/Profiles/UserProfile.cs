using System.Collections.Generic;
using AutoMapper;
using User = Models.ViewModels.User;

namespace GdShows.API.V1.Profiles
{
    public class UserProfile : Profile
    {        
        public UserProfile()
        {
            CreateMap<Data.User, User>();
        }
    }
}
