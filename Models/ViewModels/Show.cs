
using System;

namespace Models.ViewModels
{

	public class Show : BaseViewModel
	{
		public DateTime? ShowDate { get; set; } // ShowDate
		public string Venue { get; set; } // Venue (length: 256)
		public string City { get; set; } // City (length: 64)
		public string State { get; set; } // State (length: 32)
		public string Country { get; set; } // Country (length: 64)
		public Set [] Sets { get; set; }
	}
}
