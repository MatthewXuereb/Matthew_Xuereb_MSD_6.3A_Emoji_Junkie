using UnityEngine;

namespace EmojiJunkie
{
    [CreateAssetMenu(fileName = "Sentence List Object", menuName = "Objects/Sentence List Object")]
    public class SentenceListObject : ScriptableObject
    {
        public SentenceObject[] sentences;
    }
}
