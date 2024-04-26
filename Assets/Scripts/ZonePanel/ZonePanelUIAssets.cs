using CardGame.SpinWheel;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ZonePanelUIAssets", fileName = "ZonePanelUIAssets", order = 0)]
public class ZonePanelUIAssets : ScriptableObject
{
    [Header("Safe Zone")]
    [SerializeField] private string _safeZoneText;
    [SerializeField] private Color _safeZoneTextColor;
    [SerializeField] private Color _safeZoneBgColor;
    
    [Header("Super Zone")]
    [SerializeField] private string _superZoneText;
    [SerializeField] private Color _superZoneTextColor;
    [SerializeField] private Color _superZoneBgColor;
    
    public Color GetBgSpriteColor(LevelType levelType)
    {
        if (levelType == LevelType.SafeZone)
        {
            return _safeZoneBgColor;
        }

        return _superZoneBgColor;
    }

    public Color GetTextColor(LevelType levelType)
    {
        if (levelType == LevelType.SafeZone)
        {
            return _safeZoneTextColor;
        }

        return _superZoneTextColor;
    }

    public string GetZoneText(LevelType levelType)
    {
        if (levelType == LevelType.SafeZone)
        {
            return _safeZoneText;
        }

        return _superZoneText;
    }
}