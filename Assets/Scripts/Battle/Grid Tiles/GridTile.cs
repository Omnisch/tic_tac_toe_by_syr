using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class GridTile : PointerBase
    {
        #region Fields
        protected List<Pawn> pawns;
        protected List<Pawn> hintPawns;
        protected bool picked;
        protected bool locked;
        #endregion

        #region Interfaces
        public List<Pawn> Pawns => pawns;
        public override bool Interactable
        {
            get => interactable;
            set
            {
                if (locked) return;
                interactable = value;
                if (interactable)
                    pawns.ForEach(pawn => pawn.SetAlpha(1f));
                else
                    pawns.ForEach(pawn => pawn.SetAlpha(0.5f));
            }
        }
        public virtual bool Picked { get => picked; set => picked = value; }
        public virtual bool Locked { get => locked; set => locked = value; }

        public void AddPawn(PawnId pawnId, PawnInitState pawnInitState = PawnInitState.Appear) => StartCoroutine(AddPawn(pawns, pawnId, pawnInitState));
        public IEnumerator AddPawnRoutine(PawnId pawnId)
        {
            yield return AddPawn(pawns, pawnId, PawnInitState.Appear);
        }
        public IEnumerator CopyPawnsFrom(List<Pawn> pawnList)
        {
            foreach (var pawn in pawnList)
                yield return AddPawnRoutine(pawn.Id);
        }
        public IEnumerator Replace(List<PawnId> newPawnIds)
        {
            for (int i = 0; i < Mathf.Max(pawns.Count, newPawnIds.Count) - 1; i++)
            {
                if (i < pawns.Count)
                {
                    if (i < newPawnIds.Count) pawns[i].StartCoroutine(pawns[i].ChangeIdAndRefresh(newPawnIds[i]));
                    else pawns[i].StartCoroutine(pawns[i].IDisappearAndDestroy());
                }
                else
                    AddPawn(newPawnIds[i]);
            }
            {
                int i = Mathf.Max(pawns.Count, newPawnIds.Count) - 1;
                if (i < pawns.Count)
                {
                    if (i < newPawnIds.Count) yield return pawns[i].ChangeIdAndRefresh(newPawnIds[i]);
                    else yield return pawns[i].IDisappearAndDestroy();
                }
                else
                    yield return AddPawnRoutine(newPawnIds[i]);
            }
        }
        public IEnumerator NextPhase()
        {
            foreach (var pawn in pawns)
            {
                PawnId newId = new(pawn.Id.party, pawn.Id.NextType, pawn.Id.breathType);
                if (pawn == pawns.Last())
                    yield return pawn.ChangeIdAndRefresh(newId);
                else
                    pawn.StartCoroutine(pawn.ChangeIdAndRefresh(newId));
            }
        }
        public IEnumerator AddNextPhaseOf(GridTile other)
        {
            foreach (var pawn in other.pawns)
            {
                if (pawn == other.pawns.Last())
                    yield return AddPawnRoutine(new(pawn.Id.party, pawn.Id.NextType, pawn.Id.breathType));
                else
                    AddPawn(new(pawn.Id.party, pawn.Id.NextType, pawn.Id.breathType));
            }
        }
        public IEnumerator RemoveAllPawns()
        {
            if (pawns.Count == 0) yield break;
            foreach (var pawn in pawns)
            {
                if (pawn == pawns.Last())
                    yield return pawn.IDisappearAndDestroy();
                else
                    pawn.StartCoroutine(pawn.IDisappearAndDestroy());
            }
        }
        // only used in OnDestroy() of class Pawn
        public void SignOut(Pawn pawn) => pawns.Remove(pawn);
        #endregion

        #region Functions
        protected virtual void OnStart() {}

        protected IEnumerator AddPawn(List<Pawn> pawnList, PawnId pawnId, PawnInitState pawnInitState)
        {
            var newPawn = Instantiate(GameManager.Instance.Settings.pawnPrefab, transform).GetComponent<Pawn>();
            pawnList.Add(newPawn);
            newPawn.Id = pawnId;
            newPawn.transform.position -= new Vector3(0, 0, 1 + (hintPawns.Count + pawns.Count));
            switch (pawnInitState)
            {
                case PawnInitState.Appear:
                    yield return newPawn.IAppear();
                    break;
                case PawnInitState.Concentrate:
                    yield return newPawn.ICover(2f);
                    break;
                case PawnInitState.DoNotAppear:
                    break;
                case PawnInitState.Hide:
                    newPawn.Display = false;
                    break;
            }
        }
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            pawns = new();
            hintPawns = new();
            locked = false;
            OnStart();
            base.Start();
        }
        #endregion
    }
}
