using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class PlayerManager : MonoBehaviour
    {
        #region Serialized Fields
        #endregion

        #region Fields
        private bool canInteract;
        private Party turn;
        private Pawn activePawn;
        #endregion

        #region Interfaces
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        private void Awake()
        {
            GameManager.Instance.Player = this;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance.Player == this) GameManager.Instance.Player = null;
        }
        #endregion
    }
}
