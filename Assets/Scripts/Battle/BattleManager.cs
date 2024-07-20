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
        public List<Player> players;
        #endregion

        #region Fields
        #endregion

        #region Interfaces
        #endregion

        #region Functions
        private void InitBattle()
        {
            players = new();
            Player player1 = new(chessboard.ToolkitSets[0]);
            Player player2 = new(chessboard.ToolkitSets[1]);
            players.Add(player1);
            players.Add(player2);
            StartCoroutine(CreateStartup());
        }
        private IEnumerator CreateStartup()
        {
            yield return new WaitForFixedUpdate();
            GameManager.Instance.Player.CanInteract = false;
            yield return chessboard.InitStartupByMode(GameMode.Standard);
            GameManager.Instance.Player.CanInteract = true;
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            InitBattle();
        }
        #endregion
    }
}
