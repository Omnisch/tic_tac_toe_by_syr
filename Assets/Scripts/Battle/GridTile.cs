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
        public virtual bool Selected
        {
            get => selected;
            set => selected = value;
        }

        public Pawn AddPawn(PawnId pawnId) => AddPawn(pawns, pawnId);
        public void RemovePawn(PawnId pawnId) => RemovePawn(pawns, pawnId);
        #endregion

        #region Functions
        protected Pawn AddPawn(List<Pawn> pawnList, PawnId pawnId)
        {
            var newPawn = Instantiate(pawnPrefab, transform).GetComponent<Pawn>();
            pawnList.Add(newPawn);
            newPawn.Id = pawnId;
            newPawn.transform.position += new Vector3(0, 0, -(hintPawns.Count + pawns.Count));
            return newPawn;
        }

        protected void RemovePawn(List<Pawn> pawnList, PawnId pawnId)
        {
            if (pawnList.Count == 0) return;
            var pawn = pawnList.Find(p => p.Id.SameWith(pawnId));
            pawn.DisappearAndDestroy();
            pawnList.Remove(pawn);
        }
        #endregion

        #region Unity Methods
        protected override void Start()
        {
            pawns = new();
            hintPawns = new();
            AddPawn(hintPawns, new(Party.Hint, hintType, false));

            base.Start();
        }
        #endregion
    }
}
