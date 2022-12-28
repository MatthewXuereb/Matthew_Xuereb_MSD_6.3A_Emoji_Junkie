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

        public void EndGame()
        {
            _countdownTimer.EndTimer();
            gameOverPanel.SetActive(true);

            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("activePlayer").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                if (task.IsFaulted)
                {
                    //
                }
                else if (task.IsCompleted)
                {
                    int activePlayer = int.Parse(task.Result.Value.ToString());

                    if (activePlayer == 0) _gameSceneManager.gameOverPanelWinnerText.text = "Player 2 Won";
                    else _gameSceneManager.gameOverPanelWinnerText.text = "Player 1 Won";
                }
            });
        }
    }
}
