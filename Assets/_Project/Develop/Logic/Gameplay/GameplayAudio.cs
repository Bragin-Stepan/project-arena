using System;
using _Project.Develop.Logic.Characters;
using _Project.Develop.Logic.Spawn;
using _Project.Develop.Runtime.Utils.ReactiveManagement;
using _Project.Develop.Shared.Const;
using _Project.Develop.Utils.Audio;
using UnityEngine;

namespace _Project.Develop.Logic.Gameplay
{
    public class GameplayAudio : IDisposable
    {
        private readonly AudioService _audioService;
        private readonly MainHeroSpawner _mainHeroSpawner;
        
        private Character _mainHero;
        private IDisposable _disposable;

        public GameplayAudio(
            AudioService audioService,
            MainHeroSpawner mainHeroSpawner,
            IReadOnlyVariable<bool> isGameplayStarted)
        {
            _audioService = audioService;
            _mainHeroSpawner = mainHeroSpawner;
            
            _mainHeroSpawner.Spawned += OnMainHeroSpawned;
            
            _disposable = isGameplayStarted.Subscribe(OnStartGameplay);
        }

        private void OnMainHeroSpawned(Character mainHero)
        {
            _mainHero = mainHero;
            _mainHero.OnShoot += OnShoot;
        }

        private void OnShoot(Vector3 startPoint, Vector3 endPoint)
        {
            _audioService.PlaySFX(AudioNameKey.PistolShoot);
        }

        private void OnStartGameplay(bool oldValue, bool newValue)
        {
            if (newValue)
                _audioService.StartBGM(AudioNameKey.Gameplay);
            else
                _audioService.StopBGM();
        }

        public void Dispose()
        {
            _mainHeroSpawner.Spawned -= OnMainHeroSpawned;
            _mainHero.OnShoot -= OnShoot;
            _disposable.Dispose();
        }
    }
}