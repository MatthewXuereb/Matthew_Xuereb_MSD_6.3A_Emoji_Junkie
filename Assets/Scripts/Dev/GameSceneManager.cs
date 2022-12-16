using EmojiJunkie.Data;
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
        [SerializeField] private GameObject _gameOverPanel;

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
            _gameOverPanel.SetActive(true);
        }

        public void Replay()
        {
            _gameOverPanel.SetActive(false);

            GameData.ResetGame();
            
            FindObjectOfType<CountdownTimer>().ResetTimer();
            FindObjectOfType<GameSceneManager>().Switch();
        }

        public void SetDefualtPanel()
        {
            SetEmojiPanel();
        }

        private void SetEmojiPanel()
        {
            float r = Mathf.InverseLerp(0.0f, 255.0f, 255.0f);
            float g = Mathf.InverseLerp(0.0f, 255.0f, 200.0f);
            float b = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);

            _camera.backgroundColor = new Color(r, g, b, 1);

            _emojiPanel.SetActive(true);
            _wordPanel.SetActive(false);
        }
        private void SetWordPanel()
        {
            float r = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);
            float g = Mathf.InverseLerp(0.0f, 255.0f, 200.0f);
            float b = Mathf.InverseLerp(0.0f, 255.0f, 0.0f);

            _camera.backgroundColor = new Color(r, g, b, 1);

            _emojiPanel.SetActive(false);
            _wordPanel.SetActive(true);
        }
    }
}
