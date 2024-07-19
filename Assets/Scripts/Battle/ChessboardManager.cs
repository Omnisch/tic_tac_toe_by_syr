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

        public void InitStartupByName(string startupName) => InitStartup((GameMode)System.Enum.Parse(typeof(GameMode), startupName));
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

        private void InitStartup(GameMode mode)
        {
            var pawnIds = GameManager.Instance.Settings.startups.startupSets.First(startup => startup.mode == mode).pawnIds;
            for (int i = 0; i < toolbarSets.Count; i++)
            {
                for (int j = 0; j < toolbarSets[0].GridTiles.Count; j++)
                {
                    toolbarSets[i].GridTiles[j].SpawnPawn(pawnIds[toolbarSets[0].GridTiles.Count * i + j], true);
                }
            }
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
