using System.Collections;
using _Project.Develop.Configs.Audio;
using _Project.Develop.Shared.Const;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Develop.Utils.Audio
{
    public class AudioService
    {
        private AudioDatabaseSO _audioDB;
        private AudioSource _bgmSource;
        private AudioSource _sfxSource;
        private bool _isBgmPlay;
    
        private const float MinValuePitch = 0.95f;
        private const float MaxValuePitch = 1.1f;

        public AudioService(ConfigsLoader configsLoader, ResourcesLoader resourcesLoader)
        {
            _audioDB = configsLoader.GetConfig<AudioDatabaseSO>();
            
            AudioSource bgmPrefab = resourcesLoader.Load<AudioSource>(ResourcesPath.AudioBackground);
            AudioSource sfxPrefab = resourcesLoader.Load<AudioSource>(ResourcesPath.AudioSFX);
            
            _bgmSource = Object.Instantiate(bgmPrefab, Vector3.zero, Quaternion.identity, null);
            _sfxSource = Object.Instantiate(sfxPrefab, Vector3.zero, Quaternion.identity, null);
        }

        public void PlaySFX(string nameKey)
        {
            AudioPlaylistData data = _audioDB.Get(nameKey);
            if (data == null)
                return;

            AudioClipData clipData = data.GetRandomClip();
            if (clipData == null)
                return;

            _sfxSource.clip = clipData.Clip;
            _sfxSource.pitch = Random.Range(MinValuePitch, MaxValuePitch);
            _sfxSource.volume = data.Volume;
            _sfxSource.PlayOneShot(clipData.Clip);
        }
    
        public void StartBGM(string nameKey)
        {
            AudioPlaylistData data = _audioDB.Get(nameKey);
            if (data == null)
                return;
        
            AudioClipData clipData = data.GetRandomClip();
            if (clipData == null)
                return;
        
            if (clipData.Clip == null)
                return;
        
            _bgmSource.clip = clipData.Clip;
            _bgmSource.volume = data.Volume;
            _bgmSource.Play();
        }

        public void StopBGM() => _bgmSource.Stop();
    }
}