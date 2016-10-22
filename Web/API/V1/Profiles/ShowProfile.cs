using System;
using System.Collections.Generic;
using AutoMapper;
using Show = Models.ViewModels.Show;

namespace GdShows.API.V1.Profiles
{
    public class ShowProfile : Profile
    {
        public ShowProfile()
        {
			// Sending to client
	        CreateMap<Data.Show, Show>();
		        
			
			// Sending to service/data layer
	        CreateMap<Show, Data.Show>();

        }
    }
}
