using UnityEngine;

namespace CardGame.SpinWheel
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SpinWheelUIAssets", fileName = "SpinWheelUIAssets", order = 0)]
    public class SpinWheelUIAssets : ScriptableObject
    {
        [SerializeField] private Sprite[] _spinWheelBodySpriteArray;
        [SerializeField] private Sprite[] _spinWheelIndicatorSpriteArray;

        public bool TryGetWheelBodySprite(LevelType levelType, out Sprite sprite)
        {
            int levelTypeIndex = (int)levelType;
            if (_spinWheelBodySpriteArray.Length <= levelTypeIndex)
            {
                sprite = null;
                return false;
            }

            sprite = _spinWheelBodySpriteArray[levelTypeIndex];
            return true;
        }
        
        public bool TryGetWheelIndicatorSprite(LevelType levelType, out Sprite sprite)
        {
            int levelTypeIndex = (int)levelType;
            if (_spinWheelIndicatorSpriteArray.Length <= levelTypeIndex)
            {
                sprite = null;
                return false;
            }

            sprite = _spinWheelIndicatorSpriteArray[levelTypeIndex];
            return true;
        }

    }
}