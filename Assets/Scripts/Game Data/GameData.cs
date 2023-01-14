namespace EmojiJunkie.Data
{
    public static class GameData
    {
        private static int _numberOfRounds = 8;

        public static string connectedRoom;

        public static int playerId;
        public static int currentActivePlayer;

        public static bool playerIsHost = false;
        public static bool switchRoles = false;

        public static bool EndGanme(int currentRound)
        {
            if (currentRound > _numberOfRounds)
                return true;
            else
                return false;
        }
    }
}
