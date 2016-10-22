namespace Models.Api
{
	public class Pagable: Sortable
	{
		public int? Offset { get; set; }
		public int? Limit { get; set; }
	}
}