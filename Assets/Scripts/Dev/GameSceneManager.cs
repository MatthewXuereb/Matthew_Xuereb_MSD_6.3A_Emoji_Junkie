using EmojiJunkie.Data;
using EmojiJunkie.UI;
using Firebase.Database;
using Firebase.Extensions;
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
        [SerializeField] private GameObject _emojiPanelObject;
        [SerializeField] private GameObject _wordPanelObject;
        [SerializeField] private GameObject gameOverPanel;

        [Header("Text")]
        public TextMeshProUGUI gameOverPanelWinnerText;
        [SerializeField] private TextMeshProUGUI _currentRoundText;

        [Header("Inputs")]
        [SerializeField] private TMP_InputField[] _inputs;

        [Header("Scripts")]
        [SerializeField] private CountdownTimer _countdownTimer;
        [SerializeField] private GameSceneManager _gameSceneManager;
        [SerializeField] private EmojisPanel _emojiPanelScript;
        [SerializeField] private WordPanel _wordPanelScript;

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

                if (currentTurn == 1 && _emojiPanelObject.activeSelf == false) SetEmojiPanel();
                else if (currentTurn == 2 && _wordPanelObject.activeSelf == false) SetWordPanel();
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

                                GameData.currentRound = nextRound;
                                GameData.SetCurrentRound();

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

                            if (player1Score == player2Score)
                            {
                                gameOverPanelWinnerText.text = "Draw";
                            }
                            else if (player1Score > player2Score)
                            {
                                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("username").GetValueAsync().ContinueWithOnMainThread(playerTask =>
                                {
                                    gameOverPanelWinnerText.text = playerTask.Result.Value.ToString() + " Won";
                                });
                            }
                            else 
                            { 
                                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("username").GetValueAsync().ContinueWithOnMainThread(playerTask =>
                                {
                                    gameOverPanelWinnerText.text = playerTask.Result.Value.ToString() + " Won";
                                });
                            }
                        }
                    });
                }
            });

            _countdownTimer.EndTimer();
            gameOverPanel.SetActive(true);
        }

        public void Replay()
        {
            GameData.currentRound = 0;

            gameOverPanel.SetActive(false);

            _firebaseController.ResetRoom();
            _countdownTimer.ResetTimer();

            SetDefualtPanel();
        }

        public void SetDefualtPanel()
        {
            SetEmojiPanel();
        }

        private void SetActivePlayer(DatabaseReference reference, int activePlayer)
        {
            if (GameData.switchRoles)
            {
                if (activePlayer == 0) activePlayer = 1;
                else if (activePlayer == 1) activePlayer = 0;
            }

            GameData.currentActivePlayer = activePlayer;
            reference.Child(GameData.connectedRoom).Child("activePlayer").SetValueAsync(activePlayer.ToString());
        }

        public void SetEmojiPanel()
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("activePlayer").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;
                if (task.IsCompleted) SetActivePlayer(reference, 0);
            });
            reference.Child(GameData.connectedRoom).Child("inEmojiPanel").SetValueAsync("true");

            float r = Mathf.InverseLerp(0.0f, 255.0f, 255.0f);
            float g = Mathf.InverseLerp(0.0f, 255.0f, 200.0f);
            float b = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);

            _camera.backgroundColor = new Color(r, g, b, 1);

            _emojiPanelObject.SetActive(true);
            _wordPanelObject.SetActive(false);

            _emojiPanelScript.SetQuestion();
        }
        public void SetWordPanel()
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("activePlayer").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;
                if (task.IsCompleted) SetActivePlayer(reference, 1);
            });
            reference.Child(GameData.connectedRoom).Child("inEmojiPanel").SetValueAsync("false");

            float r = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);
            float g = Mathf.InverseLerp(0.0f, 255.0f, 200.0f);
            float b = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);

            _camera.backgroundColor = new Color(r, g, b, 1);

            _emojiPanelObject.SetActive(false);
            _wordPanelObject.SetActive(true);

            _wordPanelScript.SetQuestion();
            ToggleInput();
        }

        private void ToggleInput()
        {
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i].interactable = true;
                if (GameData.currentActivePlayer == GameData.playerId)
                    _inputs[i].interactable = false;
            }
        }
    }
}
