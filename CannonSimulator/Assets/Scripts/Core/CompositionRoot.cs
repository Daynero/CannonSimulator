using System.Collections.Generic;
using Controllers;
using Enums;
using Objects.Cannon;
using Screens.LoadingScreen;
using Screens.MainScreen;

namespace Core
{
    using UnityEngine;

    public class CompositionRoot : MonoBehaviour
    {
        [Header("Root Objects")]
        [SerializeField] private Transform allScreensContainer;
        [SerializeField] private GameObject[] allScreenPrefabs;
        [SerializeField] private GameObject[] allObjectsPrefabs;

        private readonly Dictionary<string, GameObject> _allScreens = new();
        private readonly Dictionary<string, GameObject> _allObjects = new();
        private static ScreenNavigationSystem _screenNavigationSystem;
        private LoadingScreenPresenter _loadingScreenPresenter;
        private MainScreenPresenter _mainScreenPresenter;
        private GameController _gameController;
        private CannonController _cannonController;
        private CannonView _cannonView;

        private void Awake()
        {
            foreach (var somePrefab in allScreenPrefabs)
            {
                _allScreens.Add(somePrefab.name, somePrefab);
            }
            
            foreach (var somePrefab in allObjectsPrefabs)
            {
                _allObjects.Add(somePrefab.name, somePrefab);
            }
            
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _screenNavigationSystem = new ScreenNavigationSystem(AddScreenToScene);

            AddScreenToScene(ObjectName.LoadingScreen);
            
            ControllersInitialization();
        }

        private void ControllersInitialization()
        {
            _gameController = new GameController(_screenNavigationSystem, _mainScreenPresenter);
        }
        
        private void InstantiateView<T>(Transform parent, string screenName, out T view)
        {
            string objName = screenName.Remove(screenName.Length - 4);

            GameObject prefab = null;
            
            if (_allScreens.ContainsKey(objName))
            {
                prefab = _allScreens[objName];
            }
            else
            {
                Debug.LogWarning("Screen not found: " + objName);
            }

            GameObject newGO = Instantiate(prefab, parent);
            view = newGO.GetComponent<T>();

            if (!newGO.TryGetComponent(out ScreenView screenView)) return;

            _screenNavigationSystem.AddScreen(screenView.ObjectName, screenView);
        }
        
        private void InstantiateObject<T>(string screenName, out T view)
        {
            string objName = screenName.Remove(screenName.Length - 4);

            GameObject prefab = null;
            
            if (_allObjects.ContainsKey(objName))
            {
                prefab = _allObjects[objName];
            }
            else
            {
                Debug.LogWarning("Object not found: " + objName);
            }

            view = prefab!.GetComponent<T>();
        }

        private void AddScreenToScene(ObjectName objectName)
        {
            switch (objectName)
            {
                case ObjectName.MainScreen:
                    InstantiateView(allScreensContainer, nameof(MainScreenView),
                        out MainScreenView mainScreenView);
                    _mainScreenPresenter = new MainScreenPresenter(mainScreenView, _screenNavigationSystem);
                    AddScreenToScene(ObjectName.Cannon);
                    break;
                case ObjectName.LoadingScreen:
                    InstantiateView(allScreensContainer, nameof(LoadingScreenView),
                        out LoadingScreenView loadingScreenView);
                    _loadingScreenPresenter = new LoadingScreenPresenter(loadingScreenView, _screenNavigationSystem);
                    break;
                case ObjectName.Cannon:
                    InstantiateObject(nameof(CannonView), out CannonView cannonView);
                    _cannonController = new CannonController(cannonView, _mainScreenPresenter);
                    break;
            }
        }
    }
}