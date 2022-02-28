using CsvHelper;
using ExcelDataReader;
using System;
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
            string[] filenames = args;
            var data = new List<Model>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
                        var model = new Model();

                        for (int column = 0; column < reader.FieldCount; column++)
                        {
                            var value = reader.GetValue(column); //Get Value returns object
                            ParseValue(model, value);
                        }

                        data.Add(model);
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

            csv.WriteRecords(data.Distinct());
        }

        private static void ParseValue(Model model, object value)
        {
            if (value is DateTime)
            {
                model.BirthDate = (DateTime)value;
                return;
            }

            if (value is string)
            {
                var stringValue = value as string;

                // Check for operator string
                if (stringValue.StartsWith("ПАО") || stringValue.StartsWith("ООО"))
                {
                    model.Operator = stringValue;
                }
                // Check for name
                else if (stringValue.Split(' ').Length == 3 && !stringValue.Contains("-") && !stringValue.Contains("Республика"))
                {
                    model.Name = stringValue;
                }
                else if (stringValue.StartsWith("UTC"))
                {
                    model.UTCOffset = stringValue;
                }
                else
                {
                    model.Address = stringValue;
                }
                return;
            }

            if (value is double)
            {
                var doubleValue = (double)value;
                if (doubleValue.ToString().Length == 11)
                {
                    model.Phone = "+" + doubleValue.ToString();
                }
                else
                {
                    model.Unknown = doubleValue;
                }
            }
        }
    }
}
