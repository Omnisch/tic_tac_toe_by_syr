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
                    hintPawns.ForEach(hintPawn => hintPawn.StartCoroutine(hintPawn.Appear()));
                else if (!picked)
                    hintPawns.ForEach(hintPawn => hintPawn.StartCoroutine(hintPawn.Disappear()));
            }
        }

        public override bool Picked
        {
            get => picked;
            set
            {
                picked = value;
                if (picked)
                {
                    pawns.ForEach(pawn => pawn.StartCoroutine(pawn.Highlight()));
                    hintPawns.Find(pawn => pawn.Id.SameWith(new(Party.Hint, HintType.ToolInteracted))).Show();
                }
                else
                {
                    pawns.ForEach(pawn => pawn.StartCoroutine(pawn.ToNeutralScale()));
                    hintPawns.ForEach(hintPawn => hintPawn.StartCoroutine(hintPawn.Disappear()));
                    hintPawns.Find(pawn => pawn.Id.SameWith(new(Party.Hint, HintType.ToolInteracted))).Hide();
                }
            }
        }
        #endregion

        #region Functions
        protected override void OnStart()
        {
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.Tool), PawnInitState.DoNotAppear));
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.ToolInteracted), PawnInitState.Transparent));
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