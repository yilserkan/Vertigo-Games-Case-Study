using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Extensions;
using CardGame.Items;
using CardGame.ServiceManagement;
using CardGame.Utils;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace CardGame.RemoteConfig
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Config/SpinWheelConfigData", fileName = "SpinWheelConfigData", order = 0)]
    public class SpinWheelConfigData : AbstractScriptableManagerBase
    {
        [SerializeField] private WheelRemoteConfigData _configData;
        public WheelRemoteConfigData ConfigData => _configData;// != null ? ConfigData : g;
        
        public override void Initialize()
        {
            base.Initialize();
            ServiceLocator.LazyGlobal.OrNull()?.Register(this);
            
            var json = RemoteConfigService.Instance.appConfig.GetJson("SPIN_WHEEL_CONFIG");
            var remoteConfig = JsonUtility.FromJson<WheelRemoteConfig>(json);
            _configData = remoteConfig.Config;
        }
    }

    [Serializable]
    public class WheelRemoteConfig
    {
        public WheelRemoteConfigData Config;
    }

    [Serializable]
    public class WheelRemoteConfigData
    {
        public int SafeZoneInterval;
        public int SuperZoneInterval;
        public int LevelCount;
        public int SliceCount;
        public int ReviveCost;
    }
}

