using System.Collections;
using System.Collections.Generic;
using CardGame.Utils;
using TMPro;
using UnityEngine;

public class ZoneCardManager : MonoBehaviour, IObserver<int>
{
    [SerializeField] private TextMeshProUGUI _zoneLevelText;
    
    public void Notify(int value)
    {
        SetLevelText(value);
    }

    private void SetLevelText(int value)
    {
        _zoneLevelText.text = $"{value}";
    }
}
