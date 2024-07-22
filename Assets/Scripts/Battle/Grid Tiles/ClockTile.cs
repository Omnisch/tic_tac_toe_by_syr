namespace Omnis.TicTacToe
{
    public class ClockTile : GridTile
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
                    pawns.ForEach(pawn => pawn.StartCoroutine(pawn.Highlight()));
                else
                    pawns.ForEach(pawn => pawn.StartCoroutine(pawn.ToNeutralScale()));
            }
        }
        public void TriggerTick()
        {
            if (pawns.Count > 0)
                pawns[0].GetComponent<UnityEngine.Animator>().SetTrigger("Tick");
        }
        #endregion

        #region Functions
        protected override void OnStart()
        {
            StartCoroutine(AddPawn(pawns, new(Party.Tool, ToolType.Clock, BreathType.Rolling), PawnInitState.Appear));
        }
        #endregion

        #region Handlers
        protected override void OnInteract()
        {
            if (Locked || !Interactable) return;

            GameManager.Instance.Player.FirstTile = this;
            GameManager.Instance.Player.SecondTile = this;
        }
        #endregion
    }
}
