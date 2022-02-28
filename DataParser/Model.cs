using System;

namespace DataParser
{
    public class Model
    {
        public int ID { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Operator { get; set; }

        public string UTCOffset { get; set; }
    }
}
