using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Omnis.TicTacToe
{
    public class Pawn : MonoBehaviour
    {
        #region Serialized Fields
        #endregion

        #region Fields
        private bool isHidden;
        private bool doBreatheAnim;
        private bool isPointed;
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
        private Coroutine coroutine;
        #endregion

        #region Interfaces
        public bool IsPointed
        {
            get => isPointed;
            set
            {
                isPointed = value;
                if (value) Highlight();
                else ToNeutralScale();
            }
        }

        public void Appear()
        {
            coroutine = StartCoroutine(IAppear());
        }

        public void Disappear()
        {
            coroutine = StartCoroutine(IDisappear());
        }

        public void ToNeutralScale()
        {
            if (coroutine != null) return;
            coroutine = StartCoroutine(IToNeutralScale());
        }

        public void Highlight()
        {
            if (coroutine != null) return;
            coroutine = StartCoroutine(IHighlight());
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
            coroutine = null;
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
            coroutine = null;
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
            coroutine = null;
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
            coroutine = null;
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            isHidden = true;
            doBreatheAnim = false;
            IsPointed = false;
            SpriteScale = 0f;
            transform.localScale = Vector3.zero;
        }
        private void Update()
        {
            if (doBreatheAnim && GameManager.Instance)
                SpriteScale = GameManager.Instance.PawnBreatheScale;
        }
        #endregion

        #region Handlers
        protected void OnPointerEnter()
        {
            IsPointed = true;
        }

        protected void OnPointerExit()
        {
            IsPointed = false;
        }

        protected void OnInteract()
        {
            Debug.Log("Interacted.");
        }
        #endregion
    }
}
