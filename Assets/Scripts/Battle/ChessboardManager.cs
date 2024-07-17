using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class ChessboardManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private List<Transform> gridSetCenters;
        [SerializeField] private GameObject gridSetPrefab;
        #endregion

        #region Fields
        #endregion

        #region Interfaces
        #endregion

        #region Functions
        private void InitChessboard()
        {
            gridSetCenters.ForEach(center => CreateGridSet(center));
        }
        private void CreateGridSet(Transform center)
        {
            Instantiate(gridSetPrefab, center);
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
