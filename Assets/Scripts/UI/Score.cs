using EmojiJunkie.Data;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

namespace EmojiJunkie.UI
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _player1Score;
        [SerializeField] private TextMeshProUGUI _player2Score;

        private FirebaseController _firebaseController;

        private void Awake()
        {
            _firebaseController = FindObjectOfType<FirebaseController>();
        }

        void Update()
        {
            //
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("score").GetValueAsync().ContinueWithOnMainThread(player1Task =>
            {
                if (player1Task.IsCompleted)
                {
                    FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("1").Child("score").GetValueAsync().ContinueWithOnMainThread(player2Task =>
                    {
                        if (player2Task.IsCompleted)
                        {
                            float player1Score = float.Parse(player1Task.Result.Value.ToString());
                            float player2Score = float.Parse(player2Task.Result.Value.ToString());

                            _player1Score.text = "Player 1: " + player1Score.ToString();
                            _player2Score.text = "Player 2: " + player2Score.ToString();
                        }
                    });
                }
            });
            //

            /*_firebaseController.GetPlayerScore("0");
            _firebaseController.GetPlayerScore("1");

            _player1Score.text = "Player 1: " + GameData.player1Score.ToString();
            _player2Score.text = "Player 2: " + GameData.player2Score.ToString();*/
        }
    }
}
