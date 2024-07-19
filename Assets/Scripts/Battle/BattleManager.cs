using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Omnis.TicTacToe
{
    public partial class BattleManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private ChessboardManager chessboard;
        [SerializeField] private List<UnityEvent> stages;
        #endregion

        #region Fields
        private int stageIndex;
        #endregion

        #region Interfaces
        public Party WinningParty { set => Settle(value); }
        public void FinishStage()
        {
            NextStage();
        }
        #endregion

        #region Functions
        private void InitBattle()
        {
            stageIndex = -1;
            StartCoroutine(StartBattle());
        }

        private IEnumerator StartBattle()
        {
            yield return new WaitForFixedUpdate();

            NextStage();
        }

        private void NextStage()
        {
            if (stages.Count == 0)
                return;

            if (++stageIndex >= stages.Count)
                stageIndex = 0;

            stages[stageIndex]?.Invoke();
        }

        private void Settle(Party winningParty)
        {

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
