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
                        hintPawns.ForEach(hintPawn => hintPawn.Cover(1.05f));
                    else
                        pawns.ForEach(pawn => pawn.Cover(1.05f));
                }
                else
                {
                    if (Picked)
                        hintPawns.ForEach(hintPawn => hintPawn.Uncover(1.05f));
                    else
                        pawns.ForEach(pawn => pawn.Uncover(1.05f));
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
                    hintPawns.ForEach(hintPawn => hintPawn.Cover(1.05f));
                }
                else
                {
                    pawns.ForEach(pawn => pawn.Uncover(1.05f));
                    hintPawns.ForEach(hintPawn => hintPawn.Uncover(1.05f));
                }
            }
        }

        public override bool Locked
        {
            get => locked;
            set
            {
                locked = value;
                pawns.Find(pawn => pawn.Id.SameWith(new(Party.Tool, ToolType.Blindfold))).Display = locked;
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
