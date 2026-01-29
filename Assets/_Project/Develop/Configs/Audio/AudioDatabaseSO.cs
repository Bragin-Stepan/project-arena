using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Develop.Configs.Audio
{
    [CreateAssetMenu(menuName = "Project/Audio/Audio Database")]
    public class AudioDatabaseSO : ScriptableObject
    {
        public List<AudioPlaylistData> SoundEffects;
        public List<AudioPlaylistData> GameMusic;

        private Dictionary<string, AudioPlaylistData> ClipCollection;

        private void OnEnable()
        {
            ClipCollection = new Dictionary<string, AudioPlaylistData>();
            
            AddToCollection(SoundEffects);
            AddToCollection(GameMusic);
        }

        public AudioPlaylistData Get(string clipName) => ClipCollection.GetValueOrDefault(clipName);

        private void AddToCollection(List<AudioPlaylistData> listToAdd)
        {
            foreach (AudioPlaylistData data in listToAdd.Where(data => data != null && !ClipCollection.ContainsKey(data.PlaylistName)))
                ClipCollection.Add(data.PlaylistName, data);
        }
    }
}