using System;
using CardGame.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.SpinWheel
{
    public class SpinWheelSlotManager : MonoBehaviour
    {
        [SerializeField] private Image _slotIcon;
        [SerializeField] private TextMeshProUGUI _amountText;

        public void InitializeSlot(ItemData data, int amount)
        {
            _slotIcon.sprite = data.Sprite;
            _amountText.text = amount.ToString();
        }
    }
}
