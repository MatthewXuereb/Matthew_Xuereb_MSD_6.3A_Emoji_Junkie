using EmojiJunkie.Data;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

namespace EmojiJunkie
{
    public class Status : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statusText;

        void Update()
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("currentRound").GetValueAsync().ContinueWithOnMainThread(currentRoundTask =>
            {
                if (currentRoundTask.IsCompleted)
                {
                    bool switchRoles = int.Parse(currentRoundTask.Result.Value.ToString()) % 2 == 0 ? true : false;
                    GameData.switchRoles = switchRoles;
                }

                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("activePlayer").GetValueAsync().ContinueWithOnMainThread(activePlayerTask =>
                {
                    if (activePlayerTask.IsCompleted)
                    {
                        int activePlayer = int.Parse(activePlayerTask.Result.Value.ToString());

                        if (GameData.currentActivePlayer == 0) SetStatus("0");
                        else if (GameData.currentActivePlayer == 1) SetStatus("1");
                    }
                });
            });
        }

        private void SetStatus(string playerId)
        {
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child(playerId).Child("username").GetValueAsync().ContinueWithOnMainThread(playerNameTask =>
            {
                if (playerNameTask.IsCompleted)
                {
                    string player = playerNameTask.Result.Value.ToString();

                    _statusText.text = player + " Turn";
                }
            });
        }
    }
}
