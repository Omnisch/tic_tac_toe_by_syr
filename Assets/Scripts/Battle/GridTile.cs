using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public abstract class GridTile : PointerBase
    {
        #region Serialized Fields
        [SerializeField] private GameObject pawnPrefab;
        [SerializeField] private HintType hintType;
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

        public void AddPawn(PawnId pawnId, bool showInstantly = true) => AddPawn(pawns, pawnId, showInstantly);
        public void CopyPawnsFrom(GridTile other) => other.Pawns.ForEach(pawn => this.AddPawn(pawn.Id));
        //public void NextPhase() => 
        public void AddNextPhaseOf(GridTile other) => other.Pawns.ForEach(pawn => this.AddPawn(new(pawn.Id.party, pawn.Id.NextType)));
        public void RemoveAll() => RemoveAll(ref pawns);
        #endregion

        #region Functions
        protected void AddPawn(List<Pawn> pawnList, PawnId pawnId, bool showInstantly)
        {
            var newPawn = Instantiate(pawnPrefab, transform).GetComponent<Pawn>();
            pawnList.Add(newPawn);
            newPawn.Id = pawnId;
            newPawn.transform.position -= new Vector3(0, 0, 1 + (hintPawns.Count + pawns.Count));
            if (showInstantly)
                StartCoroutine(newPawn.Appear());
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
            AddPawn(hintPawns, new(Party.Hint, hintType, false), false);

            base.Start();
        }
        #endregion
    }
}
