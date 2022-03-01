namespace DataParser.Model
{
    public class Person
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is Person)
            {
                Person other = obj as Person;
                return Phone == other.Phone && Email == other.Email && Name == other.Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Phone.GetHashCode() ^ Email.GetHashCode() ^ Name.GetHashCode();
        }
    }
}
