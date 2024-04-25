using System;
using CardGame.ServiceManagement;
using DG.Tweening;
using UnityEngine;

namespace CardGame.SpinWheel
{
//     public class SpinWheelAnimationController : MonoBehaviour
//     {
//         [SerializeField] private Transform _spinWheelParentTransform;
//         [SerializeField] private SpinWheelAnimationData _animationData;
//         
//         private SpinWheelManager _spinWheelManager;
//         
//         private const int _slotCount = 8;
//         public bool responseArrived;
//
//         public static event Action OnSpinAnimationCompleted;
//         private bool _hasCloudResponseArrived;
//         private int _loopCount;
//         public int TestIndex;
//
//         private void Awake()
//         {
//             ServiceLocator.For(this).Register<SpinWheelAnimationController>(this);
//         }
//
//         private void Start()
//         {
//             ServiceLocator.For(this).Get(out _spinWheelManager);
//         }
//
//         public void StartSpinAnimation()
//         {
//             ResetParams();
//             _spinWheelParentTransform.eulerAngles = Vector3.zero;
//             _spinWheelParentTransform
//                 .DORotate(_animationData.StartAnimationTargetRotation, _animationData.StartAnimationDuration, RotateMode.WorldAxisAdd)
//                 .SetEase(_animationData.StartAnimationEaseCurve)
//                 .OnComplete(LoopDoTweenAnimation);
//         }
//
//         private void LoopDoTweenAnimation()
//         {
//             _spinWheelParentTransform
//                 .DORotate(_animationData.LoopAnimationTargetRotation, _animationData.LoopAnimationDuration, RotateMode.WorldAxisAdd)
//                 .SetLoops(_animationData.LoopAnimationLoopAmount)
//                 .SetEase(_animationData.LoopAnimationEaseCurve)
//                 .OnComplete(OnLoopAnimationCompleted);
//         }
//
//         private void OnLoopAnimationCompleted()
//         {
//             if (_loopCount < _animationData.MinLoopAmount || !_hasCloudResponseArrived)
//             {
//                 _loopCount++;
//                 LoopDoTweenAnimation();
//                 return;
//             }
//             
//             PlayEndAnimation();
//         }
//
//         private void PlayEndAnimation()
//         {
//             _spinWheelParentTransform
//                 .DORotate(GetTargetRotation(), GetEndAnimDuration(), RotateMode.WorldAxisAdd)
//                 .SetEase(_animationData.EndAnimationEaseCurve)
//                 .OnComplete(OnSpinWheelAnimationCompleted);
//         }
//
//         private void ResetParams()
//         {
//             SetGotCloudResponse(false);
//             _loopCount = 0;
//         }
//         
//         private void OnSpinWheelAnimationCompleted()
//         {
//             OnSpinAnimationCompleted?.Invoke();
//         }
//
//         private Vector3 GetTargetRotation()
//         {
//             float wheelSliceAngle = 360 / (float)_slotCount;
//             float zAngle = wheelSliceAngle * TestIndex;
//             // if (Math.Abs(zAngle) < 1 )
//             // {
//             //     zAngle = 360;
//             // }
//
//             // if (TestIndex <= 6)
//             // {
//             //     zAngle += 360;
//             // }
//             zAngle += 360;
//             return new Vector3(0, 0, zAngle);
//         }
//
//         private float GetEndAnimDuration()
//         {
//             return _animationData.EndAnimationMaxDuration;
//             return _animationData.EndAnimationMaxDuration + (_animationData.EndAnimationMaxDuration / 2 ) * ( TestIndex / 8f);
//             
//             if (TestIndex == 0 ) return _animationData.EndAnimationMaxDuration;
//
//             return _animationData.EndAnimationMaxDuration / 8 * TestIndex;
//         }
//
//         public void SetGotCloudResponse(bool gotResponse)
//         {
//             _hasCloudResponseArrived = gotResponse;
//         }
//     }
// }


 public class SpinWheelAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _spinWheelParentTransform;
    
        [SerializeField] private AnimationCurve _startAnimCurve;
        [SerializeField] private AnimationCurve _startLoopCurve;
        
        
        
        private SpinWheelManager _spinWheelManager;
        
        private readonly int START_SPIN_ANIM_HASH = Animator.StringToHash("Animation_SpinWheel_Start");
        private readonly int LOOP_SPIN_ANIM_HASH = Animator.StringToHash("Animation_SpinWheel_Loop");
        private readonly int END_SPIN_ANIM_HASH = Animator.StringToHash("Animation_SpinWheel_End");
    
        private readonly string CLOUD_RESPONSE_ARRIVED_PARAM = "CloudResponseArrived";
        private const int _slotCount = 8;
        public bool responseArrived;
    
        public static event Action OnSpinAnimationCompleted;
        private bool _hasCloudResponseArrived;
        private int _loopCount;
        private const int MIN_LOOP_COUNT = 3;
        
        private void Awake()
        {
            ServiceLocator.For(this).Register<SpinWheelAnimationController>(this);
        }
    
        private void Start()
        {
            ServiceLocator.For(this).Get(out _spinWheelManager);
        }
        
        public void StartSpinAnimation()
        {
            SetGotCloudResponse(false);
            SetCloudResponseArrivedParam(false);
            _loopCount = 0;
            _animator.Play(START_SPIN_ANIM_HASH);
        }
    
        public void AnimEvent_OnCheckForResponse()
        {
            if (_loopCount < MIN_LOOP_COUNT)
            {
                _loopCount++;
                return;
            }
            
            if (!_hasCloudResponseArrived) { return; }
            
            SetParentRotation(_spinWheelManager.SpinWheelResponse.SlotIndex);
            SetCloudResponseArrivedParam(true);
        }
        
        public void AnimEvent_OnSpinAnimationCompleted()
        {
            OnSpinAnimationCompleted?.Invoke();
        }
        
        public void SetCloudResponseArrivedParam(bool hasArrived)
        {
            _animator.SetBool(CLOUD_RESPONSE_ARRIVED_PARAM, hasArrived);
        }
    
        public void SetParentRotation(int slotIndex)
        {
            var angle = 360 / (float)_slotCount;
            _spinWheelParentTransform.eulerAngles = new Vector3(0, 0, angle * slotIndex);
        }
    
        public void SetGotCloudResponse(bool gotResponse)
        {
            _hasCloudResponseArrived = gotResponse;
        }
    }
}