using EmojiJunkie.Dev;
using EmojiJunkie.Data;
using UnityEngine;
using TMPro;
using System;

namespace EmojiJunkie
{
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOver;

        [SerializeField] private TextMeshProUGUI _timerText;

        [Header("Timer Duration")]
        public float durationInSeconds = 0;
        private float _startTime = 0.0f;
        [NonSerialized] public float timeLeft = 0.0f;

        private bool _stopTimer = false;

        private GameSceneManager _gameSceneManager;

        private void Awake()
        {
            _gameSceneManager = FindObjectOfType<GameSceneManager>();
        }

        private void Start()
        {
            _startTime = Time.time;
        }

        void Update()
        {
            if (!_stopTimer)
            {
                float adjustedTime = (Time.time - _startTime);
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
                    if (GameData.EndGanme())
                    {
                        _stopTimer = true; 
                        ShowGameOverPanel();
                    }
                    else
                    {
                        ResetTimer();

                        if (GameData.activePlayer == 0)
                        {
                            GameData.activePlayer = 1;
                            GameData.player2Score++;
                        }
                        else
                        {
                            GameData.activePlayer = 0;
                            GameData.player1Score++;
                        }

                        _gameSceneManager.Switch();
                    }
                }

                if (!_stopTimer)
                {
                    string minutesText = timeInMinutes < 10 ? "0" + timeInMinutes.ToString() : timeInMinutes.ToString();
                    string secondsText = timeLeft < 10 ? "0" + Mathf.FloorToInt(timeLeft).ToString() : Mathf.FloorToInt(timeLeft).ToString();

                    _timerText.text = "<b>Timer</b>\n" + minutesText + ":" + secondsText;
                }
            }
        }

        public void ResetTimer()
        {
            _startTime = Time.time;
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
