using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Develop.Configs.Audio
{
    [Serializable]
    public class AudioPlaylistData
    {
        public string PlaylistName;
        public List<AudioClipData> Clips = new ();
        [Range(0f, 1f)] public float Volume = 0.5f;

        public AudioClipData GetRandomClip()
        {
            if (Clips == null || Clips.Count == 0)
                return null;

            return Clips[Random.Range(0, Clips.Count)];
        }
    }
}