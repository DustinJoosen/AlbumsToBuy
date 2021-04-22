namespace AlbumsToBuy.Models
{
	public interface IAlbumCollection
	{
		public Album Album { get; set; }
		public int Quantity { get; set; }
	}
}