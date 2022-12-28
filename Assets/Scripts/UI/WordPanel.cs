using EmojiJunkie.Data;
using EmojiJunkie.Dev;
using Firebase.Database;
using Firebase.Extensions;
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
                DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("0").Child("score").GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                        float currentScore = float.Parse(task.Result.Value.ToString());
                        float newScore = currentScore + (3.0f * Mathf.InverseLerp(0.0f, _countdownTimer.durationInSeconds, _countdownTimer.timeLeft));

                        reference.Child(GameData.connectedRoom).Child("0").Child("score").SetValueAsync(newScore.ToString());
                    }
                });
                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("1").Child("score").GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                        float currentScore = float.Parse(task.Result.Value.ToString());
                        float newScore = currentScore + (2.0f * Mathf.InverseLerp(0.0f, _countdownTimer.durationInSeconds, _countdownTimer.timeLeft));

                        reference.Child(GameData.connectedRoom).Child("1").Child("score").SetValueAsync(newScore.ToString());
                    }
                });

                Reset();
                FindObjectOfType<GameSceneManager>().Switch();
            }
        }
    }
}
