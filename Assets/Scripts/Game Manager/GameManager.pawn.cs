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
        public Sprite GetSpriteOfParty(Party party, int pawnStage) => Instance.Settings.partySprites.First(p => p.partyName == party).sprites[pawnStage];
        #endregion

        #region Functions
        private void UpdatePawnBreatheScale()
        {
            pawnBreatheScale = 1f + Instance.Settings.amplitude * Mathf.Sin(2f * Mathf.PI * Instance.Settings.frequency * Time.realtimeSinceStartup);
        }
        #endregion
    }
}
