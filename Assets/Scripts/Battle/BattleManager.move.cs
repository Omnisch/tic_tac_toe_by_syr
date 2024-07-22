using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class BattleManager
    {
        #region Functions
        private IEnumerator PlayerMove()
        {
            Player player = GameManager.Instance.Player;
            playerMoveSucceeded = true;
            switch (player.FirstTile.Pawns[0].Id.party)
            {
                case Party.Nature:
                case Party.Artifact:
                    yield return Transport(player.FirstTile, player.SecondTile);
                    if (!chessboard.CheckMove(out winnerParty))
                        yield return chessboard.AddMultiPhases(player.SecondTile, () => chessboard.CheckMove(out winnerParty));
                    break;
                case Party.Tool:
                    {
                        switch ((ToolType)player.FirstTile.Pawns[0].Id.type)
                        {
                            case ToolType.Shovel1:
                            case ToolType.Shovel2:
                                yield return DigBack(player, player.SecondTile);
                                break;
                            case ToolType.Hammer1:
                            case ToolType.Hammer2:
                                yield return Erase(player.SecondTile);
                                break;
                        }
                    }
                    break;
            }
            DeselectAll();
            yield return new WaitForEndOfFrame();
        }

        private void DeselectAll()
        {
            Player player = GameManager.Instance.Player;
            player.FirstTile = null;
            player.SecondTile = null;
        }

        private IEnumerator Transport(GridTile fromTile, GridTile toTile)
        {
            List<Pawn> tempPawnList = fromTile.Pawns;
            fromTile.StartCoroutine(fromTile.RemoveAllPawns());
            yield return toTile.CopyPawnsFrom(tempPawnList);
        }

        private IEnumerator DigBack(Player player, GridTile toDig)
        {
            GridTile toPut = player.Toolkit.FindFirstAvailable();
            if (!toPut)
            {
                playerMoveSucceeded = false;
                yield break;
            }
            else
            {
                yield return Transport(toDig, toPut);
                yield return chessboard.RemoveMultiPhases(toDig);
            }
        }

        private IEnumerator Erase(GridTile toErase)
        {
            yield return toErase.RemoveAllPawns();
            yield return chessboard.RemoveMultiPhases(toErase);
        }    

        private IEnumerator TimePass()
        {
            foreach (var boardSet in chessboard.BoardSets)
            {
                foreach (var boardTile in boardSet.GridTiles)
                {
                    if (boardSet == chessboard.BoardSets.Last() && boardTile == boardSet.GridTiles.Last())
                        yield return boardTile.NextPhase();
                    else
                        boardTile.StartCoroutine(boardTile.NextPhase());
                }
            }
        }
        #endregion
    }
}
