using System.Collections;
using CardGame.ServiceManagement;
using CardGame.Utils;
using TMPro;
using UnityEngine;

namespace CardGame.Extensions
{
    public static class TextExtensions
    {
        private static UnityThreadProvider _unityThread;
        
        // DoText can only be used in DoTween Pro -_-
        public static void CustomDOText(this TextMeshProUGUI tmp, string to, float duration, ScrambleMode scrambleMode = ScrambleMode.Numeric)
        {
            SetUnityThread();
            if (_unityThread == null) { return; }
            
            if (scrambleMode == ScrambleMode.Numeric)
            {
                if (!float.TryParse(tmp.text, out var startVal))
                {
                    Debug.LogWarning($" Trying to Lerp startValue {tmp.text} with numeric. Use only numeric numbers with this mode!");
                    return;
                }

                if (!float.TryParse(to, out var endVal))
                {
                    Debug.LogWarning($" Trying to Lerp to endValue {tmp.text} with numeric. Use only numeric numbers with this mode!");
                    return;
                }
                
                
                _unityThread.StartCoroutine(LerpNumericText(tmp, startVal, endVal, duration));
            }
        }

        private static IEnumerator LerpNumericText(TextMeshProUGUI tmp, float startValue, float endValue, float duration)
        {
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                 var val = Mathf.Lerp(startValue, endValue, timeElapsed / duration);
                 tmp.text = $"{(int)val}"; 
                 timeElapsed += Time.deltaTime;

                yield return null;
            }

            tmp.text = $"{endValue}"; 
        }

        private static void SetUnityThread()
        {
            if (_unityThread == null)
            {
                ServiceLocator.Global.OrNull()?.Get(out _unityThread);
            }
        }
        public enum ScrambleMode
        {
            Numeric
        }
    }
}