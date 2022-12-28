using EmojiJunkie.Data;
using Firebase.Database;
using Firebase.Extensions;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EmojiJunkie.Dev
{
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Image _gamePanel;

        [Header("Panels")]
        [SerializeField] private GameObject _emojiPanel;
        [SerializeField] private GameObject _wordPanel;
        [SerializeField] private GameObject gameOverPanel;

        [Header("Text")]
        public TextMeshProUGUI gameOverPanelWinnerText;

        [Header("Scripts")]
        [SerializeField] private CountdownTimer _countdownTimer;
        [SerializeField] private GameSceneManager _gameSceneManager;

        private FirebaseController _firebaseController;

        private void Awake()
        {
            SetEmojiPanel();

            _firebaseController = FindObjectOfType<FirebaseController>();
        }

        private void Update()
        {
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("currentTurn").GetValueAsync().ContinueWithOnMainThread(currentTurnTask =>
            {
                int currentTurn = int.Parse(currentTurnTask.Result.Value.ToString());

                if (currentTurn == 1 && _emojiPanel.activeSelf == false) SetEmojiPanel();
                else if (currentTurn == 2 && _wordPanel.activeSelf == false) SetWordPanel();
            });
        }

        public void Switch()
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;
            reference.Child(GameData.connectedRoom).Child("opponentSwitchPanel").SetValueAsync("true");

            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("inEmojiPanel").GetValueAsync().ContinueWithOnMainThread(switchTask =>
            {
                if (switchTask.IsCompleted)
                {
                    FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("currentTurn").GetValueAsync().ContinueWithOnMainThread(currentTurnTask =>
                    {
                        int currentTurn = int.Parse(currentTurnTask.Result.Value.ToString());
                        int nextTurn = currentTurn + 1 > 2 ? 1 : 2;

                        reference.Child(GameData.connectedRoom).Child("currentTurn").SetValueAsync(nextTurn.ToString());
                    });

                    bool inEmojiPanel = switchTask.Result.Value.ToString().ToLower() == "true" ? true : false;
                    Debug.Log(inEmojiPanel);

                    GameSceneManager gameSceneManager = FindObjectOfType<GameSceneManager>();
                    if (inEmojiPanel == true)
                    {
                        gameSceneManager.SetWordPanel();
                    }
                    else
                    {
                        FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("currentRound").GetValueAsync().ContinueWithOnMainThread(currentRoundTask =>
                        {
                            if (currentRoundTask.IsCompleted)
                            {
                                int currentRound = int.Parse(currentRoundTask.Result.Value.ToString());
                                int nextRound = currentRound + 1;

                                reference.Child(GameData.connectedRoom).Child("currentRound").SetValueAsync(nextRound.ToString());

                                if (GameData.EndGanme(nextRound)) gameSceneManager.ShowGameOverPanel();
                                else gameSceneManager.SetEmojiPanel();
                            }
                        });
                    }
                }
            });
        }

        public void ShowGameOverPanel()
        {
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
                            
                            if (player1Score == player2Score) gameOverPanelWinnerText.text = "Draw";
                            else if (player1Score > player2Score) gameOverPanelWinnerText.text = "Player 1 Won";
                            else gameOverPanelWinnerText.text = "Player 2 Won";
                        }
                    });
                }
            });

            _countdownTimer.EndTimer();
            gameOverPanel.SetActive(true);
        }

        public void Replay()
        {
            gameOverPanel.SetActive(false);

            _firebaseController.ResetRoom();
            _countdownTimer.ResetTimer();

            SetDefualtPanel();
        }

        public void SetDefualtPanel()
        {
            SetEmojiPanel();
        }

        public void SetEmojiPanel()
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("activePlayer").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                if (task.IsCompleted) reference.Child(GameData.connectedRoom).Child("activePlayer").SetValueAsync("0");
            });
            reference.Child(GameData.connectedRoom).Child("inEmojiPanel").SetValueAsync("true");

            float r = Mathf.InverseLerp(0.0f, 255.0f, 255.0f);
            float g = Mathf.InverseLerp(0.0f, 255.0f, 200.0f);
            float b = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);

            _camera.backgroundColor = new Color(r, g, b, 1);

            _emojiPanel.SetActive(true);
            _wordPanel.SetActive(false);
        }
        public void SetWordPanel()
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("activePlayer").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                if (task.IsCompleted) reference.Child(GameData.connectedRoom).Child("activePlayer").SetValueAsync("1");
            });
            reference.Child(GameData.connectedRoom).Child("inEmojiPanel").SetValueAsync("false");

            float r = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);
            float g = Mathf.InverseLerp(0.0f, 255.0f, 200.0f);
            float b = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);

            _camera.backgroundColor = new Color(r, g, b, 1);

            _emojiPanel.SetActive(false);
            _wordPanel.SetActive(true);
        }
    }
}
