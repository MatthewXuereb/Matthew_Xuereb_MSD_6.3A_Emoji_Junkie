using EmojiJunkie.Data;
using Firebase.Database;
using UnityEngine;

namespace EmojiJunkie.UI
{
    public class EmojiButton : MonoBehaviour
    {
        public EmojiItem item;

        public void SetIcon()
        {
            FindObjectOfType<EmojisPanel>().SetCurrentIconImage(item);

            //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            //reference.Child(GameData.connectedRoom).Child("Sentences").Child("Round_1").Child("guessIndex").Child(GameData.currentSelectedIcon.ToString()).SetValueAsync(item.index.ToString());
        }
    }
}
