using UnityEngine;

namespace CardGame.Extensions
{
    public static class GameObjectExtensions
    {
        
        // With this extension we can check if GameObjects are null simply with gameObject.OrNull()?.DoSomething()
        // instead of if(gameObject != null ) gameObject.DoSomething()
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
    }
}