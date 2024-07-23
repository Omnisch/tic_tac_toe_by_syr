using System.Collections;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class Pawn : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private PawnId id;
        [SerializeField] private SpriteRenderer spriteRenderer;
        #endregion

        #region Fields
        private bool doBreathe;
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
        private float SpriteAlpha
        {
            get => transform.GetComponent<SpriteRenderer>().color.a;
            set => transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, value);
        }
        private int lerpPriority;
        private IEnumerator scaleRoutine;
        private IEnumerator alphaRoutine;
        #endregion

        #region Interfaces
        public PawnId Id
        {
            get => id;
            set
            {
                id = value;
                spriteRenderer.sprite = GameManager.Instance.GetSpriteOfParty(id.party, id.type);
            }
        }

        public IEnumerator ChangeIdAndRefresh(PawnId newId)
        {
            yield return IDisappear();
            Id = newId;
            yield return IAppear();
        }

        public bool Appear { set => StartCoroutine(value ? IAppear() : IDisappear()); }
        public IEnumerator IAppear()
        {
            doBreathe = false;
            if (LerpIsPrior(1))
                scaleRoutine = LerpScale(1f);
            yield return scaleRoutine;
            doBreathe = true;
        }
        public IEnumerator IDisappear()
        {
            doBreathe = false;
            if (LerpIsPrior(1))
                scaleRoutine = LerpScale(0f);
            yield return scaleRoutine;
        }
        public IEnumerator IDisappearAndDestroy()
        {
            doBreathe = false;
            if (LerpIsPrior(2))
                scaleRoutine = LerpScale(0f);
            yield return scaleRoutine;
            StartCoroutine(DestroyAfterPlayerMove());
        }

        public void Cover(float fromScale) => StartCoroutine(ICover(fromScale));
        public IEnumerator ICover(float fromScale)
        {
            SpriteScale = fromScale;
            SpriteAlpha = 0f;
            doBreathe = false;
            if (LerpIsPrior(1))
            {
                scaleRoutine = LerpScale(1f);
                alphaRoutine = LerpAlpha(1f);
            }
            StartCoroutine(alphaRoutine);
            yield return scaleRoutine;
            doBreathe = true;
        }
        public void Uncover(float toScale) => StartCoroutine(IUncover(toScale));
        public IEnumerator IUncover(float toScale)
        {
            SpriteScale = 1f;
            SpriteAlpha = 1f;
            doBreathe = false;
            if (LerpIsPrior(1))
            {
                scaleRoutine = LerpScale(toScale);
                alphaRoutine = LerpAlpha(0f);
            }
            StartCoroutine(alphaRoutine);
            yield return scaleRoutine;
        }

        public bool Highlight { set => StartCoroutine(value ? IHighlight() : IToNeutralScale()); }
        public bool Display { set => GetComponent<SpriteRenderer>().enabled = value; }
        public void SetAlpha(float value) => SpriteAlpha = value;
        #endregion

        #region Functions
        private IEnumerator LerpScale(float destScale)
        {
            float speed = GameManager.Instance.Settings.lerpSpeed;
            float epsilon = speed * Time.deltaTime;
            while (Mathf.Abs(SpriteScale - destScale) > epsilon)
            {
                SpriteScale = Mathf.Lerp(SpriteScale, destScale, speed * Time.deltaTime);
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }
            SpriteScale = destScale;

            lerpPriority = 0;
        }

        private IEnumerator LerpAlpha(float destAlpha)
        {
            float speed = GameManager.Instance.Settings.lerpSpeed;
            float epsilon = speed * Time.deltaTime;
            while (Mathf.Abs(SpriteAlpha - destAlpha) > epsilon)
            {
                SpriteAlpha = Mathf.Lerp(SpriteAlpha, destAlpha, speed * Time.deltaTime);
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }
            SpriteAlpha = destAlpha;

            lerpPriority = 0;
        }

        private bool LerpIsPrior(int priority)
        {
            if (priority < lerpPriority) return false;
            else
            {
                if (scaleRoutine != null) StopCoroutine(scaleRoutine);
                if (alphaRoutine != null) StartCoroutine(alphaRoutine);
                lerpPriority = priority;
                return true;
            }
        }

        private IEnumerator IToNeutralScale()
        {
            doBreathe = false;
            if (LerpIsPrior(0))
                scaleRoutine = LerpScale(1f);
            yield return scaleRoutine;
            doBreathe = true;
        }
        private IEnumerator IHighlight()
        {
            doBreathe = false;
            if (LerpIsPrior(0))
                scaleRoutine = LerpScale(GameManager.Instance.Settings.highlightScale);
            yield return scaleRoutine;
        }

        private IEnumerator DestroyAfterPlayerMove()
        {
            yield return new WaitUntil(() => GameManager.Instance.Controllable);
            StopAllCoroutines();
            Destroy(gameObject);
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            doBreathe = false;
            SpriteScale = 0f;
            lerpPriority = 0;
            scaleRoutine = null;
        }
        private void Update()
        {
            if (doBreathe)
            {
                switch (Id.breathType)
                {
                    case BreathType.None:
                        break;
                    case BreathType.Breath:
                        SpriteScale = 1f + GameManager.Instance.PawnBreatheScale;
                        break;
                    case BreathType.Floating:
                        transform.localPosition = new Vector3(transform.localPosition.x, GameManager.Instance.PawnBreatheScale, transform.localPosition.z);
                        break;
                    case BreathType.Rolling:
                        transform.localRotation = Quaternion.Euler(0f, 0f, 50f * GameManager.Instance.PawnBreatheScale);
                        break;
                }
            }
        }

        private void OnDestroy()
        {
            if (GetComponentInParent<GridTile>()) GetComponentInParent<GridTile>().SignOut(this);
        }
        #endregion
    }

    public enum PawnInitState
    {
        Appear,
        Concentrate,
        DoNotAppear,
        Hide,
    }
}
