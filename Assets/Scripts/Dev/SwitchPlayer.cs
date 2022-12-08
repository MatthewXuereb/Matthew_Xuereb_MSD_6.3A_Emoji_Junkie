using UnityEngine;
using UnityEngine.UI;

namespace EmojiJunkie
{
    public class SwitchPlayer : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
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
