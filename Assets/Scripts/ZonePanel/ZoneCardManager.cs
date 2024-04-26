using System.Collections;
using System.Collections.Generic;
using CardGame.Utils;
using TMPro;
using UnityEngine;

namespace CardGame.Zones
{
    public class ZoneCardManager : MonoBehaviour, IObserver<int>
    {
        [SerializeField] private TextMeshProUGUI _zoneLevelText;
    
        public void Notify(int value)
        {
            SetLevelText(value);
        }

        private void SetLevelText(int value)
        {
            if (value == ZonePanelManager.ZONE_NOT_FOUND)
            {
                EnableZoneCard(false);
                return;
            }
            
            EnableZoneCard(true);
            _zoneLevelText.text = $"{value}";
        }

        private void EnableZoneCard(bool enable)
        {
            gameObject.SetActive(enable);
        }
    }
}
