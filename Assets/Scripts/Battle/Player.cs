using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class Player : MonoBehaviour
    {
        #region Fields
        private GridSet toolkit;
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

        public Player(GridSet toolkit)
        {
            this.toolkit = toolkit;
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
