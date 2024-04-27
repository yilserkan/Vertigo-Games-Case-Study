using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Inventory;
using CardGame.Items;
using TMPro;
using UnityEngine;

namespace CardGame.Currency
{
    public class CurrencyUIHelper : MonoBehaviour, Utils.IObserver<float>
    {
        [SerializeField] private CurrencyType _currencyType;
        [SerializeField] private TextMeshProUGUI _currencyAmountText;

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
            _currencyAmountText.text = $"{value}";
        }
    }
}
