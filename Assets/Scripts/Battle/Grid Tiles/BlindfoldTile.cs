using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class BlindfoldTile : GridTile
    {
        #region Fields
        #endregion

        #region Interfaces
        public override bool IsPointed
        {
            get => isPointed;
            set
            {
                if (Locked) return;
                isPointed = value;
                Pawn hover = hintPawns.Find(pawn => pawn.Id.SameWith(new(Party.Hint, HintType.BlindfoldHover)));
                if (isPointed)
                    hover.StartCoroutine(hover.Cover(1.2f));
                else
                    hover.StartCoroutine(hover.Uncover(1.2f));
            }
        }
        #endregion

        #region Functions
        protected override void OnStart()
        {
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.BlindfoldHover, BreathType.None), PawnInitState.DoNotAppear));
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.Blindfold, BreathType.None), PawnInitState.Transparent));
        }
        protected override void OnInteracted()
        {
            if (Locked) return;

        }
        #endregion
    }
}
