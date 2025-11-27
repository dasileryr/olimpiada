namespace Olimpiada.Models
{
    public class Olympics
    {
        public int OlympicsId { get; set; }
        public int Year { get; set; }
        public bool IsSummer { get; set; }
        public int HostCountryId { get; set; }
        public string City { get; set; } = string.Empty;
        public string HostCountryName { get; set; } = string.Empty;
    }
}

