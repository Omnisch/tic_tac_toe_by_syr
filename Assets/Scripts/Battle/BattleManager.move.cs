using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class BattleManager
    {
        #region Fields
        private bool playerMoveSucceeded;
        #endregion

        #region Functions
        private IEnumerator WaitUntilPlayerMoved()
        {
            playerMoveSucceeded = false;
            while (!playerMoveSucceeded)
            {
                GameManager.Instance.Controllable = true;
                yield return new WaitUntil(() => GameManager.Instance.Player.SecondTile != null);
                GameManager.Instance.Controllable = false;
                yield return PlayerMove();
            }
            clock.TriggerTick();
        }

        private IEnumerator PlayerMove()
        {
            Player player = GameManager.Instance.Player;
            playerMoveSucceeded = true;
            if (player.FirstTile == null) yield break;

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
                                yield return Dig(player, player.SecondTile);
                                break;
                            case ToolType.Hammer1:
                            case ToolType.Hammer2:
                                if (player.FirstTile.TryGetComponent<ToolkitTile>(out var toolkitTile))
                                    toolkitTile.StartCoroutine(toolkitTile.LockInNextTurns(1));
                                yield return Erase(player.SecondTile);
                                break;
                            case ToolType.Clock:
                                if (GameManager.Instance.Settings.gameModeSettings.Find(settings => settings.modeName == gameMode).reloadWhenSkip)
                                    StartCoroutine(chessboard.ReloadToolkit(CurrPlayerIndex));
                                break;
                            case ToolType.BlindfoldHover:
                                if (player.FirstTile != player.SecondTile)
                                {
                                    playerMoveSucceeded = false;
                                    break;
                                }
                                chessboard.BoardSets.ForEach(boardSet => boardSet.Active = true);
                                chessboard.BlindfoldSetIndex = chessboard.BlindfoldSets[0].GridTiles.FindIndex(tile => tile == player.FirstTile);
                                chessboard.BlindfoldSets.ForEach(set => set.GridTiles.ForEach(tile => tile.Locked = true));
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
            AudioManager.Instance.PlaySE(CurrPlayerIndex == 0 ? SoundEffectName.NatureKick : SoundEffectName.ArtifactKick);
            List<Pawn> tempPawnList = fromTile.Pawns;
            fromTile.StartCoroutine(fromTile.RemoveAllPawns());
            yield return toTile.CopyPawnsFrom(tempPawnList);
        }

        private IEnumerator Dig(Player player, GridTile toDig)
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
                if (boardSet.GridTiles.Where(boardTile => boardTile.Pawns.Count == 0).Count() == boardSet.GridTiles.Count)
                    continue;
                foreach (var boardTile in boardSet.GridTiles)
                {
                    if (boardTile == boardSet.GridTiles.Last(tile => tile.Pawns.Count > 0))
                        yield return boardTile.NextPhase();
                    else
                        boardTile.StartCoroutine(boardTile.NextPhase());
                }
            }
            if (gameMode == GameMode.Blindfold) chessboard.BlindfoldSetIndex++;
        }
        #endregion
    }
}
