using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Extensions;
using CardGame.Inventory;
using CardGame.Items;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CardGame.Currency
{
    public class CurrencyUIHelper : MonoBehaviour, Utils.IObserver<float>
    {
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private TextMeshProUGUI _currencyAmountText;

        private const float ANIM_DURATION = .5f;
        
        private void Start()
        {
            if (PlayerInventory.Currencies.TryGetValue(PlayerInventory.GetCurrencyID(_currencyType), out var observableCurrency))
            {
                observableCurrency.AddListener(this);   
                Notify(observableCurrency.Value);
            }
        }

        public void Notify(float value)
        {
            _currencyAmountText.CustomDOText(value.ToString(), ANIM_DURATION);
        }
    }
}
