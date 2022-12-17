using EmojiJunkie.Data;
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

        private void Awake()
        {
            SetEmojiPanel();
        }

        public void Switch()
        {
            if (_emojiPanel.activeSelf)
            {
                SetWordPanel();
            }
            else
            {
                GameData.currentRound++;

                if (GameData.EndGanme())
                    ShowGameOverPanel();
                else
                    SetEmojiPanel();
            }
        }

        public void ShowGameOverPanel()
        {
            if (GameData.player1Score == GameData.player2Score)
                gameOverPanelWinnerText.text = "Draw";
            else if (GameData.player1Score > GameData.player2Score)
                gameOverPanelWinnerText.text = "Player 1 Won";
            else
                gameOverPanelWinnerText.text = "Player 2 Won";

            _countdownTimer.EndTimer();
            gameOverPanel.SetActive(true);
        }

        public void Replay()
        {
            gameOverPanel.SetActive(false);

            GameData.ResetGame();
            _countdownTimer.ResetTimer();

            SetDefualtPanel();
        }

        public void SetDefualtPanel()
        {
            SetEmojiPanel();
        }

        private void SetEmojiPanel()
        {
            GameData.activePlayer = 0;

            float r = Mathf.InverseLerp(0.0f, 255.0f, 255.0f);
            float g = Mathf.InverseLerp(0.0f, 255.0f, 200.0f);
            float b = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);

            _camera.backgroundColor = new Color(r, g, b, 1);

            _emojiPanel.SetActive(true);
            _wordPanel.SetActive(false);
        }
        private void SetWordPanel()
        {
            GameData.activePlayer = 1;

            float r = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);
            float g = Mathf.InverseLerp(0.0f, 255.0f, 200.0f);
            float b = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);

            _camera.backgroundColor = new Color(r, g, b, 1);

            _emojiPanel.SetActive(false);
            _wordPanel.SetActive(true);
        }
    }
}
