using EmojiJunkie.Data;
using EmojiJunkie.Dev;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace EmojiJunkie
{
    public class FirebaseController : MonoBehaviour
    {
        DatabaseReference reference;

        private HomePanelManager _homePanelManager;

        //[SerializeField] private SentenceListObject _sentenceList;

        void Awake()
        {
            SetReference();
            _homePanelManager = FindObjectOfType<HomePanelManager>();
        }

        private void Update()
        {
            if (GameData.connectedRoom != null)
            {
                FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("gameIsActive").GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                        if (task.Result.Value.ToString().ToLower() == "true")
                            _homePanelManager.LoadGameScene();
                });
            }
        }

        public void SetReference()
        {
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void CreateRoom(string roomName, string name)
        {
            FirebaseDatabase.DefaultInstance.GetReference(roomName).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Value == null)
                    {
                        User user = new User(name);
                        string json = JsonUtility.ToJson(user);

                        GameData.playerId = 0;
                        GameData.currentActivePlayer = 0;
                        reference.Child(roomName).Child("0").SetRawJsonValueAsync(json);
                        CreateData(roomName);

                        _homePanelManager.SetWaitingPanel();
                        GameData.connectedRoom = roomName;

                        GameData.playerIsHost = true;

                        /*for (int i = 0; i < GameData.numberOfRounds; i++)
                        {
                            SentenceObject sentenceObject = _sentenceList.sentences[0];
                            int length = sentenceObject.items.Length;

                            Sentences sentence = new Sentences();
                            sentence.correctWord = new string[length];
                            sentence.correctIndex = new int[length];
                            sentence.guessWord = new string[length];
                            sentence.guessIndex = new int[length];

                            for (int j = 0; j < length; j++)
                            {
                                sentence.correctWord[j] = sentenceObject.items[j].name;
                                sentence.correctIndex[j] = sentenceObject.items[j].index;

                                sentence.guessWord[j] = "null";
                                sentence.guessIndex[j] = -1;
                            }

                            string sentenceJson = JsonUtility.ToJson(sentence);
                            reference.Child(roomName).Child("Sentences").Child("Round_" + (i + 1).ToString()).SetRawJsonValueAsync(sentenceJson);
                        }*/
                    }
                    else
                    {
                        Debug.LogError("Room name taken!");
                    }
                }
            });
        }
        public void JoinRoom(string roomName, string name)
        {
            FirebaseDatabase.DefaultInstance.GetReference(roomName).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Value == null)
                    {
                        User user = new User(name);
                        string json = JsonUtility.ToJson(user);

                        GameData.playerId = 0;
                        GameData.currentActivePlayer = 0;
                        reference.Child(roomName).Child("0").SetRawJsonValueAsync(json);
                        CreateData(roomName);

                        _homePanelManager.SetWaitingPanel();
                        GameData.connectedRoom = roomName;

                        GameData.playerIsHost = true;
                    }
                    else
                    {
                        User user = new User(name);
                        string json = JsonUtility.ToJson(user);

                        GameData.playerId = 1;
                        GameData.currentActivePlayer = 0;
                        reference.Child(roomName).Child("1").SetRawJsonValueAsync(json);

                        CreateData(roomName);
                        reference.Child(roomName).Child("activePlayer").SetRawJsonValueAsync("0");
                        reference.Child(roomName).Child("gameIsActive").SetValueAsync("true");
                        reference.Child(roomName).Child("timerStartTime").SetRawJsonValueAsync(Time.time.ToString());

                        _homePanelManager.JoinRoom();
                        GameData.connectedRoom = roomName;

                        GameData.playerIsHost = false;
                    }
                }
            });
        }

        private void CreateData(string roomName)
        {
            reference.Child(roomName).Child("gameIsActive").SetRawJsonValueAsync("false");

            reference.Child(roomName).Child("timerStartTime").SetRawJsonValueAsync("0");
            reference.Child(roomName).Child("currentTime").SetRawJsonValueAsync("0");

            reference.Child(roomName).Child("inEmojiPanel").SetRawJsonValueAsync("true");
            reference.Child(roomName).Child("opponentSwitchPanel").SetRawJsonValueAsync("true");

            reference.Child(roomName).Child("currentRound").SetRawJsonValueAsync("1");
            reference.Child(roomName).Child("currentTurn").SetRawJsonValueAsync("1");
            //reference.Child(roomName).Child("currentSelectedIcon").SetRawJsonValueAsync("-1");

            reference.Child(roomName).Child("endGame").SetRawJsonValueAsync("false");
        }

        public void ResetRoom()
        {
            if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

            reference.Child(GameData.connectedRoom).Child("0").Child("score").SetValueAsync("0");
            reference.Child(GameData.connectedRoom).Child("1").Child("score").SetValueAsync("0");

            reference.Child(GameData.connectedRoom).Child("gameIsActive").SetValueAsync("false");
            reference.Child(GameData.connectedRoom).Child("activePlayer").SetValueAsync("0");

            reference.Child(GameData.connectedRoom).Child("timerStartTime").SetValueAsync("0");
            reference.Child(GameData.connectedRoom).Child("currentTime").SetValueAsync("0");

            reference.Child(GameData.connectedRoom).Child("inEmojiPanel").SetValueAsync("true");
            reference.Child(GameData.connectedRoom).Child("opponentSwitchPanel").SetValueAsync("true");

            reference.Child(GameData.connectedRoom).Child("currentRound").SetValueAsync("1");
            reference.Child(GameData.connectedRoom).Child("currentTurn").SetValueAsync("1");
        }
    }

    public class User
    {
        public string username;
        public string score = "0";

        public User(string username)
        {
            this.username = username;
        }
    }

    /*public class Sentences
    {
        public string[] correctWord;
        public string[] guessWord;

        public int[] correctIndex;
        public int[] guessIndex;
    }*/
}
