using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StinkyModel;
using StinkyProjectServer.Data;

namespace StinkyProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(Comp584DatabaseContext context, IHostEnvironment environment) : ControllerBase
    {
        string _pathName = Path.Combine(environment.ContentRootPath, "Data/2020.csv");

        [HttpPost("Make")]
        public async Task<ActionResult> ImportMakeAsync()
        {
            Dictionary<string, Make> makesByName = context.Makes
                .AsNoTracking().ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            List<StinkyDTO> records = csv.GetRecords<StinkyDTO>().ToList();
            foreach (StinkyDTO record in records)
            {
                if (makesByName.ContainsKey(record.make))
                {
                    continue;
                }

                Make make = new()
                {
                    Name = record.make,
                };
                await context.Makes.AddAsync(make);
                makesByName.Add(record.make, make);
            }

            await context.SaveChangesAsync();

            return new JsonResult(makesByName.Count);
        }

        [HttpPost("Car")]
        public async Task<ActionResult> ImportCarAsync()
        {
            Dictionary<string, Make> makes = await context.Makes.ToDictionaryAsync(c => c.Name.Trim());

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };
            int carCount = 0;
            using (StreamReader reader = new(_pathName))
            using (CsvReader csv = new(reader, config))
            {
                IEnumerable<StinkyDTO>? records = csv.GetRecords<StinkyDTO>();
                foreach (StinkyDTO record in records)
                {
                    if (!makes.TryGetValue(record.make, out Make? value))
                    {
                        Console.WriteLine($"Not found make for {record.model}");
                        return NotFound(makes);
                    }


                    Car car = new()
                    {
                        Model = record.model,
                        Year = (int)record.year,
                        MakeId = value.MakeId,
                    };
                    context.Cars.Add(car);
                    carCount++;
                }
                await context.SaveChangesAsync();
            }
            return new JsonResult(carCount);
        }
    }
}