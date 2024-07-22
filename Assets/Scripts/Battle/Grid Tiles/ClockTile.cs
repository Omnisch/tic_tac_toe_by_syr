using UnityEngine;

namespace Omnis.TicTacToe
{
    public class ClockTile : GridTile
    {
        #region Serialized Fields
        [SerializeField] private RuntimeAnimatorController animatorController;
        #endregion

        #region Interfaces
        public override bool IsPointed
        {
            get => isPointed;
            set
            {
                if (Locked) return;
                isPointed = value;
                if (isPointed)
                    hintPawns.ForEach(hintPawn => hintPawn.StartCoroutine(hintPawn.Appear()));
                else
                    hintPawns.ForEach(hintPawn => hintPawn.StartCoroutine(hintPawn.Disappear()));
            }
        }
        public void TriggerTick()
        {
            if (pawns.Count > 0)
            {
                if (!pawns[0].GetComponent<Animator>())
                    pawns[0].gameObject.AddComponent<Animator>().runtimeAnimatorController = animatorController;
                pawns[0].GetComponent<UnityEngine.Animator>().SetTrigger("Tick");
            }
        }
        #endregion

        #region Functions
        protected override void OnStart()
        {
            StartCoroutine(AddPawn(pawns, new(Party.Tool, ToolType.Clock, BreathType.Rolling), PawnInitState.Appear));
            StartCoroutine(AddPawn(hintPawns, new(Party.Hint, HintType.Skip, BreathType.None), PawnInitState.DoNotAppear));
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
