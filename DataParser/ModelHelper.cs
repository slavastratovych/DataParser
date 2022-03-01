using DataParser.Model;
using System.Collections.Generic;

namespace DataParser
{
    public static class ModelHelper
    {
        public static Person Merge(IEnumerable<Person> models)
        {
            Person result = null;

            foreach (var modelItem in models)
            {
                if (result == null)
                {
                    result = modelItem;
                }
                else
                {
                    Merge(result, modelItem);
                }
            }

            return result;
        }

        public static void Merge(Person target, Person source)
        {
            if (string.IsNullOrWhiteSpace(target.Name))
            {
                target.Name = source.Name;
            }

            if (string.IsNullOrWhiteSpace(target.Email))
            {
                target.Email = source.Email;
            }

            if (string.IsNullOrWhiteSpace(target.Phone))
            {
                target.Phone = source.Phone;
            }
        }
    }
}
