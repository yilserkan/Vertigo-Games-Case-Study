using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.ServiceManagement;
using CardGame.SpinWheel;
using DG.Tweening;
using TMPro;
using UnityEngine;
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
            ServiceLocator.Global.Get(out _levelManager);
            _levelSliderWidth = _levelSliderRect.sizeDelta.x;
            _maxLevelCount = CalculateTotalAmountOfVisibleLevels();
        }

        private void Initialize()
        {
            if (_levelSliderItems == null)
            {
                InstantiateLevels();
            }
            else
            {
                ResetLevels();
            }
            
            CheckIfReachedMaxLevel();
        }
        
        private void InstantiateLevels()
        {
            var levelDatas = _levelManager.LevelData.Levels;
            int levelCount = Mathf.Min(levelDatas.Length, _maxLevelCount);
            _levelSliderItems = new LevelSliderItem[levelCount];
            for (int i = 0; i < levelCount; i++)
            {
                var instantiated = Instantiate(_levelSliderData.LevelSliderItemPrefab, _levelParentRect);
                instantiated.SetAnchoredPosition(new Vector2(i * _levelSliderData.SingleItemWidth, 0));
                instantiated.SetLevelText((LevelType)levelDatas[i].LevelType, i);
                _levelSliderItems[i] = instantiated;
            }
        }

        private void ResetLevels()
        {
            _levelParentRect.anchoredPosition = Vector2.zero;
            var levelDatas = _levelManager.LevelData.Levels;
            for (int i = 0; i < _levelSliderItems.Length; i++)
            {
                if (levelDatas.Length <= i) { break; }
                
                _levelSliderItems[i].SetLevelText((LevelType)levelDatas[i].LevelType, i);
            }
        }

        private void AnimateToNextLevel()
        {
            var targetPos = _levelParentRect.anchoredPosition.x -_levelSliderData.SingleItemWidth;
            _levelParentRect.DOAnchorPosX(targetPos, _levelSliderData.AnimDuration).OnComplete(CheckIfNeedToReAdjustPosition);
        }

        private void AnimateBgColor()
        {
            var levelType = (LevelType)_levelManager.LevelData.Levels[_levelManager.CurrentStage].LevelType;
            var targetColor = _levelSliderData.GetLevelBgColor(levelType);
            _currentLevelBgImage.DOColor(targetColor, _levelSliderData.AnimDuration);
        }
        
        private void CheckIfNeedToReAdjustPosition()
        {
            if (_levelParentRect.anchoredPosition.x > -(_maxLevelCount/2f * _levelSliderData.SingleItemWidth) + 5 || _reachedMaxLevel ) { return; }

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
                _levelSliderItems[i].SetLevelText((LevelType)levelType, newLevel);
            }

            CheckIfReachedMaxLevel();
        }

        private void CheckIfReachedMaxLevel()
        {
            if (_levelSliderItems[^1].LevelIndex + 1 == _levelManager.LevelData.Levels.Length)
            {
                _reachedMaxLevel = true;
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AnimateToNextLevel();
            }
        }

        private void HandleOnShowNextStage(int level)
        {
            AnimateToNextLevel();
            AnimateBgColor();
        }

        private int CalculateTotalAmountOfVisibleLevels()
        {
            return (int)(_levelSliderWidth / _levelSliderData.SingleItemWidth) + 1;
        }
        private void AddListeners()
        {
            LevelManager.OnStartGame += Initialize;
            LevelManager.OnShowNextStage += HandleOnShowNextStage;
        }

        private void RemoveListeners()
        {
            LevelManager.OnStartGame -= Initialize;
            LevelManager.OnShowNextStage -= HandleOnShowNextStage;
        }
    }
}
