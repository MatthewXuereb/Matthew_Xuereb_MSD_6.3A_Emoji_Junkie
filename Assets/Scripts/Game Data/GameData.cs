namespace EmojiJunkie.Data
{
    public static class GameData
    {
        public static int currentRound = 1;
        private static int _numberOfRounds = 8;

        public static int activePlayer = 0;
        public static float player1Score = 0;
        public static float player2Score = 0;

        public static bool EndGanme()
        {
            if (currentRound > _numberOfRounds)
                return true;
            else
                return false;
        }

        public static void ResetGame()
        {
            activePlayer = 0;
            player1Score = 0;
            player2Score = 0;

            currentRound = 1;
        }
    }
}
