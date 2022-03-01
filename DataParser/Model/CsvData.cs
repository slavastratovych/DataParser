using CsvHelper.Configuration.Attributes;
using System;

namespace DataParser.Model
{
    internal class CsvData
    {
        [Index(1)]
        [TypeConverter(typeof(CsvHelper.TypeConversion.StringConverter))]
        public string Phone { get; set; }

        [Index(0)]
        [TypeConverter(typeof(CsvHelper.TypeConversion.StringConverter))]
        public string Email { get; set; }

        [Index(2)]
        [TypeConverter(typeof(CsvHelper.TypeConversion.StringConverter))]
        public string Name { get; set; }
    }
}
