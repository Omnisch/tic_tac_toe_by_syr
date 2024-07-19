using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class ToolbarTile : GridTile
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
                if (isPointed && pawns.Count > 0)
                    hintPawns[0].Appear();
                else
                    hintPawns[0].Disappear();
            }
        }
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            base.Start();

            AddPawn(hintPawns, new PawnId(Party.Hint, HintType.ToolInteracted, false));
        }
        #endregion

        #region Handlers
        protected override void OnInteract()
        {
            if (!Interactable) return;

            GameManager.Instance.Player.FirstTile = this;
            pawns.ForEach(pawn => pawn.Highlight());
            hintPawns.Find(hintPawn => hintPawn.Id.type == (int)HintType.ToolInteracted).Appear();
        }
        #endregion
    }
}
