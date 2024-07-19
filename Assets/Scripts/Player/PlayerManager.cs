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
        #endregion

        #region Interfaces
        public bool CanInteract
        {
            get => canInteract;
            set
            {
                canInteract = value;
                GameManager.Instance.SendMessage("SetInputEnabled", value);
                Cursor.visible = canInteract;
            }
        }
        public Party ActiveParty { get; set; }
        public GridTile FirstTile { get; set; }
        public GridTile SecondTile { get; set; }
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        private void Start()
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
