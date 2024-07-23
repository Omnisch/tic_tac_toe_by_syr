using System.Collections.Generic;
using UnityEngine;

namespace Omnis
{
    [CreateAssetMenu(menuName = "Omnis/Sound Effect Settings")]
    public class SoundEffectSettings : ScriptableObject
    {
        public List<SoundEffectId> soundEffects;
    }

    [System.Serializable]
    public struct SoundEffectId
    {
        public SoundEffectName name;
        public AudioClip se;
    }

    public enum SoundEffectName
    {
        Blip,
        NatureKick,
        ArtifactKick,
        SetKick,
        Finish,
    }
}
