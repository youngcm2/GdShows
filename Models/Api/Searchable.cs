namespace Models.Api
{
	public class Searchable<T> : Pagable
	{
		public T Value { get; set; }
	}
}