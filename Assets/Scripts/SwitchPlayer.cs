using UnityEngine;
using UnityEngine.UI;

namespace EmojiJunkie
{
    public class SwitchPlayer : MonoBehaviour
    {
        [SerializeField] private Image _gamePanel;

        [SerializeField] private GameObject _emojiPanel;
        [SerializeField] private GameObject _wordPanel;

        private void Awake()
        {
            SetEmojiPanel();
        }

        public void Switch()
        {
            if (_emojiPanel.activeSelf)
                SetWordPanel();
            else
                SetEmojiPanel();
        }

        private void SetEmojiPanel()
        {
            _gamePanel.color = new Color(255, 200, 0, 255);

            _emojiPanel.SetActive(true);
            _wordPanel.SetActive(false);
        }
        private void SetWordPanel()
        {
            _gamePanel.color = new Color(0, 200, 0, 255);

            _emojiPanel.SetActive(false);
            _wordPanel.SetActive(true);
        }
    }
}
