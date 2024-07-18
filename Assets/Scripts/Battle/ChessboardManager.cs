using System.Collections.Generic;
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
        private List<BoardSet> boardSets;
        private List<ToolbarSet> toolbarSets;
        #endregion

        #region Interfaces
        #endregion

        #region Functions
        private void InitChessboard()
        {
            boardSetPivots.ForEach(pivot => CreateGridSet(boardSetPrefab, pivot));
            toolbarPivots.ForEach(pivot => CreateGridSet(toolbarPrefab, pivot));
        }
        private void CreateGridSet(GameObject prefab, Transform pivot)
        {
            Instantiate(prefab, pivot);
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
