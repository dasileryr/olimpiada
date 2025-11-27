namespace Olimpiada.Models
{
    public class Athlete
    {
        public int AthleteId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public byte[]? Photo { get; set; }
        public string FullName => $"{LastName} {FirstName} {MiddleName ?? ""}".Trim();
    }
}

