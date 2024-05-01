using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CardGame.Zones
{
    public class ZoneCardManager : MonoBehaviour, Utils.IObserver<int>
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private TextMeshProUGUI _zoneLevelText;

        private const float NEXT_ZONE_ANIM_DURATION = .25f;
        private const float ZONE_COMPLETED_ANIM_DURATION = .3f;
        
        public void Notify(int value)
        {
            SetLevelText(value);
        }

        private void SetLevelText(int value)
        {
            if (value == ZonePanelManager.ZONE_NOT_FOUND)
            {
                PlayZoneCompletedAnimation();
                return;
            }
            
            EnableZoneCard(true);
            PlayNextZoneAnimation(value);
        }

        private void EnableZoneCard(bool enable)
        {
            gameObject.SetActive(enable);
        }

        private void PlayNextZoneAnimation(int value)
        {
            _parent.DOKill();
            _parent.localScale = Vector3.one;
            _zoneLevelText.text = value.ToString();
            _parent.DOScale(1.15f, NEXT_ZONE_ANIM_DURATION).SetLoops(2, LoopType.Yoyo);
        }

        private void PlayZoneCompletedAnimation()
        {
            _parent.DOScale(0, ZONE_COMPLETED_ANIM_DURATION)
                .SetEase(Ease.InBack)
                .OnComplete(() => EnableZoneCard(false));
        }
    }
}
