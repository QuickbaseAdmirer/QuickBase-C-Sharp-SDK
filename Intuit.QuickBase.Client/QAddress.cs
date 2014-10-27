namespace Intuit.QuickBase.Client
{
    public class QAddress
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public QAddress(string line1, string line2, string city, string province, string postCode, string country)
        {
            Line1 = line1;
            Line2 = line2;
            City = city;
            Province = province;
            PostalCode = postCode;
            Country = country;
        }
    }
}
