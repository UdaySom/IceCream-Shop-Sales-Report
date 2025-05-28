namespace IceCreamParlour.Models
{
	public class SalesRecord
	{

		public DateTime Date { get; set; }
		public string SKU { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }

		public string MonthYear => Date.ToString("yyyy-MM");


	}
}
