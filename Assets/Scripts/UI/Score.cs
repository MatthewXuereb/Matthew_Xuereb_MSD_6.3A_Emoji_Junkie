using EmojiJunkie.Data;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

namespace EmojiJunkie.UI
{
    public class Score : MonoBehaviour
    {
        private string _player1Name;
        private string _player2Name;

        [SerializeField] private TextMeshProUGUI _player1Score;
        [SerializeField] private TextMeshProUGUI _player2Score;

        private void Start()
        {
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("username").GetValueAsync().ContinueWithOnMainThread(playerTask =>
            {
                if (playerTask.IsCompleted) _player1Name = playerTask.Result.Value.ToString();
            });
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("username").GetValueAsync().ContinueWithOnMainThread(playerTask =>
            {
                if (playerTask.IsCompleted) _player2Name = playerTask.Result.Value.ToString();
            });
        }

        void Update()
        {
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("score").GetValueAsync().ContinueWithOnMainThread(player1Task =>
            {
                if (player1Task.IsCompleted)
                {
                    float player1Score = float.Parse(player1Task.Result.Value.ToString());
                    _player1Score.text = _player1Name + ": " + player1Score.ToString();
                }
            });
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("1").Child("score").GetValueAsync().ContinueWithOnMainThread(player2Task =>
            {
                if (player2Task.IsCompleted)
                {
                    float player2Score = float.Parse(player2Task.Result.Value.ToString());
                    _player2Score.text = _player2Name + ": " + player2Score.ToString();
                }
            });
        }
    }
}
