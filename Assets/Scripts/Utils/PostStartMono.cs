using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CardGame.Utils
{
    public abstract class PostStartMono : MonoBehaviour
    {
        protected virtual void Start()
        {
            DOVirtual.DelayedCall(0.1f, PostStart);
        }
        
        // PostStart is needed for the service locator.
        // In some conditions the service was being used before it could be
        // retrieved inside the start function.
        // For ex:
        //      Class A calls a func from Class B in the start func which uses a
        //      service retrieved from the service locator. If the start function of class A 
        //      is being called before the start func of Class B, Class B tries to use  
        //      a service before it was being retrieved in the start method. 
        // To safely access any service retrieved from the service locator the PostStart func was added
        //  - Awake -> Register Service
        //  - Start -> Get Service
        //  - PostStart -> Safely usable after this 
        protected virtual void PostStart(){}
    }
}