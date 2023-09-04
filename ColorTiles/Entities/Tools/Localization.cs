using System.Collections.Generic;

namespace ColorTiles.Entities.Tools
{
    public class Localization
    {
        public string Country { get; set; }
        public string Language { get; set; }
        public string CountryCode => $"{Country}-{Language}";

        public GameStrings Strings { get; set; }

        public Localization(string language, string country)
        {
            Language = language;
            Country = country;

            Strings = new GameStrings();
        }

        public Localization(string language, string country, GameStrings strings) : this(language, country)
        {
            Strings = strings;
        }

        public static Localization Default => new("en", "US", GameStrings.Default);
    }
}