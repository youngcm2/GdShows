using AutoMapper;
using Models.ViewModels;

namespace GdShows.API.V1.Profiles
{
	public class SongProfile : Profile
	{
		public SongProfile()
		{
			// Sending to client
			CreateMap<Data.Song, Song>();


			// Sending to service/data layer
			CreateMap<Song, Data.Song>();

		}
	}
}