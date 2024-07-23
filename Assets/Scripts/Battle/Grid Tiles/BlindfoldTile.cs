namespace Omnis.TicTacToe
{
    public class BlindfoldTile : GridTile
    {
        #region Interfaces
        public override bool IsPointed
        {
            get => isPointed;
            set
            {
                if (Locked) return;
                isPointed = value;
                if (isPointed)
                {
                    if (Picked)
                        hintPawns.ForEach(hintPawn => hintPawn.Appear = true);
                    else
                        pawns.ForEach(pawn => pawn.Appear = true);
                }
                else
                {
                    if (Picked)
                        hintPawns.ForEach(hintPawn => hintPawn.Appear = false);
                    else
                        pawns.ForEach(pawn => pawn.Appear = false);
                }
            }
        }

        public override bool Picked
        {
            get => picked;
            set
            {
                if (Locked) return;
                picked = value;
                pawns.Find(pawn => pawn.Id.SameWith(new(Party.Tool, ToolType.Blindfold))).Display = picked;
                hintPawns.Find(hintPawn => hintPawn.Id.SameWith(new(Party.Hint, HintType.Confirm))).Display = picked;
                if (picked)
                {
                    hintPawns.ForEach(hintPawn => hintPawn.Appear = true);
                }
                else
                {
                    pawns.ForEach(pawn => pawn.Appear = false);
                    hintPawns.ForEach(hintPawn => hintPawn.Appear = false);
                }
            }
        }

        public override bool Locked
        {
            get => locked;
            set
            {
                locked = value;
                pawns.ForEach(pawn => pawn.Display = locked);
            }
        }
        #endregion

        #region Functions
        protected override void OnStart()
        {
            StartCoroutine(AddPawn(pawns, new(Party.Tool, ToolType.BlindfoldHover, BreathType.None), PawnInitState.DoNotAppear));
            StartCoroutine(AddPawn(pawns, new(Party.Tool, ToolType.Blindfold, BreathType.None), PawnInitState.Hide));
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.Confirm, BreathType.None), PawnInitState.Hide));
        }
        protected override void OnInteracted()
        {
            if (Locked) return;

            if (GameManager.Instance.Player.FirstTile != this)
                GameManager.Instance.Player.FirstTile = this;
            else
                GameManager.Instance.Player.SecondTile = this;
        }
        #endregion
    }
}
