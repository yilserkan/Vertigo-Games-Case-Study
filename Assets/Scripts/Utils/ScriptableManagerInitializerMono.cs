using UnityEngine;

namespace CardGame.Utils
{
    public class ScriptableManagerInitializerMono : MonoBehaviour
    {
        [SerializeField] private AbstractScriptableManagerBase[] _abstractScriptableManagerArray;

        private void Awake()
        {
            for (int i = 0; i < _abstractScriptableManagerArray.Length; i++)
            {
                _abstractScriptableManagerArray[i].Initialize();
            }
        }
    }
}