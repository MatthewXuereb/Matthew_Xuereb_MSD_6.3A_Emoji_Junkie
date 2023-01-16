namespace EmojiJunkie.Data
{
    public static class GameData
    {
        public static int numberOfRounds = 8;

        public static string connectedRoom;

        public static int playerId;
        public static int currentRound = 1;
        public static int currentRoundIndex = 0;
        public static int currentActivePlayer;
        public static int currentSelectedIcon;

        public static bool playerIsHost = false;
        public static bool switchRoles = false;

        public static bool EndGanme(int currentRound)
        {
            if (currentRound > numberOfRounds)
                return true;
            else
                return false;
        }

        public static void SetCurrentRound()
        {
            currentRoundIndex = currentRound - 1;

            if (currentRoundIndex < 0) currentRoundIndex = 0;
            if (currentRoundIndex >= numberOfRounds) currentRoundIndex = numberOfRounds - 1;
        }
    }
}
