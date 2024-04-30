using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CardGame.Utils
{
    [Serializable]
    public class AssetReferenceScriptableObject : AssetReferenceT<ScriptableObject>
    {
        public AssetReferenceScriptableObject(string guid) : base(guid)
        {
        }
    }
}