using EmojiJunkie.Data;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

namespace EmojiJunkie
{
    public class Status : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statusText;

        private FirebaseController _firebaseController;

        private void Awake()
        {
            _firebaseController = FindObjectOfType<FirebaseController>();
        }

        void Update()
        {
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            FirebaseDatabase.DefaultInstance.GetReference(GameData.connectedRoom).Child("activePlayer").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (reference == null) reference = FirebaseDatabase.DefaultInstance.RootReference;

                if (task.IsFaulted)
                {
                    //
                }
                else if (task.IsCompleted)
                {
                    int activePlayer = int.Parse(task.Result.Value.ToString());
                    
                    if (activePlayer == 0) _statusText.text = "Player 1 Turn";
                    else _statusText.text = "Player 2 Turn";
                }
            });
        }
    }
}
