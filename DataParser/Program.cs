using CsvHelper;
using ExcelDataReader;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DataParser
{
    internal class Program
    {
        static void Main(string[] args)
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

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var data = new List<Model>();
            foreach (var filename in filenames)
            {
                using var readStream = File.Open(filename, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(readStream, new ExcelReaderConfiguration
                {
                    FallbackEncoding = Encoding.GetEncoding(1252),
                });

                do
                {
                    while (reader.Read()) //Each ROW
                    {
                        var modelBuilder = new ModelBuilder();

                        for (int column = 0; column < reader.FieldCount; column++)
                        {
                            var value = reader.GetValue(column); //Get Value returns object
                            modelBuilder.SetValue(value);
                        }

                        data.Add(modelBuilder.GetResult());
                    }


                } while (reader.NextResult()); //Move to NEXT SHEET
            }

            string targetFile = "output.csv";

            using var writeStream = File.Open(targetFile, FileMode.OpenOrCreate);
            using var writer = new StreamWriter(writeStream, Encoding.UTF8)
            {
                AutoFlush = true
            };

            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            var dataByPhone = data
                .Where(x => !string.IsNullOrWhiteSpace(x.Phone))
                .GroupBy(x => x.Phone)
                .ToDictionary(x => x.Key, x => DoMerge(x))
                .Where(x => !string.IsNullOrWhiteSpace(x.Value.Name))
                .Select(x => x.Value);

            csv.WriteRecords(dataByPhone);
        }

        private static Model DoMerge(IEnumerable<Model> models)
        {
            Model result = null;

            foreach (var modelItem in models)
            {
                if (result == null)
                {
                    result = modelItem;
                }
                else
                {
                    ModelHelper.Merge(result, modelItem);
                }
            }

            return result;
        }
    }
}
