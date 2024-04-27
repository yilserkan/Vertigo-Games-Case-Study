using System;
using CardGame.ServiceManagement;
using DG.Tweening;
using UnityEngine;

namespace CardGame.SpinWheel
{ 
    public class SpinWheelAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _spinWheelParentTransform;
        [SerializeField] private SpinWheelAnimationData _animationData;
        
        private SpinWheelManager _spinWheelManager;
       
        private int _loopCount;
        private int _targetSlotIndex;
        private const int SLOT_INDEX_NOT_SET = -1;
        
        
        private void Awake()
        {
            ServiceLocator.For(this)?.Register<SpinWheelAnimationController>(this);
        }
    
        private void Start()
        {
            ServiceLocator.For(this)?.Get(out _spinWheelManager);
        }

        private void OnDestroy()
        {
            ServiceLocator.For(this, false)?.Unregister(this);
        }

        public void StartSpinAnimation()
        {
            SetTargetSlotIndex(SLOT_INDEX_NOT_SET);
            SetCloudResponseArrivedParam(false);
            _loopCount = 0;
            _animator.Play(_animationData.StartSpinAnimationHash);
        }
    
        public void AnimEvent_OnCheckForResponse()
        {
            if (_loopCount < _animationData.MinLoopCount)
            {
                _loopCount++;
                return;
            }
            
            if (!HasCloudResponseArrived()) { return; }
            
            SetParentRotation();
            SetCloudResponseArrivedParam(true);
        }
        
        public void AnimEvent_OnSpinAnimationCompleted()
        {
            _spinWheelManager.OnSpinWheelAnimationCompleted();
        }
        
        public void SetCloudResponseArrivedParam(bool hasArrived)
        {
            _animator.SetBool(_animationData.CloudResponseArrivedParamName, hasArrived);
        }
    
        public void SetParentRotation()
        {
            var angle = 360 / (float)_spinWheelManager.GetSlotCount();
            _spinWheelParentTransform.eulerAngles = new Vector3(0, 0, angle * _targetSlotIndex);
        }
    
        public void SetTargetSlotIndex(int slotIndex)
        {
            _targetSlotIndex = slotIndex;
        }

        private bool HasCloudResponseArrived() => _targetSlotIndex != SLOT_INDEX_NOT_SET;
    }
}