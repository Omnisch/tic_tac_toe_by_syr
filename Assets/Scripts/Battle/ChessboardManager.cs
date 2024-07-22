using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private List<Transform> blindfoldPivots;
        [SerializeField] private GameObject blindfoldPrefab;
        [SerializeField] private List<GridSetCallback> finishBoardCallback;
        #endregion

        #region Fields
        private List<GridSet> boardSets;
        private List<GridSet> toolkitSets;
        private List<GridSet> blindfoldSets;
        private List<Party> finishBoardMarks;
        private int blindfoldSetIndex;
        #endregion

        #region Interfaces
        public List<GridSet> BoardSets => boardSets;
        public List<GridSet> ToolkitSets => toolkitSets;
        public List<GridSet> BlindfoldSets => blindfoldSets;

        public int BlindfoldSetIndex
        {
            get => blindfoldSetIndex;
            set
            {
                if (value < 0 || BoardSets.Count == 0) return;
                boardSets[blindfoldSetIndex].Active = true;
                blindfoldSetIndex = value % boardSets.Count;
                boardSets[blindfoldSetIndex].Active = false;
            }
        }

        public IEnumerator InitStartupByMode(GameMode gameMode) => InitStartup(gameMode);

        public IEnumerator AddMultiPhases(GridTile tile, System.Func<bool> stopCase = null)
        {
            for (int i = 0; i < BoardSets.Count; i++)
                for (int j = 0; j < BoardSets[i].GridTiles.Count; j++)
                    if (BoardSets[i].GridTiles[j] == tile)
                    {
                        for (int rest = i + 1; rest < BoardSets.Count; rest++)
                        {
                            GridTile currTile = BoardSets[rest].GridTiles[j];
                            if (currTile.Pawns.Count > 0) yield break;
                            yield return currTile.AddNextPhaseOf(tile);
                            if (stopCase?.Invoke() ?? false) yield break;
                            tile = currTile;
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
                            if (currTile.Locked) yield break;
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

        public bool CheckMove(out Party winnerParty)
        {
            winnerParty = Party.Null;

            bool hasFinishBoard = CheckFinishBoard();

            Dictionary<Party, int> scoreboard = new();
            foreach (var finishBoard in finishBoardMarks)
            {
                if (!scoreboard.TryAdd(finishBoard, 1))
                    scoreboard[finishBoard]++;
                if (scoreboard[finishBoard] >= (finishBoardMarks.Count + 1) / 2)
                {
                    winnerParty = finishBoard;
                    break;
                }
            }

            return hasFinishBoard;
        }
        #endregion

        #region Functions
        private void InitChessboard()
        {
            boardSets = new();
            toolkitSets = new();
            blindfoldSets = new();
            finishBoardMarks = new();
            BlindfoldSetIndex = -1;

            boardSetPivots.ForEach(pivot => CreateGridSet(boardSetPrefab, pivot, BoardSets));
            toolkitPivots.ForEach(pivot => CreateGridSet(toolkitPrefab, pivot, ToolkitSets));
            blindfoldPivots.ForEach(pivot => CreateGridSet(blindfoldPrefab, pivot, BlindfoldSets));

            BoardSets.ForEach(boardset => finishBoardMarks.Add(Party.Null));
        }
        private void CreateGridSet(GameObject prefab, Transform pivot, List<GridSet> gridSets)
        {
            var go = Instantiate(prefab, pivot);
            gridSets.Add(go.GetComponent<GridSet>());
        }

        private IEnumerator InitStartup(GameMode mode)
        {
            var pawnIds = GameManager.Instance.Settings.gameModeSettings.Find(gameMode => gameMode.modeName == mode).startup;
            for (int i = 0; i < ToolkitSets.Count; i++)
            {
                for (int j = 0; j < ToolkitSets[0].GridTiles.Count; j++)
                {
                    ToolkitSets[i].GridTiles[j].AddPawn(pawnIds[ToolkitSets[0].GridTiles.Count * i + j]);
                    yield return new WaitForSeconds(0.4f);
                }
            }
        }

        private bool CheckFinishBoard()
        {
            bool hasFinishBoard = false;
            List<WhatCountAsARow> rowList = GameManager.Instance.Settings.whatCountAsARow;
            for (int i = 0; i < BoardSets.Count; i++)
            {
                if (finishBoardMarks[i] != Party.Null) continue;
                for (int rowIndex = 0; rowIndex < rowList.Count; rowIndex++)
                {
                    List<PawnId> pawnIdInARow = GetOneRow(BoardSets[i], rowList[rowIndex], out bool isFilled);
                    if (isFilled && IsThreeInARow(pawnIdInARow))
                    {
                        for (int j = 0; j < rowList[rowIndex].indices.Count; j++)
                            BoardSets[i].GridTiles[rowList[rowIndex].indices[j]].Locked = true;
                        finishBoardMarks[i] = pawnIdInARow[0].party;
                        hasFinishBoard = true;
                        finishBoardCallback[i].partyCallbacks.Find(callback => callback.party == finishBoardMarks[i]).callback.Invoke();
                    }
                }
            }
            return hasFinishBoard;
        }
        private List<PawnId> GetOneRow(GridSet boardSet, WhatCountAsARow row, out bool isFilled)
        {
            List<PawnId> output = new();
            isFilled = true;
            for (int i = 0; i < row.indices.Count; i++)
            {
                if (boardSet.GridTiles[row.indices[i]].Pawns.Count == 0)
                {
                    isFilled = false;
                    continue;
                }
                output.Add(boardSet.GridTiles[row.indices[i]].Pawns[0].Id);
            }
            return output;
        }
        private bool IsThreeInARow(List<PawnId> pawnIdInARow)
        {
            if (pawnIdInARow.Count == 0) return false;
            for (int successor = 1; successor < pawnIdInARow.Count; successor++)
            {
                if (!pawnIdInARow[0].SameWith(pawnIdInARow[successor]))
                    return false;
            }
            return true;
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            InitChessboard();
        }
        #endregion

        [System.Serializable]
        private struct GridSetCallback
        {
            public List<PartyCallback> partyCallbacks;
        }
    }
}
