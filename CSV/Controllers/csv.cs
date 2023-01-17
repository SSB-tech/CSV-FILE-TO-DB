using ClosedXML.Excel;
using CSV.Model;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.IO;

namespace CSV.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class csv : ControllerBase
	{

		private readonly IConfiguration configuration;

		public csv(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		//Storing specified csv file in db

		//[HttpPost]
		//public async Task<IActionResult> uploadcsc()
		//{
		//	//This will read files line by line and store in an array
		//	string[] csvlines = System.IO.File.ReadAllLines(@"C:\Users\basne\OneDrive\Desktop\nsale_dist.csv");

		//	List<model> list = new List<model>();

		//	for(int i=0; i<csvlines.Length; i++) 
		//	{	
		//		string[] rowdata = csvlines[i].Split('^'); //Will split each line of array whenever it encounters the split symbol

		//		list.Add(new model //Adding value to the model class so that we can transfer it to the database
		//		{
		//			CountryCode= rowdata[0],
		//			CountryName= rowdata[1]
		//		});

		//		//Console.WriteLine((csvlines[i])); 
		//	}
		//	var conn = new SqlConnection(configuration.GetConnectionString("connection"));
		//	await conn.ExecuteAsync("insert into csv (CountryCode, CountryName) values (@CountryCode,@CountryName)", list);
		//	return Ok(list);
		//}




		//Storing Uploaded csv file in db
		[HttpPost]
		public async Task<IActionResult> uploadcsc(IFormFile file)
		{
			if (file.Length <= 0)
				return BadRequest("Empty file");

			//Finding name of file
			var originalFileName = file.FileName;

			//Creating a unique file path
			//var uniqueFileName = Path.GetRandomFileName();

			//Saving file locally
			var uniqueFilePath = Path.Combine(@"C:\Users\basne\OneDrive\Desktop", originalFileName);

			using (var stream = System.IO.File.Create(uniqueFilePath))
			{
				await file.CopyToAsync(stream);
			}

			//This will read files line by line and store in an array
			string[] csvlines = System.IO.File.ReadAllLines(uniqueFilePath);

			List<model> list = new List<model>();

			for (int i = 0; i < csvlines.Length; i++)
			{
				string[] rowdata = csvlines[i].Split('^'); //Will split each line of array whenever it encounters the split symbol

				list.Add(new model //Adding value to the model class so that we can transfer it to the database
				{
					CountryCode = rowdata[0],
					CountryName = rowdata[1]
				});

				//Console.WriteLine((csvlines[i])); 
			}
			var conn = new SqlConnection(configuration.GetConnectionString("connection"));
			await conn.ExecuteAsync("insert into csv (CountryCode, CountryName) values (@CountryCode,@CountryName)", list);
			return Ok(list);
		}
	}
}
