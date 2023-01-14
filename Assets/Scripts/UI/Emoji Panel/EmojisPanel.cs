using EmojiJunkie.Data;
using EmojiJunkie.Dev;
using Firebase.Database;
using Firebase.Extensions;
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

        [SerializeField] private SentenceObject _sentence;
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
            if (GameData.currentActivePlayer == GameData.playerId) _panel.SetActive(true);
        }

        public void Close()
        {
            _panel.SetActive(false);
        }

        public void SetCurrentIconIndex(int i)
        {
            if (GameData.currentActivePlayer == GameData.playerId) _currentIcon = i;
        }

        public void SetCurrentIconImage(EmojiItem item)
        {
            if (GameData.currentActivePlayer == GameData.playerId)
            {
                _icons[_currentIcon].GetComponent<Image>().sprite = item.sprite;
                _emojis[_currentIcon] = item;

                bool allCorrect = true;
                for (int i = 0; i < _sentence.items.Length; i++)
                {
                    if (_emojis[i] != null)
                        _emojisCorrect[i] = _sentence.items[i].name.Equals(_emojis[i].name);
                    else
                        _emojisCorrect[i] = false;

                    if (!_emojisCorrect[i])
                        allCorrect = false;
                }

                if (allCorrect)
                {
                    DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                    FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("score").GetValueAsync().ContinueWithOnMainThread(task =>
                    {
                        if (task.IsCompleted)
                        {
                            if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                            float currentScore = float.Parse(task.Result.Value.ToString());
                            float newScore = currentScore + 1;

                            if (GameData.switchRoles) reference.Child(GameData.connectedRoom).Child("1").Child("score").SetValueAsync(newScore.ToString());
                            else reference.Child(GameData.connectedRoom).Child("0").Child("score").SetValueAsync(newScore.ToString());
                        }
                    });

                    _countdownTimer.ResetTimer();
                    FindObjectOfType<GameSceneManager>().Switch();

                    Reset();
                }

                Close();
            }
        }
    }
}
