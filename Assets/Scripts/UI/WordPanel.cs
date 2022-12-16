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
                _countdownTimer.ResetTimer();

                GameSceneManager gameSceneManager = FindObjectOfType<GameSceneManager>();
                GameData.currentRound++;
                if (GameData.EndGanme())
                {
                    gameSceneManager.ShowGameOverPanel();
                }
                else
                {
                    Reset();
                    gameSceneManager.Switch();
                }
            }
        }
    }
}
