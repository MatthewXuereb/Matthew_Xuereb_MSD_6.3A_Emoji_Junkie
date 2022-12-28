using EmojiJunkie.Data;
using EmojiJunkie.Dev;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace EmojiJunkie.UI
{
    public class Surrender : MonoBehaviour
    {
        [Header("Scripts")]
        [SerializeField] private CountdownTimer _countdownTimer;
        [SerializeField] private GameSceneManager _gameSceneManager;

        [Header("Panels")]
        [SerializeField] private GameObject gameOverPanel;

        private void Update()
        {
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("endGame").GetValueAsync().ContinueWithOnMainThread(endGameTask =>
            {
                if (endGameTask.IsCompleted)
                {
                    bool endGame = bool.Parse(endGameTask.Result.Value.ToString());
                    if (endGame) EndGame();
                }
            });
        }

        public void EndGame()
        {
            if (GameData.currentActivePlayer == GameData.playerId)
            {
                _countdownTimer.EndTimer();
                gameOverPanel.SetActive(true);

                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("activePlayer").GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        int activePlayer = int.Parse(task.Result.Value.ToString());

                        FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("endGame").GetValueAsync().ContinueWithOnMainThread(endGameTask =>
                        {
                            if (endGameTask.IsCompleted)
                            {
                                bool endGame = bool.Parse(endGameTask.Result.Value.ToString());

                                DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                                if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                                reference.Child(GameData.connectedRoom).Child("endGame").SetValueAsync("true");

                                if (activePlayer == 0) SetName("1");
                                else if (activePlayer == 1) SetName("0");
                            }
                        });
                    }
                });
            }
        }

        private void SetName(string playerId)
        {
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child(playerId).Child("username").GetValueAsync().ContinueWithOnMainThread(playerNameTask =>
            {
                if (playerNameTask.IsCompleted)
                {
                    string player = playerNameTask.Result.Value.ToString();

                    _gameSceneManager.gameOverPanelWinnerText.text = player + " Won";
                }
            });
        }
    }
}
