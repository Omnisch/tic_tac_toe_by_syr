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
        private int easePriority;
        private IEnumerator scalingRoutine;
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
            yield return Disappear();
            Id = newId;
            yield return Appear();
        }

        public void AppearForSerializing() => StartCoroutine(Appear());
        public IEnumerator Appear()
        {
            doBreathe = false;
            if (EaseIsPrior(1))
                scalingRoutine = LerpScale(1f);
            yield return scalingRoutine;
            doBreathe = true;
        }

        public void DisappearForSerializing() => StartCoroutine(Disappear());
        public IEnumerator Disappear()
        {
            doBreathe = false;
            if (EaseIsPrior(1))
                scalingRoutine = LerpScale(0f);
            yield return scalingRoutine;
        }
        public IEnumerator DisappearAndDestroy()
        {
            doBreathe = false;
            if (EaseIsPrior(2))
                scalingRoutine = LerpScale(0f);
            yield return scalingRoutine;
            StartCoroutine(DestroyAfterPlayerMove());
        }

        public IEnumerator Cover(float fromScale)
        {
            SpriteScale = fromScale;
            SpriteAlpha = 0f;
            doBreathe = false;
            if (EaseIsPrior(1))
            {
                scalingRoutine = LerpScale(1f);
                StartCoroutine(LerpAlpha(1f));
            }
            yield return scalingRoutine;
            doBreathe = true;
        }

        public IEnumerator Uncover(float toScale)
        {
            SpriteScale = 1f;
            SpriteAlpha = 1f;
            doBreathe = false;
            if (EaseIsPrior(1))
            {
                scalingRoutine = LerpScale(toScale);
                StartCoroutine(LerpAlpha(0f));
            }
            yield return scalingRoutine;
        }

        public IEnumerator ToNeutralScale()
        {
            doBreathe = false;
            if (EaseIsPrior(0))
                scalingRoutine = LerpScale(1f);
            yield return scalingRoutine;
            doBreathe = true;
        }

        public IEnumerator Highlight()
        {
            doBreathe = false;
            if (EaseIsPrior(0))
                scalingRoutine = LerpScale(GameManager.Instance.Settings.highlightScale);
            yield return scalingRoutine;
        }

        public void Show()
        {
            SpriteAlpha = 1f;
        }

        public void HalfShow()
        {
            SpriteAlpha = 0.5f;
        }

        public void Hide()
        {
            SpriteAlpha = 0f;
        }
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

            easePriority = 0;
        }

        private IEnumerator LerpAlpha(float destAlpha, float speed = -1f)
        {
            if (speed < 0f) speed = GameManager.Instance.Settings.lerpSpeed;
            float epsilon = speed * Time.deltaTime;
            while (Mathf.Abs(SpriteAlpha - destAlpha) > epsilon)
            {
                SpriteAlpha = Mathf.Lerp(SpriteAlpha, destAlpha, speed * Time.deltaTime);
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }
            SpriteAlpha = destAlpha;

            easePriority = 0;
        }

        private bool EaseIsPrior(int priority)
        {
            if (priority < easePriority) return false;
            else
            {
                if (scalingRoutine != null) StopCoroutine(scalingRoutine);
                easePriority = priority;
                return true;
            }
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
            easePriority = 0;
            scalingRoutine = null;
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
            GetComponentInParent<GridTile>()?.SignOut(this);
        }
        #endregion
    }

    public enum PawnInitState
    {
        Appear,
        Concentrate,
        DoNotAppear,
        Transparent,
    }
}
