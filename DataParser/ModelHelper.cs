using System;

namespace DataParser
{
    public static class ModelHelper
    {
        public static void Merge(Model target, Model source)
        {
            if (string.IsNullOrWhiteSpace(target.Name))
            {
                target.Name = source.Name;
            }

            if (string.IsNullOrWhiteSpace(target.Operator))
            {
                target.Operator = source.Operator;
            }

            if (string.IsNullOrWhiteSpace(target.Address))
            {
                target.Address = source.Address;
            }

            if (string.IsNullOrWhiteSpace(target.UTCOffset))
            {
                target.UTCOffset = source.UTCOffset;
            }

            if (target.BirthDate == null)
            {
                target.BirthDate = source.BirthDate;
            }
        }
    }
}
