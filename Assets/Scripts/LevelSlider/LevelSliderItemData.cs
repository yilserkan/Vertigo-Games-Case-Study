using CardGame.SpinWheel;
using UnityEngine;

namespace CardGame.LevelSlider
{
    
    [CreateAssetMenu(menuName = "ScriptableObjects/LevelSlider/ItemData", fileName = "LevelSliderItemData", order = 0)]
    public class LevelSliderItemData : ScriptableObject
    {
        [SerializeField] private Color _defualtTextColor;
        [SerializeField] private Color _safeZoneTextColor;
        [SerializeField] private Color _superZoneTextColor;
        
        public Color GetTextColor(LevelType levelType)
        {
            if (levelType == LevelType.Default)
            {
                return _defualtTextColor;
            }

            if (levelType == LevelType.SafeZone)
            {
                return _safeZoneTextColor;
            }

            return _superZoneTextColor;
        }
        
    }
}