using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class ToolkitTile : GridTile
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
                    hintPawns.ForEach(hintPawn => hintPawn.StartCoroutine(hintPawn.Appear()));
                else if (!selected)
                    hintPawns.ForEach(hintPawn => hintPawn.StartCoroutine(hintPawn.Disappear()));
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
                    pawns.ForEach(pawn => StartCoroutine(pawn.Highlight()));
                    hintPawns.Find(pawn => pawn.Id.SameWith(new(Party.Hint, HintType.ToolInteracted))).Show();
                }
                else
                {
                    pawns.ForEach(pawn => StartCoroutine(pawn.ToNeutralScale()));
                    hintPawns.ForEach(hintPawn => StartCoroutine(hintPawn.Disappear()));
                    hintPawns.Find(pawn => pawn.Id.SameWith(new(Party.Hint, HintType.ToolInteracted))).Hide();
                }
            }
        }
        #endregion

        #region Functions
        protected override void OnStart()
        {
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.Tool, false), PawnInitState.DoNotAppear));
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.ToolInteracted, false), PawnInitState.Transparent));
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
