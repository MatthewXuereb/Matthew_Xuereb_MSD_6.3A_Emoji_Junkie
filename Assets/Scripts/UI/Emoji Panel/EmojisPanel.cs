using EmojiJunkie.Data;
using EmojiJunkie.Dev;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EmojiJunkie.UI
{
    public class EmojisPanel : MonoBehaviour
    {
        [SerializeField] private Transform _parent;

        [SerializeField] private GameObject _emojisPanel;
        [SerializeField] private GameObject _emojiButton;

        [SerializeField] private List<GameObject> _icons = new List<GameObject>();

        [SerializeField] private EmojiList _emojiList;
        [SerializeField] private SentenceListObject _sentenceList;
        [SerializeField] private EmojiItem[] _emojis;

        [SerializeField] private TextMeshProUGUI _question;
        [SerializeField] private TextMeshProUGUI _difficultyText;

        private CountdownTimer _countdownTimer;

        private int _currentIcon = 0;

        private List<bool> _emojisCorrect = new List<bool>();

        //private FirebaseDatabase _database;
        //private DatabaseReference _scoreRef;
        private DatabaseReference _reference;

        private void Start()
        {
            //_database = FirebaseDatabase.DefaultInstance;
            _reference = FirebaseDatabase.DefaultInstance.RootReference;

            /*_scoreRef = FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("score");
            _scoreRef.ValueChanged += HandleValueChange;*/

            _countdownTimer = FindObjectOfType<CountdownTimer>();

            for (int i = 0; i < _emojis.Length; i++)
                _emojisCorrect.Add(false);
        }

        /*private void OnDestroy()
        {
            _scoreRef.ValueChanged -= HandleValueChange;
            _scoreRef = null;
        }*/

        public void Reset()
        {
            GameData.currentRound = 0;

            for (int i = 0; i < _emojis.Length; i++)
            {
                _emojis[i] = null;
                _icons[i].GetComponent<Image>().sprite = null;
            }
        }

        /*private void Update()
        {
            for (int i = 0; i < 5; i++)
            {
                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("Sentences").Child("Round_1").Child("guessIndex").Child(i.ToString()).GetValueAsync().ContinueWithOnMainThread(guessTask =>
                {
                    if (guessTask.IsCompleted)
                    {
                        int index = int.Parse(guessTask.Result.Value.ToString());
                        if (index != -1)
                        {
                            _icons[i].GetComponent<Image>().sprite = _emojiList.emojis[index].sprite;
                        }
                    }
                });
            }
        }*/

        public void SetQuestion()
        {
            string s1 = _sentenceList.sentences[GameData.currentRoundIndex].items[0].sprite.name;
            string s2 = _sentenceList.sentences[GameData.currentRoundIndex].items[1].sprite.name;
            string s3 = _sentenceList.sentences[GameData.currentRoundIndex].items[2].sprite.name;

            _question.text = s1 + " " + s2 + " " + s3;
            
            _difficultyText.text = _sentenceList.sentences[GameData.currentRoundIndex].difficulty;
        }

        public void Open()
        {
            if (GameData.currentActivePlayer == GameData.playerId) _emojisPanel.SetActive(true);

            EmojiButton[] buttons = FindObjectsOfType<EmojiButton>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Image>().sprite = buttons[i].item.sprite;
                //_icons.Add(buttons[i].gameObject);
            }
        }

        public void Close()
        {
            _emojisPanel.SetActive(false);
        }

        public void SetCurrentIconIndex(int i)
        {
            if (GameData.currentActivePlayer == GameData.playerId) 
            {
                _currentIcon = i;

                GameData.currentSelectedIcon = i;

                /*DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                reference.Child(GameData.connectedRoom).Child("currentSelectedIcon").SetValueAsync(i.ToString());*/
            }
        }

        public void SetCurrentIconImage(EmojiItem item)
        {
            if (GameData.currentActivePlayer == GameData.playerId)
            {
                _icons[_currentIcon].GetComponent<Image>().sprite = item.sprite;
                _emojis[_currentIcon] = item;

                bool allCorrect = true;
                for (int i = 0; i < _emojis.Length; i++)
                {
                    if (_emojis[i] != null)
                        _emojisCorrect[i] = _sentenceList.sentences[GameData.currentRoundIndex].items[i].name.Equals(_emojis[i].name);
                    else
                        _emojisCorrect[i] = false;

                    if (!_emojisCorrect[i])
                        allCorrect = false;
                }

                if (allCorrect)
                {
                    FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("score").GetValueAsync().ContinueWithOnMainThread(task =>
                    {
                        if (task.IsCompleted)
                        {
                            if (_reference == null) _reference = FirebaseDatabase.DefaultInstance.RootReference;

                            float currentScore = float.Parse(task.Result.Value.ToString());
                            float newScore = currentScore + 1;

                            if (GameData.switchRoles) _reference.Child(GameData.connectedRoom).Child("1").Child("score").SetValueAsync(newScore.ToString());
                            else _reference.Child(GameData.connectedRoom).Child("0").Child("score").SetValueAsync(newScore.ToString());
                        }
                    });

                    _countdownTimer.ResetTimer();
                    FindObjectOfType<GameSceneManager>().Switch();

                    Reset();
                }

                Close();
            }
        }

        /*private void HandleValueChange(object sender, ValueChangedEventArgs e)
        {
            string json = e.Snapshot.GetRawJsonValue();
            Debug.Log(json);
        }*/
    }
}
