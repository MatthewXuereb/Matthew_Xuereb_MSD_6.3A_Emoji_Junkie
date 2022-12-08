namespace EmojiJunkie.Data
{
    public static class GameData
    {
        public static int currentRound = 1;
        public static int currentTurn = 1;
        private static int _numberOfRounds = 8;
        public static int numberOfTurnsInRound = 2;

        public static bool EndGanme()
        {
            if (currentRound >= _numberOfRounds)
                return true;
            else
                return false;
        }

        public static void ResetGame()
        {
            currentRound = 1;
            currentTurn = 1;
        }
    }
}
