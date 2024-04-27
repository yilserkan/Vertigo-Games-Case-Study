using System;
using CardGame.ServiceManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.SpinWheel
{
    public class SpinWheelParticleManager : MonoBehaviour
    {
        [SerializeField] private SpinWheelParticleItem _particleImagePrefab;
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

        public void SpawnParticles(RectTransform targetRect, Sprite sprite,  Action onCompleted)
        {
            var randomAmount = Random.Range(_minParticleCount, _maxParticleCount);
            for (int i = 0; i < randomAmount; i++)
            {
                var instantiated = Instantiate(_particleImagePrefab, _particleParent);
                instantiated.InitializeAndStartAnimation(targetRect, sprite);

                if (i == randomAmount - 1)
                {
                    instantiated.OnParticleCompletedEvent += onCompleted;
                }
            }
        }
    }
}