using System;
using UnityEngine.AddressableAssets;

namespace CardGame.Utils
{
    [Serializable]
    public class AssetReferenceScriptableManagerBase : AssetReferenceT<AbstractScriptableManagerBase>
    {
        public AssetReferenceScriptableManagerBase(string guid) : base(guid)
        {
        }
    }
}