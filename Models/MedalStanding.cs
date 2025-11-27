namespace Olimpiada.Models
{
    public class MedalStanding
    {
        public string CountryName { get; set; } = string.Empty;
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Bronze { get; set; }
        public int Total => Gold + Silver + Bronze;
    }
}

