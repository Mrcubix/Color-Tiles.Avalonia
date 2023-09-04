namespace ColorTiles.Entities.Tools
{
    public class GameStrings
    {
        public string ScoreLabel { get; set; } = string.Empty;
        public string PlayAgainLabel { get; set; } = string.Empty;
        public string QuitLabel { get; set; } = string.Empty;

        public GameStrings()
        {
        }

        public GameStrings(string scoreLabel, string playAgainLabel, string quitLabel)
        {
            ScoreLabel = scoreLabel;
            PlayAgainLabel = playAgainLabel;
            QuitLabel = quitLabel;
        }

        public static GameStrings Default => new("Score", "Play Again", "Quit W");
    }
}