using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Omnis.TicTacToe
{
    public class Pawn : PointerBase
    {
        #region Serialized Fields
        [SerializeField] private PawnId id;
        [SerializeField] private SpriteRenderer spriteRenderer;
        #endregion

        #region Fields
        private bool isInteractive;
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
        public override bool IsPointed
        {
            get => isPointed;
            set
            {
                isPointed = value;
                if (value) Highlight();
                else ToNeutralScale();
            }
        }

        public PawnId Id
        {
            get => id;
            set
            {
                id = value;
                spriteRenderer.sprite = GameManager.Instance.Settings.partySprites[(int)id.party].sprites[(int)id.type];
            }
        }

        public void Appear()
        {
            isInteractive = doBreatheAnim = false;
            StopAllCoroutines();
            StartCoroutine(EaseZoom(1f, () => isInteractive = doBreatheAnim = true));
        }

        public void Disappear()
        {
            isInteractive = doBreatheAnim = false;
            StopAllCoroutines();
            StartCoroutine(EaseZoom(0f, () => isInteractive = true));
        }

        public void DisappearAndDestroy()
        {
            isInteractive = doBreatheAnim = false;
            StopAllCoroutines();
            StartCoroutine(EaseZoom(0f, () => Destroy(gameObject)));
        }

        public void FlashOff()
        {
            isInteractive = doBreatheAnim = false;
            StopAllCoroutines();
            Destroy(gameObject);
        }

        public void ToNeutralScale()
        {
            if (!isInteractive) return;

            doBreatheAnim = false;
            StopAllCoroutines();
            StartCoroutine(EaseZoom(1f, () => doBreatheAnim = true));
        }

        public void Highlight()
        {
            if (!id.canHighlight) return;
            if (!isInteractive) return;

            doBreatheAnim = false;
            StopAllCoroutines();
            StartCoroutine(EaseZoom(GameManager.Instance.Settings.highlightScale));
        }
        #endregion

        #region Functions
        private IEnumerator EaseZoom(float destScale, System.Action callback = null)
        {
            float epsilon = GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
            if (SpriteScale > destScale)
                while (SpriteScale > destScale + epsilon)
                {
                    SpriteScale -= GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                    yield return new WaitForSecondsRealtime(Time.deltaTime);
                }
            else
                while (SpriteScale < destScale - epsilon)
                {
                    SpriteScale += GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                    yield return new WaitForSecondsRealtime(Time.deltaTime);
                }
            SpriteScale = destScale;

            callback?.Invoke();
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            isInteractive = false;
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

        private void OnDestroy()
        {
            Debug.LogWarning("OnDestroy() of Pawn needs to be fixed.");
        }
        #endregion

        #region Handlers
        protected override void OnInteract()
        {
            Debug.Log("Clicked on pawn.");
        }
        #endregion
    }

    [System.Serializable]
    public struct PawnId
    {
        public Party party;
        public int type;
        public Transform parent;
        public bool canHighlight;

        public PawnId(Party party, Transform parent, bool canHighlight = true)
        {
            this.party = party;
            this.type = 0;
            this.parent = parent;
            this.canHighlight = canHighlight;
        }
        public PawnId(Party party, PawnStage stage, Transform parent, bool canHighlight = true)
        {
            this.party = party;
            this.type = (int)stage;
            this.parent = parent;
            this.canHighlight = canHighlight;
        }
        public PawnId(Party party, ToolType type, Transform parent, bool canHighlight = true)
        {
            this.party = party;
            this.type = (int)type;
            this.parent = parent;
            this.canHighlight = canHighlight;
        }
        public PawnId(Party party, HintType type, Transform parent, bool canHighlight = true)
        {
            this.party = party;
            this.type = (int)type;
            this.parent = parent;
            this.canHighlight = canHighlight;
        }
    }
}
