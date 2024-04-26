using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/SpinWheelAnimationData", fileName = "SpinWheelAnimationData", order = 0)]
public class SpinWheelAnimationData : ScriptableObject
{
    [Header("Start Animation")] 
    public float StartAnimationDuration;
    public Vector3 StartAnimationTargetRotation;
    public AnimationCurve StartAnimationEaseCurve;
    
    [Header("Loop Animation")] 
    public float LoopAnimationDuration;
    public Vector3 LoopAnimationTargetRotation;
    public AnimationCurve LoopAnimationEaseCurve;
    public int LoopAnimationLoopAmount;
    public int MinLoopAmount;
    
    [Header("End Animation")] 
    public float EndAnimationMaxDuration;
    public AnimationCurve EndAnimationEaseCurve;
}