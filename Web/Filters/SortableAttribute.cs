using System;

namespace GdShows.Filters
{
	public class SortableAttribute : Attribute
	{
		public string DefaultSortName { get; set; }
		public bool DefaultSortDescending { get; set; }

		public SortableAttribute(string defaultSortName)
		{
			DefaultSortName = defaultSortName;
		}
	}
}