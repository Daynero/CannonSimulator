using System.Collections.Generic;
using Controllers;
using Screens.LoadingScreen;
using Screens.MainScreen;
using XmannaSDK.Core;

namespace Core
{
    using UnityEngine;

    public class CompositionRoot : MonoBehaviour
    {
        [Header("Root Objects")]
        [SerializeField] private Transform allScreensContainer;
        [SerializeField] private GameObject[] allScreenPrefabs;

        private readonly Dictionary<string, GameObject> _allScreens = new();
        private static ScreenNavigationSystem _screenNavigationSystem;
        private LoadingScreenPresenter _loadingScreenPresenter;
        private MainScreenPresenter _mainScreenPresenter;
        private GameController _gameController;
        
        private void Awake()
        {
            foreach (var somePrefab in allScreenPrefabs)
            {
                _allScreens.Add(somePrefab.name, somePrefab);
            }
            
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            Initialize();
            
            _loadingScreenPresenter.OnStart();
        }

        private void Initialize()
        {
            _screenNavigationSystem = new ScreenNavigationSystem(AddScreenToScene);

            // ControllersInitialization();
            AddScreenToScene(ScreenName.LoadingScreen);
        }

        private void ControllersInitialization()
        {
            _gameController = new GameController(_screenNavigationSystem, _mainScreenPresenter);
        }
        
        private void InstantiateView<T>(Transform parent, string screenName, out T view)
        {
            string objName = screenName.Remove(screenName.Length - 4);

            GameObject prefab;
            
            if (_allScreens.ContainsKey(objName))
            {
                prefab = _allScreens[objName];
            }
            else
            {
                Debug.LogWarning("Screen not found: " + objName);
                prefab = _allScreens[objName];
            }

            GameObject newGO = Instantiate(prefab, parent);
            view = newGO.GetComponent<T>();

            if (!newGO.TryGetComponent(out ScreenView screenView)) return;

            _screenNavigationSystem.AddScreen(screenView.ScreenName, screenView);
        }

        private void AddScreenToScene(ScreenName screenName)
        {
            switch (screenName)
            {
                case ScreenName.MainScreen:
                    InstantiateView(allScreensContainer, nameof(MainScreenView),
                        out MainScreenView mainScreenView);
                    _mainScreenPresenter = new MainScreenPresenter(mainScreenView, _screenNavigationSystem);
                    break;
                case ScreenName.LoadingScreen:
                    InstantiateView(allScreensContainer, nameof(LoadingScreenView),
                        out LoadingScreenView loadingScreenView);
                    _loadingScreenPresenter = new LoadingScreenPresenter(loadingScreenView, _screenNavigationSystem);
                    break;
            }
        }
    }
}