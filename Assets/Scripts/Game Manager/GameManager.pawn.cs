using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class GameManager
    {
        #region Serialized Fields
        #endregion

        #region Fields
        private float pawnBreatheScale;
        #endregion

        #region Interfaces
        public float PawnBreatheScale => pawnBreatheScale;
        public Sprite GetSpriteOfParty(Party party, int type) => Instance.Settings.partySettings.Find(p => p.partyName == party).sprites[type];
        #endregion

        #region Functions
        private void UpdatePawnBreatheScale()
        {
            pawnBreatheScale = Instance.Settings.breathAmplitude * Mathf.Sin(2f * Mathf.PI * Instance.Settings.breathFrequency * Time.realtimeSinceStartup);
        }
        #endregion
    }
}
