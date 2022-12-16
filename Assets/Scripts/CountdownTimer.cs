using EmojiJunkie.Dev;
using EmojiJunkie.Data;
using UnityEngine;
using TMPro;

namespace EmojiJunkie
{
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOver;

        [SerializeField] private TextMeshProUGUI _timerText;

        [Header("Timer Duration")]
        [SerializeField] private float _durationInSeconds = 0;
        private float _startTime = 0.0f;

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
                float currentTime = _durationInSeconds - adjustedTime;

                float timeInMinutes = 0;
                for (int i = 0; i < 10; i++)
                {
                    if (currentTime > 60.0f)
                    {
                        timeInMinutes++;
                        currentTime -= 60.0f;
                    }
                    else
                    {
                        break;
                    }
                }

                if (adjustedTime > _durationInSeconds)
                {
                    if (GameData.currentTurn >= GameData.numberOfTurnsInRound)
                    {
                        GameData.currentRound++;
                        GameData.currentTurn = 1;
                    }
                    else
                    {
                        GameData.currentTurn++;
                    }

                    if (GameData.EndGanme())
                    {
                        _stopTimer = true; 
                        ShowGameOverPanel();
                    }
                    else
                    {
                        ResetTimer();
                        _gameSceneManager.Switch();
                    }
                }

                if (!_stopTimer)
                {
                    string minutesText = timeInMinutes < 10 ? "0" + timeInMinutes.ToString() : timeInMinutes.ToString();
                    string secondsText = currentTime < 10 ? "0" + Mathf.FloorToInt(currentTime).ToString() : Mathf.FloorToInt(currentTime).ToString();

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
