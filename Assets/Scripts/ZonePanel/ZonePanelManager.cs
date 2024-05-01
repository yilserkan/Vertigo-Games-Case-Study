using CardGame.Panels;
using CardGame.ServiceManagement;
using CardGame.SpinWheel;
using CardGame.Utils;
using UnityEngine;

namespace CardGame.Zones
{
    [RequireComponent(typeof(PanelAnimationHelper))]
    public class ZonePanelManager : MonoBehaviour
    {
        [SerializeField] private PanelAnimationHelper _panelAnimationHelper;
        [SerializeField] private ZoneCardManager _safeZoneCardManager;
        [SerializeField] private ZoneCardManager _superZoneCardManager;

        private LevelManager _levelManager;
        private ZonePanelUIManager _zonePanelUIManager;
        private Observable<int> _safeZoneLevel;
        private Observable<int> _superZoneLevel;

        public const int ZONE_NOT_FOUND = -1;
        
        private void OnEnable()
        {
            AddListeners();
        }


        private void OnDisable()
        {
            RemoveListeners();
        }

        private void OnValidate()
        {
            _panelAnimationHelper = GetComponent<PanelAnimationHelper>();
        }
        
        private void Awake()
        {
            _safeZoneLevel = new Observable<int>(0, _safeZoneCardManager);
            _superZoneLevel = new Observable<int>(0, _superZoneCardManager);
        }

        private void Start()
        {
            ServiceLocator.ForScene(this)?.Get(out _levelManager);
            ServiceLocator.For(this)?.Get(out _zonePanelUIManager);
        }

        public void Initialize()
        {
            SetNextZoneLevel(LevelType.SafeZone, _levelManager.CurrentStage);
            SetNextZoneLevel(LevelType.SuperZone, _levelManager.CurrentStage);
        }

        private void HandleOnShowZonePanel()
        {
            var level = _levelManager.CurrentStage;
            LevelType currentLevelType = (LevelType)_levelManager.LevelData.Levels[level].LevelType;
            _zonePanelUIManager.ShowZonePanel(currentLevelType);
            _panelAnimationHelper.PlayOpeningAnimation(() => OnOpeningAnimationCompleted(currentLevelType, level));
        }

        private void OnOpeningAnimationCompleted(LevelType currentLevelType, int level)
        {
            _zonePanelUIManager.SetButtonInteractables(true);
            SetNextZoneLevel(currentLevelType, level);
        }
        
        private void SetNextZoneLevel(LevelType type, int currentLevel)
        {
            var nextZoneLevel = FindNextZoneLevel(type, currentLevel);
            if (type == LevelType.SafeZone)
            {
                _safeZoneLevel.Value = nextZoneLevel;
            }
            else
            {
                _superZoneLevel.Value = nextZoneLevel;
            }
        }

        private int FindNextZoneLevel(LevelType type, int currentLevel)
        {
            for (int i = currentLevel + 1; i < _levelManager.LevelData.Levels.Length; i++)
            {
                if (_levelManager.LevelData.Levels[i].LevelType == (int)type)
                {
                    return i + 1;
                }
            }

            return -1;
        }
        
        private void AddListeners()
        {
            LevelManager.OnStartGame += Initialize;
            LevelManager.OnShowZonePanel += HandleOnShowZonePanel;
        }

        private void RemoveListeners()
        {
            LevelManager.OnStartGame -= Initialize;
            LevelManager.OnShowZonePanel -= HandleOnShowZonePanel;
        }
    }
}
