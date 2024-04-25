using System;
using System.Threading.Tasks;
using CardGame.ServiceManagement;
using CardGame.Singleton;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CardGame.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneData[] _sceneDatas;

        private AsyncOperationHandle<SceneInstance> _activeAddressableSceneHandle;
        private AsyncOperationHandle<SceneInstance> _prevAddressableSceneHandle;
        
        private const string INITIALIZATION_SCENE_NAME = "InitializationScene"; 
        public void Awake()
        {
            ServiceLocator.Global.Register(this);
        }

        [ContextMenu("LoadGameScene")]
        public void LoadGameScene()
        {
            var progress = new LoadingProgress();
            progress.Progressed += f => Debug.Log("Progressed " + f); 
            LoadScene(SceneType.GameScene);
        }
        
        [ContextMenu("LoadInitialScene")]
        public void LoadInitialScene()
        {
            var progress = new LoadingProgress();
            progress.Progressed += f => Debug.Log("Progressed " + f); 
            LoadScene(SceneType.InitializationScene);
        }

        public async Task LoadScene(SceneType type)
        {
            ServiceLocator.Global.Get<LoadingHelper>(out var loadingHelper);
            loadingHelper.EnableLoadingPanel(true);
            
            Scene prevScene = SceneManager.GetActiveScene();
            _prevAddressableSceneHandle = _activeAddressableSceneHandle;
            
            if (type == SceneType.InitializationScene)
            {
                await LoadInitialScene(loadingHelper);
            }
            else
            {
                await LoadAddressableScene(type, loadingHelper);
            }

            await Task.Delay(1000);
            await UnloadScene(prevScene);
            
            loadingHelper.EnableLoadingPanel(false);
        }

        private async Task LoadInitialScene(IProgress<float> progress)
        {
            var handle = SceneManager.LoadSceneAsync(INITIALIZATION_SCENE_NAME, LoadSceneMode.Additive);

            while (!handle.isDone)
            {
                progress?.Report(handle.progress);
                await Task.Delay(100);
            }
        }

        private async Task LoadAddressableScene(SceneType type, IProgress<float> progress)
        {
            if (!TryGetAddressableSceneReference(type, out var sceneReference)) { return; }
            
            _activeAddressableSceneHandle = sceneReference.LoadSceneAsync(LoadSceneMode.Additive);

            while (!_activeAddressableSceneHandle.IsDone)
            {
                progress?.Report(_activeAddressableSceneHandle.PercentComplete);
                await Task.Delay(100);
            }
        }

        public async Task UnloadScene(Scene scene)
        {
            if (scene.name.Equals(INITIALIZATION_SCENE_NAME) || scene.name.Equals("Test 2"))
            {
                await UnloadInitialScene();    
            }
            else
            {
                await UnloadAddressableScene();
            }
        }

        private async Task UnloadInitialScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            var handle = SceneManager.UnloadSceneAsync(activeScene);

            while (!handle.isDone)
            {
                await Task.Delay(100);
            }
            
            Resources.UnloadUnusedAssets();
        }

        private async Task UnloadAddressableScene()
        {
            if (!_prevAddressableSceneHandle.IsValid()) { return; }
            
            var handle = Addressables.UnloadSceneAsync(_prevAddressableSceneHandle);
            while (!handle.IsDone)
            {
                await Task.Delay(100);
            }
            
            Resources.UnloadUnusedAssets();
        }

        private bool TryGetAddressableSceneReference(SceneType type, out AssetReference reference)
        {
            for (int i = 0; i < _sceneDatas.Length; i++)
            {
                if (_sceneDatas[i].SceneType == type)
                {
                    reference = _sceneDatas[i].SceneReference;
                    return true;
                }
            }

            reference = null;
            return false;
        }
    }
    
    public enum SceneType {InitializationScene, GameScene}
}
