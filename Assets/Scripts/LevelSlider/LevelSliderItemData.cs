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

        [SerializeField][Range(0f, 1f)] private float _defaultTextAlpha;
        [SerializeField][Range(0f, 1f)] private float _levelCompletedTextAlpha;
        
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

        public float GetTextAlpha(bool levelCompleted)
        {
            return levelCompleted ? _levelCompletedTextAlpha : _defaultTextAlpha;
        }
        
    }
}