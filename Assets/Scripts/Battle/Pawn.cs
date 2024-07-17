using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class Pawn : MonoBehaviour
    {
        #region Serialized Fields
        #endregion

        #region Fields
        private bool doBreatheAnim;
        #endregion

        #region Interfaces
        public void Highlight()
        {

        }
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        private void Start()
        {
            doBreatheAnim = true;
        }
        private void Update()
        {
            if (doBreatheAnim && GameManager.Instance)
            {
                transform.localScale = GameManager.Instance.PawnBreatheScale * Vector3.one;
            }
        }
        #endregion
    }
}
