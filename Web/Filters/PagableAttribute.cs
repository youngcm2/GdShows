namespace GdShows.Filters
{
    public class PagableAttribute : SortableAttribute
    {
	    public int DefaultLimit { get; set; } = 100;

	    public PagableAttribute(string defaultSortName): base(defaultSortName)
	    {
		    
	    }
    }
}
