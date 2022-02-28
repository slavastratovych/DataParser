using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser
{
    public class Model
    {
        public string Phone { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string Address { get; set; }

        public string Operator { get; set; }

        public string UTCOffset { get; set; }

        public double Unknown { get; set; }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            if(ReferenceEquals(this, obj))
            {
                return true;
            }

            if(obj.GetType() != typeof(Model))
            {
                return false;
            }

            Model that = (Model)obj;

            return Phone == that.Phone
                && Name == that.Name
                && BirthDate == that.BirthDate
                && Address == that.Address
                && Operator == that.Operator
                && UTCOffset == that.UTCOffset;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
