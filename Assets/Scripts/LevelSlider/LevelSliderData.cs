using CardGame.SpinWheel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CardGame.LevelSlider
{
    [CreateAssetMenu(menuName = "ScriptableObjects/LevelSlider/Data", fileName = "LevelSliderData", order = 0)]
    public class LevelSliderData : ScriptableObject
    {
        [SerializeField] private AssetReferenceGameObject _levelSliderItemPrefab;
        [SerializeField] private float _singleItemWidth;
        [SerializeField] private float _animDuration;

        [SerializeField] private Color _defaultLevelBgColor;
        [SerializeField] private Color _safeZoneLevelBgColor;
        [SerializeField] private Color _superZoneLevelBgColor;

        public AssetReferenceGameObject LevelSliderItemPrefab => _levelSliderItemPrefab;
        public float SingleItemWidth => _singleItemWidth;
        public float AnimDuration => _animDuration;
        
        public Color GetLevelBgColor(LevelType levelType)
        {
            if (levelType == LevelType.Default)
            {
                return _defaultLevelBgColor;
            }

            if (levelType == LevelType.SafeZone)
            {
                return _safeZoneLevelBgColor;
            }

            return _superZoneLevelBgColor;
        }

    }
}