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
        private GridTile firstTile;
        private GridTile secondTile;
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
        public GridTile FirstTile
        {
            get => firstTile;
            set
            {
                if (firstTile) firstTile.Selected = false;
                firstTile = value;
                if (firstTile) firstTile.Selected = true;
            }
        }
        public GridTile SecondTile
        {
            get => secondTile;
            set
            {
                if (!FirstTile) return;
                if (secondTile) secondTile.Selected = false;
                secondTile = value;
                if (secondTile) secondTile.Selected = true;
            }
        }
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
