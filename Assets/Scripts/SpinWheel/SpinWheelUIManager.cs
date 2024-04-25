using System;
using CardGame.ServiceManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.SpinWheel
{
    public class SpinWheelUIManager : MonoBehaviour
    {
        [SerializeField] private Button _spinButton;

        private SpinWheelManager _spinWheelManager;
        
        private void Awake()
        {
            ServiceLocator.For(this).Register<SpinWheelUIManager>(this);
        }

        private void Start()
        {
            ServiceLocator.For(this).Get(out _spinWheelManager);
        }

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void HandleOnSpinButtonClicked()
        {
            _spinWheelManager.SpinWheel();
        }
        
        private void AddListeners()
        {
            _spinButton.onClick.AddListener(HandleOnSpinButtonClicked);
        }

        private void RemoveListeners()
        {
            _spinButton.onClick.AddListener(HandleOnSpinButtonClicked);
        }
    }
}