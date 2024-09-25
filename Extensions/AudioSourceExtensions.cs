using System;
using UnityEngine;


namespace Extensions
{
    public static partial class AudioSourceExtensions
    {
        public static void Play(this AudioSource source, AudioClip clipToPlay)
        {
            if (source == null) throw new NullReferenceException("Audio source is null");
            if (clipToPlay == null) throw new NullReferenceException("Audio clip is null");

            source.clip = clipToPlay;
            source.Play();
        }
    }

}
