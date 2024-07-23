using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Omnis.TicTacToe
{
    public partial class BattleManager : MonoBehaviour
    {
        #region Serialized Fields
        public GameMode gameMode;
        public ChessboardManager chessboard;
        [SerializeField] private ClockTile clock;
        [SerializeField] private List<GameModeCallback> beforePlayingStartCallback;
        [SerializeField] private List<GameModeCallback> beforePlayingFinishCallback;
        [SerializeField] private List<UnityEvent> postTurnCallback;
        [SerializeField] private List<PartyCallback> winnerCallback;
        #endregion

        #region Fields
        private Party winnerParty;
        private List<Player> players;
        private int currPlayerIndex;
        private int CurrPlayerIndex
        {
            get => currPlayerIndex;
            set
            {
                currPlayerIndex = value % players.Count;
                GameManager.Instance.Player = players[currPlayerIndex];
                players.ForEach(player => player.Active = player == GameManager.Instance.Player);
            }
        }
        #endregion

        #region Interfaces
        #endregion

        #region Functions
        private IEnumerator BattleRoutine()
        {
            winnerParty = Party.Null;
            players = new();

            yield return new WaitForFixedUpdate();

            Player player0 = new(chessboard.ToolkitSets[0]);
            Player player1 = new(chessboard.ToolkitSets[1]);
            players.Add(player0);
            players.Add(player1);
            CurrPlayerIndex = 0;
            clock.Locked = !GameManager.Instance.Settings.gameModeSettings.Find(gameModeSetting => gameModeSetting.modeName == gameMode).allowSkip;

            yield return CreateStartup();

            yield return ActionBeforePlaying();

            while (true)
            {
                yield return WaitUntilPlayerMoved();
                if (winnerParty != Party.Null) break;
                postTurnCallback[CurrPlayerIndex].Invoke();
                CurrPlayerIndex++;
                if (CurrPlayerIndex == 0) yield return TimePass();
            }

            // settle
            yield return new WaitForSecondsRealtime(1.5f);
            winnerCallback.Find(callback => callback.party == winnerParty).callback.Invoke();
        }
        private IEnumerator CreateStartup()
        {
            GameManager.Instance.Controllable = false;
            yield return chessboard.InitStartupByMode(gameMode);
            GameManager.Instance.Controllable = true;
        }
        private IEnumerator ActionBeforePlaying()
        {
            beforePlayingStartCallback.Find(callback => callback.modeName == gameMode).callback?.Invoke();
            switch (gameMode)
            {
                case GameMode.Relax:
                    break;
                case GameMode.NoDraw:
                    break;
                case GameMode.Blindfold:
                    chessboard.CreateBlindfoldSets();
                    chessboard.BoardSets.ForEach(boardSet => boardSet.Active = false);
                    yield return WaitUntilPlayerMoved();
                    break;
            }
            beforePlayingFinishCallback.Find(callback => callback.modeName == gameMode).callback?.Invoke();
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            StartCoroutine(BattleRoutine());
        }
        #endregion
    }

    [System.Serializable]
    public struct GameModeCallback
    {
        public GameMode modeName;
        public UnityEvent callback;
    }

    [System.Serializable]
    public struct PartyCallback
    {
        public Party party;
        public UnityEvent callback;
    }
}
