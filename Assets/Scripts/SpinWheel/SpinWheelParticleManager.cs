using System;
using CardGame.Extensions;
using CardGame.ServiceManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace CardGame.SpinWheel
{
    public class SpinWheelParticleManager : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject _particleReference;
        [SerializeField] private Transform _particleParent;
        [SerializeField] private int _minParticleCount;
        [SerializeField] private int _maxParticleCount;
        
        
        private void Awake()
        {
            ServiceLocator.For(this).Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.For(this, false)?.Unregister(this);
        }

        public async void SpawnParticles(RectTransform targetRect, Sprite sprite,  Action onCompleted)
        {
            var randomAmount = Random.Range(_minParticleCount, _maxParticleCount);
            for (int i = 0; i < randomAmount; i++)
            {
                var isLastParticle = i == randomAmount - 1;
                var instantiated = await _particleReference.InstantiateAddressableAsync(_particleParent);
                
                if(!instantiated.TryGetComponent(out SpinWheelParticleItem particleItem)) continue;
                particleItem.InitializeAndStartAnimation(targetRect, sprite);
                if (isLastParticle)
                {
                    particleItem.OnParticleCompletedEvent += onCompleted;
                }
            }
        }
    }
}