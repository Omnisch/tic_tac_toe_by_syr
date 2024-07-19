using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class GridTile : PointerBase
    {
        #region Serialized Fields
        [SerializeField] private GameObject pawnPrefab;
        [SerializeField] private HintType hintType;
        #endregion

        #region Fields
        private List<Pawn> pawns;
        #endregion

        #region Interfaces
        public override bool IsPointed
        {
            get => isPointed;
            set
            {
                isPointed = value;
                if (isPointed) pawns[0].Appear();
                else pawns[0].Disappear();
            }
        }

        public Pawn GetPawn(int i) => pawns[i];

        public void AddPawn(PawnId pawnId, bool instantAppear = false)
        {
            if (!pawnId.parent) pawnId.parent = transform;
            var newPawn = Instantiate(pawnPrefab, pawnId.parent).GetComponent<Pawn>();
            pawns.Add(newPawn);
            newPawn.Id = pawnId;
            newPawn.transform.position += new Vector3(0, 0, -pawns.Count);
            if (instantAppear) newPawn.Appear();
        }
        public void RemovePawn(Pawn pawn) => pawns.Remove(pawn);
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        private void Start()
        {
            pawns = new();
            AddPawn(new PawnId(Party.Hint, hintType, transform, false));
        }
        #endregion

        #region Handlers
        protected override void OnInteract()
        {
            Debug.Log("Clicked on grid tile.");
        }
        #endregion
    }
}
