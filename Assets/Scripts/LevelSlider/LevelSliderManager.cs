using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Extensions;
using CardGame.ServiceManagement;
using CardGame.SpinWheel;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace CardGame.LevelSlider
{
    public class LevelSliderManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _levelSliderRect;
        [SerializeField] private RectTransform _levelParentRect;
        [SerializeField] private Image _currentLevelBgImage;
        [SerializeField] private LevelSliderData _levelSliderData;
        
        private LevelManager _levelManager;
        private float _levelSliderWidth;
        private int _maxLevelCount;
        private bool _reachedMaxLevel;

        private LevelSliderItem[] _levelSliderItems;
        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void Start()
        {
            ServiceLocator.ForScene(this).Get(out _levelManager);
            Initialize();
        }

        private void Initialize()
        {
            _levelSliderWidth = _levelSliderRect.sizeDelta.x;
            _maxLevelCount = CalculateTotalAmountOfVisibleLevels();
        }

        private async void SetLevels()
        {
            if (_levelSliderItems == null)
            {
                await InstantiateLevels();
            }
            else
            {
                ResetLevels();
            }
            
            CheckIfReachedMaxLevel();
            AnimateBgColor(0);
        }
        
        private async Task InstantiateLevels()
        {
            var levelDatas = _levelManager.LevelData.Levels;
            int levelCount = Mathf.Min(levelDatas.Length, _maxLevelCount);
            _levelSliderItems = new LevelSliderItem[levelCount];
            for (int i = 0; i < levelCount; i++)
            {
                var level = _levelManager.CurrentStage + i;
                var instantiated = await _levelSliderData.LevelSliderItemPrefab.InstantiateAddressableAsync(_levelParentRect);
                if (instantiated != null && instantiated.TryGetComponent(out LevelSliderItem levelItem))
                {
                    levelItem.SetAnchoredPosition(new Vector2(i * _levelSliderData.SingleItemWidth, 0));
                    levelItem.SetLevelText((LevelType)levelDatas[level].LevelType, level, _levelManager.CurrentStage);
                    _levelSliderItems[i] = levelItem;
                }
            }
        }

        private void ResetLevels()
        {
            _levelParentRect.anchoredPosition = Vector2.zero;
            var levelDatas = _levelManager.LevelData.Levels;
            for (int i = 0; i < _levelSliderItems.Length; i++)
            {
                var level = _levelManager.CurrentStage + i;
                if (levelDatas.Length <= level) { break; }
                
                _levelSliderItems[i].SetLevelText((LevelType)levelDatas[level].LevelType, level, _levelManager.CurrentStage);
            }
        }
        
        private void HandleOnShowNextStage(int level)
        {
            AnimateToNextLevel();
            AnimateBgColor(_levelSliderData.AnimDuration);
        }
        
        private void AnimateToNextLevel()
        {
            var targetPos = _levelParentRect.anchoredPosition.x -_levelSliderData.SingleItemWidth;
            _levelParentRect.DOAnchorPosX(targetPos, _levelSliderData.AnimDuration).OnComplete(CheckIfNeedToReAdjustPosition);
        }

        private void AnimateBgColor(float duration)
        {
            var levelType = (LevelType)_levelManager.LevelData.Levels[_levelManager.CurrentStage].LevelType;
            var targetColor = _levelSliderData.GetLevelBgColor(levelType);
            _currentLevelBgImage.DOColor(targetColor, duration);
        }
        
        private void CheckIfNeedToReAdjustPosition()
        {
            if (_levelParentRect.anchoredPosition.x > -(_maxLevelCount / 2f * _levelSliderData.SingleItemWidth) + 5 || _reachedMaxLevel)
            {
                UpdateLevelTextAlphas();
                return;
            }

            var newAnchoredPosition = _levelParentRect.anchoredPosition;
            newAnchoredPosition.x += _levelSliderData.SingleItemWidth;
            _levelParentRect.anchoredPosition = newAnchoredPosition;
            UpdateLevelTexts();
        }

        private void UpdateLevelTexts()
        {
            for (int i = 0; i < _levelSliderItems.Length; i++)
            {
                var newLevel = _levelSliderItems[i].LevelIndex + 1;
                var levelType = _levelManager.LevelData.Levels[newLevel].LevelType;
                _levelSliderItems[i].SetLevelText((LevelType)levelType, newLevel, _levelManager.CurrentStage);
            }

            CheckIfReachedMaxLevel();
        }

        private void UpdateLevelTextAlphas()
        {
            for (int i = 0; i < _levelSliderItems.Length; i++)
            {
                var newLevel = _levelSliderItems[i].LevelIndex;
                _levelSliderItems[i].UpdateTextAlpha(newLevel, _levelManager.CurrentStage);
            }
        }
        
        private void CheckIfReachedMaxLevel()
        {
            if (_levelSliderItems[^1].LevelIndex + 1 == _levelManager.LevelData.Levels.Length)
            {
                _reachedMaxLevel = true;
            }
        }
        
        private int CalculateTotalAmountOfVisibleLevels()
        {
            return (int)(_levelSliderWidth / _levelSliderData.SingleItemWidth) + 1;
        }

        public void ReleaseLevels()
        {
            for (int i = _levelSliderItems.Length - 1; i >= 0; i--)
            {
                Addressables.ReleaseInstance(_levelSliderItems[i].gameObject);
            }

            _levelSliderItems = null;
        }
        
        private void AddListeners()
        {
            LevelManager.OnStartGame += SetLevels;
            LevelManager.OnShowNextStage += HandleOnShowNextStage;
            LevelManager.OnQuitGame += ReleaseLevels;
        }

        private void RemoveListeners()
        {
            LevelManager.OnStartGame -= SetLevels;
            LevelManager.OnShowNextStage -= HandleOnShowNextStage;
            LevelManager.OnQuitGame -= ReleaseLevels;
        }
    }
}
