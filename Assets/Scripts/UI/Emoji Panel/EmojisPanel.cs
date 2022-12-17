using EmojiJunkie.Data;
using EmojiJunkie.Dev;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EmojiJunkie.UI
{
    public class EmojisPanel : MonoBehaviour
    {
        [SerializeField] private Transform _parent;

        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _emojiButton;

        [SerializeField] private GameObject[] _icons;

        [SerializeField] private EmojiItem[] _sentences;
        [SerializeField] private EmojiItem[] _emojis;

        private CountdownTimer _countdownTimer;

        private int _currentIcon = 0;

        private List<bool> _emojisCorrect = new List<bool>();

        void Start()
        {
            _countdownTimer = FindObjectOfType<CountdownTimer>();

            for (int i = 0; i < _icons.Length; i++)
                _emojisCorrect.Add(false);
        }

        public void Reset()
        {
            for (int i = 0; i < _emojis.Length; i++)
            {
                _emojis[i] = null;
                _icons[i].GetComponent<Image>().sprite = null;
            }
        }

        public void Open()
        {
            _panel.SetActive(true);
        }

        public void Close()
        {
            _panel.SetActive(false);
        }

        public void SetCurrentIconIndex(int i)
        {
            _currentIcon = i;
        }

        public void SetCurrentIconImage(EmojiItem item)
        {
            _icons[_currentIcon].GetComponent<Image>().sprite = item.sprite;
            _emojis[_currentIcon] = item;

            bool allCorrect = true;
            for (int i = 0; i < _emojis.Length; i++)
            {
                if (_emojis[i] != null)
                    _emojisCorrect[i] = _sentences[i].name.Equals(_emojis[i].name);
                else
                    _emojisCorrect[i] = false;

                if (!_emojisCorrect[i])
                    allCorrect = false;
            }

            if (allCorrect)
            {
                GameData.player1Score++;

                _countdownTimer.ResetTimer();
                FindObjectOfType<GameSceneManager>().Switch();

                Reset();
            }

            Close();
        }
    }
}
