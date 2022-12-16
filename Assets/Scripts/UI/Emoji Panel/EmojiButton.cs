using System;
using UnityEngine;

namespace EmojiJunkie.UI
{
    public class EmojiButton : MonoBehaviour
    {
        public EmojiItem item;

        public void SetIcon()
        {
            FindObjectOfType<EmojisPanel>().SetCurrentIconImage(item);
        }
    }
}
