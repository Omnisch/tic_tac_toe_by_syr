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
        private bool playerMoveSucceeded;
        #endregion

        #region Interfaces
        #endregion

        #region Functions
        private IEnumerator BattleRoutine()
        {
            winnerParty = Party.Null;
            players = new();
            playerMoveSucceeded = true;

            yield return new WaitForFixedUpdate();

            Player player0 = new(chessboard.ToolkitSets[0]);
            Player player1 = new(chessboard.ToolkitSets[1]);
            players.Add(player0);
            players.Add(player1);
            CurrPlayerIndex = players.Count - 1;

            yield return CreateStartup();

            while (true)
            {
                if (playerMoveSucceeded)
                {
                    if (winnerParty != Party.Null) break;
                    postTurnCallback[CurrPlayerIndex].Invoke();
                    CurrPlayerIndex++;
                    if (CurrPlayerIndex == 0) yield return TimePass();
                }
                GameManager.Instance.Controllable = true;
                yield return new WaitUntil(() => GameManager.Instance.Player.SecondTile != null);
                GameManager.Instance.Controllable = false;
                yield return PlayerMove();
            }

            // settle
            yield return new WaitForSecondsRealtime(1.5f);
            winnerCallback?.Find(callback => callback.party == winnerParty).callback.Invoke();
        }
        private IEnumerator CreateStartup()
        {
            GameManager.Instance.Controllable = false;
            yield return chessboard.InitStartupByMode(gameMode);
            GameManager.Instance.Controllable = true;
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
    public struct PartyCallback
    {
        public Party party;
        public UnityEvent callback;
    }
}
