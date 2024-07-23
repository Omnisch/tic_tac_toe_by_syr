namespace Omnis.TicTacToe
{
    public class BoardTile : GridTile
    {
        #region Interfaces
        public override bool IsPointed
        {
            get => isPointed;
            set
            {
                if (Locked) return;
                isPointed = value;
                hintPawns.ForEach(hintPawn => hintPawn.Appear = isPointed && GameManager.Instance.Player.FirstTile);
            }
        }

        public override bool Picked
        {
            get => picked;
            set
            {
                if (pawns.Count > 0) return;
                picked = value;
            }
        }
        public override bool Locked
        {
            get => locked;
            set
            {
                locked = value;
                if (locked) StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.Lock, BreathType.Rolling), PawnInitState.Concentrate));
            }
        }
        #endregion

        #region Functions
        protected override void OnStart()
        {
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.Board), PawnInitState.DoNotAppear));
        }
        protected override void OnInteracted()
        {
            if (Locked) return;

            GameManager.Instance.Player.SecondTile = this;
        }
        #endregion
    }
}
