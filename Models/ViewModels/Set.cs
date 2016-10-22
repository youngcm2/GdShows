using System;

namespace Models.ViewModels
{
	public class Set : BaseViewModel
	{
		public Guid ShowId { get; set; } // ShowId
		public byte? SetNumber { get; set; } // SetNumber
		public bool IsEncore { get; set; } // IsEncore
		public Song[] Songs { get; set; }
	}
}