using DataParser.Model;
using System.Text.RegularExpressions;

namespace DataParser
{
    public class ModelBuilder
    {
        // TODO: Change from global match to match in all fields and extract specific value
        private static readonly Regex NameRegex = new Regex("^[А-Яа-я]+\\s+[А-Яа-я]+\\s[А-Яа-я]+$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex("^[\\w/._-]+@[\\w/._-]+.[A-Za-z]+$", RegexOptions.Compiled);
        private static readonly Regex PhoneRegex = new Regex("^\\+?\\d+$", RegexOptions.Compiled);

        private Person model;

        public ModelBuilder()
        {
            model = new Person();
        }

        public void SetValue(object value)
        {
            var stringValue = value.ToString();
            stringValue = stringValue.Trim();

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

            if (PhoneRegex.IsMatch(stringValue))
            {
                model.Phone = stringValue;
                return;
            }
        }

        public Person GetResult()
        {
            return model;
        }
    }
}
