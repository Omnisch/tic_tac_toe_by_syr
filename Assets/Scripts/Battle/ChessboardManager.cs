using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class ChessboardManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private List<Transform> boardSetPivots;
        [SerializeField] private GameObject boardSetPrefab;
        [SerializeField] private List<Transform> toolbarPivots;
        [SerializeField] private GameObject toolbarPrefab;
        #endregion

        #region Fields
        private List<GridSet> boardSets;
        private List<GridSet> toolbarSets;
        #endregion

        #region Interfaces
        public List<BoardSet> BoardSets => new(boardSets.Select(set => set as BoardSet));
        public List<ToolbarSet> ToolbarSets => new(toolbarSets.Select(set => set as ToolbarSet));
        #endregion

        #region Functions
        private void InitChessboard()
        {
            boardSets = new();
            toolbarSets = new();
            boardSetPivots.ForEach(pivot => CreateGridSet(boardSetPrefab, pivot, boardSets));
            toolbarPivots.ForEach(pivot => CreateGridSet(toolbarPrefab, pivot, toolbarSets));
        }
        private void CreateGridSet(GameObject prefab, Transform pivot, List<GridSet> gridSet)
        {
            var go = Instantiate(prefab, pivot);
            gridSet.Add(go.GetComponent<GridSet>());
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            InitChessboard();
        }
        #endregion
    }
}
