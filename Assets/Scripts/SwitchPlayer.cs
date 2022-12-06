using UnityEngine;
using UnityEngine.UI;

namespace EmojiJunkie
{
    public class SwitchPlayer : MonoBehaviour
    {
        private bool _isInEmojiPanel = true;

        [SerializeField] private Image _gamePanel;

        [SerializeField] private GameObject _emojiPanel;
        [SerializeField] private GameObject _wordPanel;


        private void Awake()
        {
            SetEmojiPanel();
        }

        public void Switch()
        {
            if (_isInEmojiPanel)
                SetWordPanel();
            else
                SetEmojiPanel();
        }

        private void SetEmojiPanel()
        {
            _gamePanel.color = new Color(255, 200, 0, 255);

            _emojiPanel.SetActive(true);
            _wordPanel.SetActive(false);

            _isInEmojiPanel = true;
        }

        private void SetWordPanel()
        {
            _gamePanel.color = new Color(0, 200, 0, 255);

            _emojiPanel.SetActive(false);
            _wordPanel.SetActive(true);

            _isInEmojiPanel = false;
        }
    }
}
