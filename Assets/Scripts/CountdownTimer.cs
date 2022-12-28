using EmojiJunkie.Dev;
using EmojiJunkie.Data;
using UnityEngine;
using TMPro;
using System;
using Firebase.Database;
using Firebase.Extensions;

namespace EmojiJunkie
{
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOver;

        [SerializeField] private TextMeshProUGUI _timerText;

        [Header("Timer Duration")]
        public float durationInSeconds = 0;
        [NonSerialized] public float timeLeft = 0.0f;

        private bool _stopTimer = false;

        private GameSceneManager _gameSceneManager;
        private FirebaseController _firebaseController;

        private void Awake()
        {
            _gameSceneManager = FindObjectOfType<GameSceneManager>();
            _firebaseController = FindObjectOfType<FirebaseController>();
            _firebaseController.SetReference();
        }

        private void Start()
        {
            if (GameData.playerIsHost) SetTimer();
        }

        void Update()
        {
            if (!_stopTimer)
            {
                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("timerStartTime").GetValueAsync().ContinueWithOnMainThread(startTimeTask =>
                {
                    if (startTimeTask.IsCompleted)
                    {
                        if (GameData.playerIsHost)
                            FirebaseDatabase.DefaultInstance.RootReference.Child(GameData.connectedRoom).Child("currentTime").SetValueAsync(Time.time);

                        FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("currentTime").GetValueAsync().ContinueWithOnMainThread(currentTimeTask =>
                        {
                            if (currentTimeTask.IsCompleted)
                            {
                                float startTime = float.Parse(startTimeTask.Result.Value.ToString());
                                float adjustedTime = (float.Parse(currentTimeTask.Result.Value.ToString()) - startTime);
                                timeLeft = durationInSeconds - adjustedTime;

                                float timeInMinutes = 0;
                                for (int i = 0; i < 10; i++)
                                {
                                    if (timeLeft > 60.0f)
                                    {
                                        timeInMinutes++;
                                        timeLeft -= 60.0f;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                if (adjustedTime > durationInSeconds)
                                {
                                    //
                                    FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("currentRound").GetValueAsync().ContinueWithOnMainThread(currentRoundTask =>
                                    {
                                        if (currentRoundTask.IsCompleted)
                                        {
                                            int currentRound = int.Parse(currentRoundTask.Result.Value.ToString());

                                            if (GameData.EndGanme(currentRound))
                                            {
                                                _stopTimer = true;
                                                ShowGameOverPanel();
                                            }
                                            else
                                            {
                                                ResetTimer();

                                                DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                                                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("activePlayer").GetValueAsync().ContinueWithOnMainThread(activePlayerTask =>
                                                {
                                                    if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                                                    if (activePlayerTask.IsCompleted)
                                                    {
                                                        string playerId = activePlayerTask.Result.Value.ToString();
                                                        FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child(playerId).Child("score").GetValueAsync().ContinueWithOnMainThread(scoreTask =>
                                                        {
                                                            if (scoreTask.IsCompleted)
                                                            {
                                                                if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                                                                float currentScore = float.Parse(scoreTask.Result.Value.ToString());
                                                                float newScore = currentScore + 1;

                                                                reference.Child(GameData.connectedRoom).Child(playerId).Child("score").SetValueAsync(newScore.ToString());
                                                            }
                                                        });

                                                        _gameSceneManager.Switch();
                                                    }
                                                });
                                            }
                                        }
                                    });
                                }

                                if (!_stopTimer)
                                {
                                    string minutesText = timeInMinutes < 10 ? "0" + timeInMinutes.ToString() : timeInMinutes.ToString();
                                    string secondsText = timeLeft < 10 ? "0" + Mathf.FloorToInt(timeLeft).ToString() : Mathf.FloorToInt(timeLeft).ToString();

                                    _timerText.text = "<b>Timer</b>\n" + minutesText + ":" + secondsText;
                                }
                            }
                        });
                    }
                });
            }
        }

        private void SetTimer()
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

            reference.Child(GameData.connectedRoom).Child("timerStartTime").SetValueAsync(Time.time.ToString());
        }

        public void ResetTimer()
        {
            SetTimer();
            _stopTimer = false;
        }

        public void EndTimer()
        {
            _stopTimer = true;
        }

        public void ShowGameOverPanel()
        {
            _gameOver.SetActive(true);
        }
    }
}
