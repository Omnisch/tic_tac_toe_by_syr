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
        private float spriteScale;
        private float SpriteScale
        {
            get => spriteScale;
            set
            {
                spriteScale = value;
                transform.localScale = value * Vector3.one;
            }
        }
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

            doBreatheAnim = false;

            while (SpriteScale < 1f)
            {
                SpriteScale += GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }

            SpriteScale = 1f;
            isHidden = false;
            doBreatheAnim = true;
        }

        private IEnumerator IDisappear()
        {
            if (isHidden) yield break;

            doBreatheAnim = false;

            while (SpriteScale > 0f)
            {
                SpriteScale -= GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }

            SpriteScale = 0f;
            isHidden = true;
        }

        private IEnumerator IToNeutralScale()
        {
            if (isHidden) yield break;

            doBreatheAnim = false;

            if (SpriteScale > 1f)
            {
                while (SpriteScale > 1f)
                {
                    SpriteScale -= GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                    yield return new WaitForSecondsRealtime(Time.deltaTime);
                }
            }
            else
            {
                while (SpriteScale < 1f)
                {
                    SpriteScale += GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                    yield return new WaitForSecondsRealtime(Time.deltaTime);
                }
            }

            SpriteScale = 1f;
            doBreatheAnim = true;
        }

        private IEnumerator IHighlight()
        {
            if (isHidden) yield break;

            doBreatheAnim = false;

            while (SpriteScale < GameManager.Instance.Settings.highlightScale)
            {
                SpriteScale += GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }

            SpriteScale = GameManager.Instance.Settings.highlightScale;
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            isHidden = true;
            doBreatheAnim = false;
            SpriteScale = 0f;
            transform.localScale = Vector3.zero;
        }
        private void Update()
        {
            if (doBreatheAnim && GameManager.Instance)
                SpriteScale = GameManager.Instance.PawnBreatheScale;
        }
        #endregion
    }
}
