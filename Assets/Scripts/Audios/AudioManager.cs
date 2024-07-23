using UnityEngine;

namespace Omnis
{
    [RequireComponent(typeof(AudioSource))]
    public partial class AudioManager : MonoBehaviour
    {
        #region Fields
        private AudioSource source;
        private bool mute;
        #endregion

        #region Interfaces
        public bool Mute
        {
            get => mute;
            set => mute = value;
        }
        public void PlaySE(SoundEffectName byName)
        {
            if (!Mute)
                source.PlayOneShot(TicTacToe.GameManager.Instance.SeSettings.soundEffects.Find(se => se.name == byName).se);
        }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (!EnsureSingleton())
                return;
        }
        private void Start()
        {
            source = GetComponent<AudioSource>();
        }
        #endregion
    }
}
