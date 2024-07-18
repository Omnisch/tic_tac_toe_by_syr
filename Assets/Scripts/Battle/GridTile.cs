using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Omnis.TicTacToe
{
    public class GridTile : PointerBase
    {
        #region Serialized Fields
        #endregion

        #region Fields
        private int x;
        private int y;
        #endregion

        #region Interfaces
        public override bool IsPointed
        {
            get => isPointed;
            set => isPointed = value;
        }
        public void SpawnPawn(PawnId id)
        {

        }
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        #endregion

        #region Handlers
        protected override void OnInteract()
        {
            Debug.Log("Clicked on grid tile.");
        }
        #endregion
    }
}
