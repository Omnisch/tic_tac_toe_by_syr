using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public abstract class GridTile : PointerBase
    {
        #region Serialized Fields
        [SerializeField] private GameObject pawnPrefab;
        #endregion

        #region Fields
        protected List<Pawn> pawns;
        protected List<Pawn> hintPawns;
        protected bool selected;
        #endregion

        #region Interfaces
        public List<Pawn> Pawns => pawns;
        public override bool Interactable
        {
            get => interactable;
            set
            {
                interactable = value;
                if (interactable)
                    pawns.ForEach(pawn => pawn.Show());
                else
                    pawns.ForEach(pawn => pawn.HalfShow());
            }
        }
        public virtual bool Selected
        {
            get => selected;
            set => selected = value;
        }

        public void AddPawn(PawnId pawnId, PawnInitState pawnInitState = PawnInitState.Appear) => StartCoroutine(AddPawn(pawns, pawnId, pawnInitState));
        public IEnumerator AddPawnRoutine(PawnId pawnId)
        {
            yield return AddPawn(pawns, pawnId, PawnInitState.Appear);
        }
        public IEnumerator CopyPawnsFrom(GridTile other)
        {
            foreach (var otherPawn in other.Pawns)
                yield return AddPawnRoutine(otherPawn.Id);
        }
        public IEnumerator NextPhase()
        {
            foreach (var pawn in Pawns)
            {
                PawnId newId = new(pawn.Id.party, pawn.Id.NextType, pawn.Id.canBreathe);
                if (pawn == Pawns.Last())
                    yield return pawn.ChangeIdAndRefresh(newId);
                else
                    pawn.StartCoroutine(pawn.ChangeIdAndRefresh(newId));
            }
        }
        public IEnumerator AddNextPhaseOf(GridTile other)
        {
            foreach (var pawn in other.Pawns)
            {
                if (pawn == other.Pawns.Last())
                    yield return AddPawnRoutine(new(pawn.Id.party, pawn.Id.NextType));
                else
                    StartCoroutine(AddPawnRoutine(new(pawn.Id.party, pawn.Id.NextType)));
            }
        }
        public void RemoveAll() => RemoveAll(ref pawns);
        #endregion

        #region Functions
        protected virtual void OnStart() {}

        protected IEnumerator AddPawn(List<Pawn> pawnList, PawnId pawnId, PawnInitState pawnInitState)
        {
            var newPawn = Instantiate(pawnPrefab, transform).GetComponent<Pawn>();
            pawnList.Add(newPawn);
            newPawn.Id = pawnId;
            newPawn.transform.position -= new Vector3(0, 0, 1 + (hintPawns.Count + pawns.Count));
            switch (pawnInitState)
            {
                case PawnInitState.Appear:
                    yield return newPawn.Appear();
                    break;
                case PawnInitState.DoNotAppear:
                    break;
                case PawnInitState.Transparent:
                    newPawn.Hide();
                    break;
            }
        }

        protected void RemoveAll(ref List<Pawn> pawnList)
        {
            if (pawnList.Count == 0) return;
            pawnList.ForEach(pawn => StartCoroutine(pawn.DisappearAndDestroy()));
            pawnList = new();
        }
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            pawns = new();
            hintPawns = new();
            OnStart();
            base.Start();
        }
        #endregion
    }
}
