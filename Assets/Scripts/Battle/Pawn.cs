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
        private bool isHidden;
        private bool doBreatheAnim;
        #endregion

        #region Interfaces
        public void Appear()
        {
            StartCoroutine(IAppear());
        }

        public void Disappear()
        {
            StartCoroutine(IDisappear());
        }

        public void ToNeutralScale()
        {
            StartCoroutine(IToNeutralScale());
        }

        public void Highlight()
        {
            StartCoroutine(IHighlight());
        }
        #endregion

        #region Functions
        private IEnumerator IAppear()
        {
            if (!isHidden) yield break;

            isHidden = false;
        }

        private IEnumerator IDisappear()
        {
            if (isHidden) yield break;

            isHidden = true;
        }

        private IEnumerator IToNeutralScale()
        {
            if (isHidden) yield break;
        }

        private IEnumerator IHighlight()
        {
            if (isHidden) yield break;
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            isHidden = true;
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
