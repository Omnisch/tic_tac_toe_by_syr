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
                    hintPawns.ForEach(hintPawn => hintPawn.Appear());
                else if (!selected)
                    hintPawns.ForEach(hintPawn => hintPawn.Disappear());
            }
        }

        public override bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                if (selected)
                {
                    pawns.ForEach(pawn => pawn.Highlight());
                    hintPawns.Find(pawn => pawn.Id.SameWith(new(Party.Hint, HintType.ToolInteracted))).Show();
                }
                else
                {
                    pawns.ForEach(pawn => pawn.ToNeutralScale());
                    hintPawns.ForEach(hintPawn => hintPawn.Disappear());
                    hintPawns.Find(pawn => pawn.Id.SameWith(new(Party.Hint, HintType.ToolInteracted))).Hide();
                }
            }
        }
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            base.Start();

            AddPawn(hintPawns, new(Party.Hint, HintType.ToolInteracted, false)).Hide();
        }
        #endregion

        #region Handlers
        protected override void OnInteract()
        {
            if (!Interactable) return;

            GameManager.Instance.Player.FirstTile = this;
        }
        #endregion
    }
}