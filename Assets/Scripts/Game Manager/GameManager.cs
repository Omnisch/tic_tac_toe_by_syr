using UnityEngine;
using UnityEngine.SceneManagement;

namespace Omnis.TicTacToe
{
    public partial class GameManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private GameSettings settings;
        [SerializeField] private SoundEffectSettings seSettings;
        public GameMode gameMode;
        #endregion

        #region Fields
        private Player player;
        private bool controllable;
        #endregion

        #region Interfaces
        public GameSettings Settings => settings;
        public SoundEffectSettings SeSettings => seSettings;
        public Player Player
        {
            get => player;
            set => player = value;
        }
        public bool Controllable
        {
            get => controllable;
            set
            {
                controllable = value;
                SendMessage("SetInputEnabled", controllable);
            }
        }
        public void SetGameMode(string modeName)
        {
            gameMode = System.Enum.Parse<GameMode>(modeName);
            SceneManager.LoadScene("Battle");
        }
        #endregion

        #region Unity Methods
        protected void Awake()
        {
            if (!EnsureSingleton())
                return;
        }

        protected void Update()
        {
            UpdatePawnBreatheScale();
        }

        protected void OnDestroy() {}
        #endregion
    }
}
