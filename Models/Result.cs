namespace Olimpiada.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int OlympicsId { get; set; }
        public int SportId { get; set; }
        public int AthleteId { get; set; }
        public int MedalType { get; set; } // 1 - золото, 2 - серебро, 3 - бронза
        public string AthleteName { get; set; } = string.Empty;
        public string SportName { get; set; } = string.Empty;
        public string MedalName => MedalType switch
        {
            1 => "Золото",
            2 => "Серебро",
            3 => "Бронза",
            _ => "Неизвестно"
        };
    }
}

