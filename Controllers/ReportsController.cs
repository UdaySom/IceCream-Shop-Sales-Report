using Microsoft.AspNetCore.Mvc;
using IceCreamParlour.Models;
using System.Globalization;

namespace IceCreamParlour.Controllers
{
	public class ReportsController : Controller
	{

		private readonly List<SalesRecord> _records;

		public ReportsController()
		{
			_records = LoadSalesData("wwwroot/data/data.txt");
		}

		private List<SalesRecord> LoadSalesData(string filePath)
		{
			var records = new List<SalesRecord>();
			var lines = System.IO.File.ReadAllLines(filePath).Skip(1); // Skip header

			foreach (var line in lines)
			{
				var parts = line.Split(',');

				if (parts.Length == 5 &&
				DateTime.TryParse(parts[0], out var date) &&
				decimal.TryParse(parts[2], out var unitPrice) &&
				int.TryParse(parts[3], out var quantity) &&
				decimal.TryParse(parts[4], out var totalPrice))
				{
					records.Add(new SalesRecord
					{
						Date = date,
						SKU = parts[1],
						UnitPrice = unitPrice,
						Quantity = quantity,
						TotalPrice = totalPrice
					});
				}
			}

			return records;
		}

		public IActionResult Index()
		{
			return View(_records.Take(10)); // Just to test loading
		}


		public IActionResult TotalSales()
		{
			decimal totalSales = _records.Sum(r => r.TotalPrice);
			ViewBag.TotalSales = totalSales;
			return View();
		}

		public IActionResult MonthWiseSales()
		{
			var monthSales = new Dictionary<string, decimal>();

			foreach (var record in _records)
			{
				string month = record.Date.ToString("MMM");

				if (!monthSales.ContainsKey(month))
				{
					monthSales[month] = 0;
				}

				monthSales[month] += record.TotalPrice;
			}

			// Sort months manually (Jan to Dec)
			var monthOrder = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
							 "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

			var sortedMonths = monthOrder.Where(m => monthSales.ContainsKey(m)).ToList();
			var sortedSales = sortedMonths.Select(m => monthSales[m]).ToList();

			ViewBag.Months = sortedMonths;
			ViewBag.Sales = sortedSales;

			return View();
		}

		public IActionResult MostPopularItems()
		{
			var monthItemStats = new Dictionary<string, Dictionary<string, List<int>>>();

			foreach (var record in _records)
			{
				string month = record.Date.ToString("MMM");
				string sku = record.SKU;
				int quantity = record.Quantity;

				if (!monthItemStats.ContainsKey(month))
					monthItemStats[month] = new Dictionary<string, List<int>>();

				if (!monthItemStats[month].ContainsKey(sku))
					monthItemStats[month][sku] = new List<int>();

				monthItemStats[month][sku].Add(quantity);
			}

			var months = new List<string>();
			var minList = new List<int>();
			var maxList = new List<int>();
			var avgList = new List<double>();
			var itemNames = new List<string>();

			foreach (var month in monthItemStats.Keys.OrderBy(m => DateTime.ParseExact(m, "MMM", null).Month))
			{
				var items = monthItemStats[month];
				string mostPopular = items.OrderByDescending(i => i.Value.Sum()).First().Key;
				var quantities = items[mostPopular];

				months.Add(month);
				minList.Add(quantities.Min());
				maxList.Add(quantities.Max());
				avgList.Add(quantities.Average());
				itemNames.Add(mostPopular);
			}

			ViewBag.Months = months;
			ViewBag.Min = minList;
			ViewBag.Max = maxList;
			ViewBag.Avg = avgList;
			ViewBag.Items = itemNames;

			return View();
		}
		public IActionResult RevenueByItem()
		{
			var monthItemRevenue = new Dictionary<string, Dictionary<string, decimal>>();

			foreach (var record in _records)
			{
				string month = record.Date.ToString("yyyy-MM");
				if (!monthItemRevenue.ContainsKey(month))
					monthItemRevenue[month] = new Dictionary<string, decimal>();

				if (!monthItemRevenue[month].ContainsKey(record.SKU))
					monthItemRevenue[month][record.SKU] = 0;

				monthItemRevenue[month][record.SKU] += record.TotalPrice;
			}

			ViewBag.Months = monthItemRevenue.Keys.OrderBy(m => m).ToList();
			ViewBag.MonthItemRevenue = monthItemRevenue;

			return View();
		}

		public IActionResult Dashboard()
		{
			// Total Sales
			ViewBag.TotalSales = _records.Sum(r => r.TotalPrice);

			// Month-wise Sales
			var monthSales = new Dictionary<string, decimal>();
			foreach (var record in _records)
			{
				string month = record.Date.ToString("MMM");
				if (!monthSales.ContainsKey(month))
					monthSales[month] = 0;
				monthSales[month] += record.TotalPrice;
			}
			var monthOrder = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
							 "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
			var sortedMonths = monthOrder.Where(m => monthSales.ContainsKey(m)).ToList();
			var sortedSales = sortedMonths.Select(m => monthSales[m]).ToList();
			ViewBag.Months = sortedMonths;
			ViewBag.Sales = sortedSales;

			// Most Popular Items
			var monthItemStats = new Dictionary<string, Dictionary<string, List<int>>>();
			foreach (var record in _records)
			{
				string month = record.Date.ToString("MMM");
				string sku = record.SKU;
				int quantity = record.Quantity;
				if (!monthItemStats.ContainsKey(month))
					monthItemStats[month] = new Dictionary<string, List<int>>();
				if (!monthItemStats[month].ContainsKey(sku))
					monthItemStats[month][sku] = new List<int>();
				monthItemStats[month][sku].Add(quantity);
			}
			var months = new List<string>();
			var minList = new List<int>();
			var maxList = new List<int>();
			var avgList = new List<double>();
			var itemNames = new List<string>();
			foreach (var month in monthItemStats.Keys.OrderBy(m => DateTime.ParseExact(m, "MMM", null).Month))
			{
				var items = monthItemStats[month];
				string mostPopular = items.OrderByDescending(i => i.Value.Sum()).First().Key;
				var quantities = items[mostPopular];
				months.Add(month);
				minList.Add(quantities.Min());
				maxList.Add(quantities.Max());
				avgList.Add(quantities.Average());
				itemNames.Add(mostPopular);
			}
			ViewBag.PopularMonths = months;
			ViewBag.Min = minList;
			ViewBag.Max = maxList;
			ViewBag.Avg = avgList;
			ViewBag.Items = itemNames;

			// Revenue by Item
			var monthItemRevenue = new Dictionary<string, Dictionary<string, decimal>>();
			foreach (var record in _records)
			{
				string month = record.Date.ToString("yyyy-MM");
				if (!monthItemRevenue.ContainsKey(month))
					monthItemRevenue[month] = new Dictionary<string, decimal>();
				if (!monthItemRevenue[month].ContainsKey(record.SKU))
					monthItemRevenue[month][record.SKU] = 0;
				monthItemRevenue[month][record.SKU] += record.TotalPrice;
			}
			ViewBag.RevenueMonths = monthItemRevenue.Keys.OrderBy(m => m).ToList();
			ViewBag.MonthItemRevenue = monthItemRevenue;

			return View();
		}




	}
}
