using UnityEngine;
using UnityEngine.Events;

namespace Omnis.TicTacToe
{
    public class BattleManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private UnityEvent[] stages;
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

            NextStage();
        }
        private void NextStage()
        {
            if (stages.Length == 0)
                return;

            if (++stageIndex >= stages.Length)
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
