using CardGame.SpinWheel;
using TMPro;
using UnityEngine;

namespace CardGame.LevelSlider
{
    public class LevelSliderItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private LevelSliderItemData _sliderItemData;
        
        private int _levelIndex;
        public int LevelIndex => _levelIndex;
        
        public void SetAnchoredPosition(Vector2 anchoredPos)
        {
            _rect.anchoredPosition = anchoredPos;
        }
        
        public void SetLevelText(LevelType levelType, int levelIndex, int currentLevel)
        {
            _levelIndex = levelIndex;
            _levelText.color = _sliderItemData.GetTextColor(levelType);;
            _levelText.text = $"{levelIndex + 1}";
            
            UpdateTextAlpha(levelIndex, currentLevel);
        }

        public void UpdateTextAlpha(int levelIndex, int currentLevel)
        {
            var levelCompleted = levelIndex < currentLevel;
            var col = _levelText.color;
            col.a = _sliderItemData.GetTextAlpha(levelCompleted);
            _levelText.color = col;
        }
    }
}