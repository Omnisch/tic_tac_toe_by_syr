namespace Omnis.TicTacToe
{
    public class BoardTile : GridTile
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
                if (Locked) return;
                isPointed = value;
                if (isPointed && GameManager.Instance.Player.FirstTile)
                    hintPawns.ForEach(hintPawn => hintPawn.StartCoroutine(hintPawn.Appear()));
                else
                    hintPawns.ForEach(hintPawn => hintPawn.StartCoroutine(hintPawn.Disappear()));
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
        #endregion

        #region Functions
        protected override void OnStart()
        {
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.Board), PawnInitState.DoNotAppear));
        }
        #endregion

        #region Handlers
        protected override void OnInteract()
        {
            if (Locked || !Interactable) return;

            GameManager.Instance.Player.SecondTile = this;
        }
        #endregion
    }
}
