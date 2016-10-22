using System;

namespace Models.ViewModels
{
	public class Song : BaseViewModel
	{
		public Guid SongRefId { get; set; }
		public string Name { get; set; } // SongRefId
		public byte Position { get; set; } // Position
		public bool IsSegued { get; set; }
	}
}