using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.SpinWheel;
using CardGame.Utils;
using UnityEngine;

public class ZonePanelManager : MonoBehaviour
{
    [SerializeField] private ZonePanelUIManager _zonePanelUIManager;
    [SerializeField] private ZoneCardManager _safeZoneCardManager;
    [SerializeField] private ZoneCardManager _superZoneCardManager;
    
    private Observable<int> _safeZoneLevel;
    private Observable<int> _superZoneLevel;
    private GetLevelResponse _levelData;
    
    private void OnEnable()
    {
        AddListeners();
    }


    private void OnDisable()
    {
        RemoveListeners();
    }

    public void Initialize(GetLevelResponse levelData)
    {
        _safeZoneLevel = new Observable<int>(0, _safeZoneCardManager);
        _superZoneLevel = new Observable<int>(0, _superZoneCardManager);
        _levelData = levelData;
        SetNextZoneLevel(LevelType.SafeZone, 0);
        SetNextZoneLevel(LevelType.SuperZone, 0);
    }

    private void HandleOnShowNextStage(int level)
    {
        LevelType currentLevelType = (LevelType)_levelData.LevelData[level].LevelType;
        if (currentLevelType is LevelType.SafeZone or LevelType.SuperZone)
        {
            _zonePanelUIManager.ShowZonePanel(currentLevelType);
            SetNextZoneLevel(currentLevelType, level);
        }
    }

    private void SetNextZoneLevel(LevelType type, int currentLevel)
    {
        var nextZoneLevel = FindNextZoneLevel(type, currentLevel);
        if (type == LevelType.SafeZone)
        {
            _safeZoneLevel.Value = nextZoneLevel;
        }
        else
        {
            _superZoneLevel.Value = nextZoneLevel;
        }
    }

    private int FindNextZoneLevel(LevelType type, int currentLevel)
    {
        for (int i = currentLevel + 1; i < _levelData.LevelData.Length; i++)
        {
            if (_levelData.LevelData[i].LevelType == (int)type)
            {
                return i + 1;
            }
        }

        return -1;
    }
    
    private void AddListeners()
    {
        LevelManager.OnShowNextStage += HandleOnShowNextStage;
    }

    private void RemoveListeners()
    {
        LevelManager.OnShowNextStage -= HandleOnShowNextStage;
    }
}
