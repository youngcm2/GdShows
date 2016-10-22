using AutoMapper;
using Models.ViewModels;

namespace GdShows.API.V1.Profiles
{
	public class SetProfile : Profile
	{
		public SetProfile()
		{
			// Sending to client
			CreateMap<Data.Set, Set>();


			// Sending to service/data layer
			CreateMap<Set, Data.Set>();

		}
	}
}