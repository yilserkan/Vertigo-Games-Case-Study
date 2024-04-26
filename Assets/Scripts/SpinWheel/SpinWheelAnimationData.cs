using System;
using CardGame.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardGame.SpinWheel
{
    
    [CreateAssetMenu(menuName = "ScriptableObjects/SpinWheelAnimationData", fileName = "SpinWheelAnimationData", order = 0)]
    public class SpinWheelAnimationData : AbstractScriptableManagerBase
    {
        [SerializeField] private AnimationClip _startSpinAnimation;
        [SerializeField] private AnimationClip _loopSpinAnimation;
        [SerializeField] private AnimationClip _endSpinAnimation;
        [SerializeField] private string _cloudResponseArrivedParamName;
        [SerializeField] private int _minLoopCount;
        
        private int _startSpinAnimationHash;
        private int _loopSpinAnimationHash;
        private int _endSpinAnimationHash;
        
        public override void Initialize()
        {
            base.Initialize();
            _startSpinAnimationHash = Animator.StringToHash(_startSpinAnimation.name);
            _loopSpinAnimationHash = Animator.StringToHash(_loopSpinAnimation.name);
            _endSpinAnimationHash = Animator.StringToHash(_endSpinAnimation.name);
        }

        public int StartSpinAnimationHash => _startSpinAnimationHash;
        public int LoopSpinAnimationHash => _loopSpinAnimationHash;
        public int EndSpinAnimationHash => _endSpinAnimationHash;
        public string CloudResponseArrivedParamName => _cloudResponseArrivedParamName;
        public int MinLoopCount => _minLoopCount;
    }
}