using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class BattleManager
    {
        #region Serialized Fields
        #endregion

        #region Fields
        #endregion

        #region Functions
        private IEnumerator PlayerMove()
        {
            Player player = GameManager.Instance.Player;
            switch (player.FirstTile.Pawns[0].Id.party)
            {
                case Party.Nature:
                    yield return Transport();
                    yield return chessboard.MultiPhases(player.SecondTile);
                    break;
                case Party.Artifact:
                    yield return Transport();
                    yield return chessboard.MultiPhases(player.SecondTile);
                    break;
                case Party.Tool:
                    {
                        switch ((ToolType)player.FirstTile.Pawns[0].Id.type)
                        {
                            case ToolType.Shovel1:
                                DigBack(player, player.FirstTile);
                                break;
                            case ToolType.Shovel2:
                                DigBack(player, player.FirstTile);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
            DeselectAll();
            yield return new WaitForEndOfFrame();
        }

        private void DeselectAll()
        {
            Player player = GameManager.Instance.Player;
            player.SecondTile = null;
            player.FirstTile = null;
        }

        private IEnumerator Transport()
        {
            Player player = GameManager.Instance.Player;
            yield return player.SecondTile.CopyPawnsFrom(player.FirstTile);
            player.FirstTile.RemoveAll();
        }

        private void DigBack(Player player, GridTile toDig)
        {

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
