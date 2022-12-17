using EmojiJunkie.Data;
using EmojiJunkie.Dev;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EmojiJunkie.UI
{
    public class WordPanel : MonoBehaviour
    {
        private CountdownTimer _countdownTimer;

        public TMP_InputField[] inputs = new TMP_InputField[5];
        private List<bool> _wordsCorrect = new List<bool>();

        [SerializeField] private EmojiItem[] _emojis;

        private void Start()
        {
            _countdownTimer = FindObjectOfType<CountdownTimer>();

            for (int i = 0; i < inputs.Length; i++)
                _wordsCorrect.Add(false);
        }

        public void Reset()
        {
            _countdownTimer.ResetTimer();

            for (int i = 0; i < _emojis.Length; i++)
            {
                inputs[i].text = "";
                _wordsCorrect[i] = false;
            }
        }

        public void CheckInput(int i)
        {
            if (inputs[i].text.ToLower().Equals(_emojis[i].name.ToLower()))
                _wordsCorrect[i] = true;

            bool allCorrect = true;
            for (int j = 0; j < _wordsCorrect.Count; j++)
                if (!_wordsCorrect[j])
                    allCorrect = false;

            if (allCorrect)
            {
                float scoreToAdd = 3 * Mathf.InverseLerp(0.0f, _countdownTimer.durationInSeconds, _countdownTimer.timeLeft);
                GameData.player1Score += scoreToAdd;

                scoreToAdd = 2 * Mathf.InverseLerp(0.0f, _countdownTimer.durationInSeconds, _countdownTimer.timeLeft);
                GameData.player2Score += scoreToAdd;

                Reset();
                FindObjectOfType<GameSceneManager>().Switch();
            }
        }
    }
}
