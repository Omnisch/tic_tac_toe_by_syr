namespace Omnis.TicTacToe
{
    public class ToolkitTile : GridTile
    {
        #region Interfaces
        public override bool IsPointed
        {
            get => isPointed;
            set
            {
                if (Locked) return;
                isPointed = value;
                if (isPointed && pawns.Count > 0)
                    hintPawns.ForEach(hintPawn => hintPawn.Appear = true);
                else if (!Picked)
                    hintPawns.ForEach(hintPawn => hintPawn.Appear = false);
            }
        }

        public override bool Picked
        {
            get => picked;
            set
            {
                picked = value;
                pawns.ForEach(pawn => pawn.Highlight = picked);
                hintPawns.Find(hintPawn => hintPawn.Id.SameWith(new(Party.Hint, HintType.ToolInteracted))).Display = picked;
                if (!picked) hintPawns.ForEach(hintPawn => hintPawn.Appear = false);
            }
        }

        public System.Collections.IEnumerator LockInNextTurns(int turns)
        {
            var currPlayer = GameManager.Instance.Player;
            for (int i = 0; i < turns + 1; i++)
            {
                Interactable = false;
                yield return new UnityEngine.WaitUntil(() => GameManager.Instance.Player != currPlayer);
                yield return new UnityEngine.WaitUntil(() => GameManager.Instance.Player == currPlayer);
            }
            Interactable = true;
        }
        #endregion

        #region Functions
        protected override void OnStart()
        {
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.Tool), PawnInitState.DoNotAppear));
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.ToolInteracted), PawnInitState.Hide));
        }
        protected override void OnInteracted()
        {
            if (Locked) return;

            GameManager.Instance.Player.FirstTile = this;
        }
        #endregion
    }
}
