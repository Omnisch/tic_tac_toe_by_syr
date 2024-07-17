using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class GameManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private GameSettings settings;
        #endregion

        #region Fields
        #endregion

        #region Interfaces
        public GameSettings Settings => settings;
        #endregion

        #region Functions
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