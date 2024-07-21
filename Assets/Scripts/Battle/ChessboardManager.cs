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
        public IEnumerator AddMultiPhases(GridTile tile)
        {
            for (int i = 0; i < BoardSets.Count; i++)
                for (int j = 0; j < BoardSets[i].GridTiles.Count; j++)
                    if (BoardSets[i].GridTiles[j] == tile)
                    {
                        for (int rest = i + 1; rest < BoardSets.Count; rest++)
                        {
                            if (BoardSets[rest].GridTiles[j].Pawns.Count > 0) yield break;
                            yield return BoardSets[rest].GridTiles[j].AddNextPhaseOf(tile);
                            tile = BoardSets[rest].GridTiles[j];
                        }
                        yield break;
                    }
        }
        public IEnumerator RemoveMultiPhases(GridTile tile)
        {
            for (int i = 0; i < BoardSets.Count; i++)
                for (int j = 0; j < BoardSets[i].GridTiles.Count; j++)
                    if (BoardSets[i].GridTiles[j] == tile)
                    {
                        for (int rest = i + 1; rest < BoardSets.Count; rest++)
                        {
                            GridTile currTile = BoardSets[rest].GridTiles[j];
                            if (currTile.Pawns.Count == 0) yield break;
                            if (currTile.Pawns[0].Id.party == tile.Pawns[0].Id.party &&
                                currTile.Pawns[0].Id.type == tile.Pawns[0].Id.NextType)
                            {
                                yield return currTile.RemoveAllPawns();
                                tile = currTile;
                            }
                            else
                                yield break;
                        }
                        yield break;
                    }
        }
        #endregion

        #region Functions
        private void InitChessboard()
        {
            boardSets = new();
            toolkitSets = new();
            boardSetPivots.ForEach(pivot => CreateGridSet(boardSetPrefab, pivot, BoardSets));
            toolkitPivots.ForEach(pivot => CreateGridSet(toolkitPrefab, pivot, ToolkitSets));
        }
        private void CreateGridSet(GameObject prefab, Transform pivot, List<GridSet> gridSet)
        {
            var go = Instantiate(prefab, pivot);
            gridSet.Add(go.GetComponent<GridSet>());
        }

        private IEnumerator InitStartup(GameMode mode)
        {
            var pawnIds = GameManager.Instance.Settings.startups.startupSets.Find(startup => startup.mode == mode).pawnIds;
            for (int i = 0; i < ToolkitSets.Count; i++)
            {
                for (int j = 0; j < ToolkitSets[0].GridTiles.Count; j++)
                {
                    ToolkitSets[i].GridTiles[j].AddPawn(pawnIds[ToolkitSets[0].GridTiles.Count * i + j]);
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
