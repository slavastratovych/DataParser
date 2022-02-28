using System;
using System.Text.RegularExpressions;

namespace DataParser
{
    public class ModelBuilder
    {
        // TODO: Change from global match to match in all fields and extract specific value
        private static readonly Regex NameRegex = new Regex("^[А-Яа-я]+\\s+[А-Яа-я]+\\s[А-Яа-я]+$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex("^[\\w/._-]+@[\\w/._-]+.[A-Za-z]+$", RegexOptions.Compiled);

        private static int _uniqueID = 1;
        private Model model;

        public ModelBuilder()
        {
            model = new Model();
            model.ID = _uniqueID++;
        }

        public void SetValue(object value)
        {
            if (value is DateTime)
            {
                model.BirthDate = (DateTime)value;
                return;
            }

            if (value is string)
            {
                var stringValue = value as string;
                stringValue = stringValue.Trim();

                // Check for operator string
                if (stringValue.StartsWith("ПАО") || stringValue.StartsWith("ООО"))
                {
                    model.Operator = stringValue;
                    return;
                }

                if (EmailRegex.IsMatch(stringValue))
                {
                    model.Email = stringValue;
                    return;
                }

                if (NameRegex.IsMatch(stringValue))
                {
                    model.Name = stringValue;
                    return;
                }

                if (stringValue.StartsWith("UTC"))
                {
                    model.UTCOffset = stringValue;
                    return;
                }

                model.Address = stringValue;
                return;
            }

            if (value is double)
            {
                var doubleValue = (double)value;
                if (doubleValue.ToString().Length == 11)
                {
                    model.Phone = "+" + doubleValue.ToString();
                }
            }
        }

        public Model GetResult()
        {
            return model;
        }
    }
}
