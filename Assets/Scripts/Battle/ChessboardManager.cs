using System.Collections;
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
        [SerializeField] private List<Transform> toolkitPivots;
        [SerializeField] private GameObject toolkitPrefab;
        #endregion

        #region Fields
        private List<GridSet> boardSets;
        private List<GridSet> toolkitSets;
        #endregion

        #region Interfaces
        public List<GridSet> BoardSets => boardSets;
        public List<GridSet> ToolkitSets => toolkitSets;
        public IEnumerator InitStartupByMode(GameMode gameMode) => InitStartup(gameMode);
        public IEnumerator MultiPhases(GridTile tile)
        {
            for (int i = 0; i < boardSets.Count; i++)
            {
                for (int j = 0; j < boardSets[i].GridTiles.Count; j++)
                {
                    if (boardSets[i].GridTiles[j] == tile)
                    {
                        for (int remain = i + 1; remain < boardSets.Count; remain++)
                        {
                            if (boardSets[remain].GridTiles[j].Pawns.Count > 0) break;
                            yield return boardSets[remain].GridTiles[j].AddNextPhaseOf(tile);
                            tile = boardSets[remain].GridTiles[j];
                        }
                        break;
                    }
                }
            }
        }
        #endregion

        #region Functions
        private void InitChessboard()
        {
            boardSets = new();
            toolkitSets = new();
            boardSetPivots.ForEach(pivot => CreateGridSet(boardSetPrefab, pivot, boardSets));
            toolkitPivots.ForEach(pivot => CreateGridSet(toolkitPrefab, pivot, toolkitSets));
        }
        private void CreateGridSet(GameObject prefab, Transform pivot, List<GridSet> gridSet)
        {
            var go = Instantiate(prefab, pivot);
            gridSet.Add(go.GetComponent<GridSet>());
        }

        private IEnumerator InitStartup(GameMode mode)
        {
            var pawnIds = GameManager.Instance.Settings.startups.startupSets.Find(startup => startup.mode == mode).pawnIds;
            for (int i = 0; i < toolkitSets.Count; i++)
            {
                for (int j = 0; j < toolkitSets[0].GridTiles.Count; j++)
                {
                    toolkitSets[i].GridTiles[j].AddPawn(pawnIds[toolkitSets[0].GridTiles.Count * i + j]);
                    yield return new WaitForSeconds(0.4f);
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
