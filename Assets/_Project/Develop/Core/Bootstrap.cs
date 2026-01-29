using System.Collections;
using _Project.Develop.Logic.Gameplay;
using _Project.Develop.Services.Inputs;
using _Project.Develop.Utils;
using _Project.Develop.Utils.Audio;
using UnityEngine;

namespace _Project.Develop.Core
{
    public class Bootstrap : MonoBehaviour
    {
        private PlayerInput _playerInput = new ();
        private GameplayCycle _gameplayCycle;
        
        private bool _isInitialized;
        
        private void Awake()
        {
            StartCoroutine(StartProcess());
        }

        private void Update()
        {
            if (_isInitialized == false)
                return;
            
            _playerInput.Update();
            _gameplayCycle.Update(Time.deltaTime);
        }

        private IEnumerator StartProcess()
        {
            ResourcesLoader resourcesLoader = new ();
            CoroutineRunner coroutineRunner = new (this);
            ConfigsLoader configsLoader = new (resourcesLoader);

            // _loadingScreen.Show(); // Сегодня без UI

            yield return configsLoader.LoadAsync();

            AudioService audioService = new (configsLoader, resourcesLoader);

            _gameplayCycle = new (
                coroutineRunner,
                configsLoader,
                audioService,
                _playerInput);
            
            yield return _gameplayCycle.LoadAsync();
            
            // _loadingScreen.Hide();
            
            _isInitialized = true;

            yield return _gameplayCycle.Start();
        }

        private void OnDestroy()
        {
            _gameplayCycle?.Dispose();
        }
    }
}