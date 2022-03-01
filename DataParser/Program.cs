using CsvHelper;
using CsvHelper.Configuration;
using DataParser.Model;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DataParser
{
    public class Program
    {
        // TODO: Move to application settings
        private static readonly string _connectionString = "Server=(local);Database=PeopleData;Trusted_Connection=True";

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Required for Excel import
            var dbContext = InitializeDbContext();

            string command = args[0];

            if (command == "import")
            {
                var importedData = new List<Person>();

                var filenames = GetFilenames(args.Skip(1));
                foreach (var filename in filenames)
                {
                    importedData.AddRange(ImportFromCsv(filename));
                }

                var filtertedData = importedData
                    .Where(x => x.Email != null)
                    .GroupBy(x => x.Email)
                    .ToDictionary(x => x.Key, x => ModelHelper.Merge(x))
                    .Select(x => x.Value);

                foreach (var item in filtertedData)
                {
                    dbContext.Persons.Add(item);
                }

                dbContext.SaveChanges();
            }

            if (command == "export")
            {
                string targetFile = args[1];

                using var writeStream = File.Open(targetFile, FileMode.OpenOrCreate);
                using var writer = new StreamWriter(writeStream, Encoding.UTF8)
                {
                    AutoFlush = true
                };

                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(dbContext.Persons);
            }
        }

        private static IEnumerable<Person> ImportFromCsv(string filename)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            using var reader = new StreamReader(filename);
            using var csv = new CsvReader(reader, config);

            var importedData = csv.GetRecords<CsvData>();

            return importedData.Select(x => new Person()
            {
                Name = x.Name == "NULL" ? null : x.Name,
                Email = x.Email == "NULL" ? null : x.Email.ToLower(),
                Phone = x.Phone == "NULL" ? null : x.Phone,
            }).ToList();
        }

        private static IEnumerable<Person> ImportFromExcel(string filename)
        {
            var data = new List<Person>();

            using FileStream readStream = File.Open(filename, FileMode.Open, FileAccess.Read);
            using IExcelDataReader reader = ExcelReaderFactory.CreateReader(readStream, new ExcelReaderConfiguration
            {
                FallbackEncoding = Encoding.GetEncoding(1252),
            });

            do
            {
                while (reader.Read()) // For each row
                {
                    var modelBuilder = new ModelBuilder();

                    for (int column = 0; column < reader.FieldCount; column++)
                    {
                        var value = reader.GetValue(column);
                        modelBuilder.SetValue(value);
                    }

                    data.Add(modelBuilder.GetResult());
                }


            } while (reader.NextResult()); // For each sheet

            return data;
        }

        private static List<string> GetFilenames(IEnumerable<string> args)
        {
            var filenames = new List<string>();

            foreach (string name in args)
            {
                if (Directory.Exists(name))
                {
                    filenames.AddRange(Directory.GetFiles(name));
                }
                else
                {
                    filenames.Add(name);
                }
            }

            return filenames;
        }

        private static PeopleContext InitializeDbContext()
        {
            var services = new ServiceCollection();
            services.AddDbContext<PeopleContext>(options => options.UseSqlServer(_connectionString));

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetService<PeopleContext>();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        private void AddOrUpdate()
        {
            //foreach (var newPerson in importedData)
            //{
            //    Person oldPerson = dbContext.Persons.SingleOrDefault(x => x.Email == newPerson.Email);

            //    if (oldPerson != null)
            //    {
            //        if (!newPerson.Equals(oldPerson))
            //        {
            //            ModelHelper.Merge(oldPerson, newPerson);
            //            dbContext.Persons.Update(oldPerson);
            //        }
            //    }
            //    else if (newPerson.Email != null)
            //    {
            //        dbContext.Persons.Add(newPerson);
            //    }
            //}
        }
    }
}
