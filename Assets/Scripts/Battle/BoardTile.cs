using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class BoardTile : GridTile
    {
        #region Serialized Fields
        #endregion

        #region Fields
        #endregion

        #region Interfaces
        public override bool IsPointed
        {
            get => isPointed;
            set
            {
                isPointed = value;
                if (isPointed && GameManager.Instance.Player.FirstTile)
                    hintPawns[0].Appear();
                else
                    hintPawns[0].Disappear();
            }
        }

        public override bool Selected
        {
            get => selected;
            set
            {
                if (pawns.Count > 0) return;
                selected = value;
            }
        }
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        #endregion

        #region Handlers
        protected override void OnInteract()
        {
            if (!Interactable) return;

            GameManager.Instance.Player.SecondTile = this;
        }
        #endregion
    }
}
