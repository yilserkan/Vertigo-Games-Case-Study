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
        
        public void SetLevelText(LevelType levelType, int levelIndex)
        {
            _levelIndex = levelIndex;
            _levelText.color = _sliderItemData.GetTextColor(levelType);
            _levelText.text = $"{levelIndex + 1}";
        }
    }
}