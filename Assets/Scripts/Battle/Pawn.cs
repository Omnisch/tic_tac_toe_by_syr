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
        private IEnumerator easeRoutine;
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

        public IEnumerator Appear()
        {
            doBreathe = false;
            if (EaseIsPrior(1))
                easeRoutine = EaseScale(1f);
            yield return easeRoutine;
            doBreathe = true;
        }

        public IEnumerator Concentrate()
        {
            SpriteScale = 2f;
            SpriteAlpha = 0f;
            doBreathe = false;
            if (EaseIsPrior(1))
            {
                easeRoutine = EaseScale(1f);
                StartCoroutine(EaseAlpha(1f));
            }
            yield return easeRoutine;
            doBreathe = true;
        }

        public IEnumerator Disappear()
        {
            doBreathe = false;
            if (EaseIsPrior(2))
                easeRoutine = EaseScale(0f);
            yield return easeRoutine;
        }

        public IEnumerator DisappearAndDestroy()
        {
            yield return Disappear();
            Destroy(gameObject);
        }

        public IEnumerator ToNeutralScale()
        {
            doBreathe = false;
            if (EaseIsPrior(0))
                easeRoutine = EaseScale(1f);
            yield return easeRoutine;
            doBreathe = true;
        }

        public IEnumerator Highlight()
        {
            doBreathe = false;
            if (EaseIsPrior(0))
                easeRoutine = EaseScale(GameManager.Instance.Settings.highlightScale);
            yield return easeRoutine;
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
        private IEnumerator EaseScale(float destScale)
        {
            float epsilon = 2f * GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
            if (SpriteScale < destScale)
                while (SpriteScale < destScale - epsilon)
                {
                    SpriteScale += GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                    yield return new WaitForSecondsRealtime(Time.deltaTime);
                }
            else
                while (SpriteScale > destScale + epsilon)
                {
                    SpriteScale -= GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                    yield return new WaitForSecondsRealtime(Time.deltaTime);
                }
            SpriteScale = destScale;

            easePriority = 0;
        }

        private IEnumerator EaseAlpha(float destAlpha, System.Action callback = null)
        {
            float epsilon = 2f * GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
            if (SpriteAlpha < destAlpha)
                while (SpriteAlpha < destAlpha - epsilon)
                {
                    SpriteAlpha += GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                    yield return new WaitForSecondsRealtime(Time.deltaTime);
                }
            else
                while (SpriteAlpha > destAlpha + epsilon)
                {
                    SpriteAlpha -= GameManager.Instance.Settings.scalingSpeed * Time.deltaTime;
                    yield return new WaitForSecondsRealtime(Time.deltaTime);
                }
            SpriteAlpha = destAlpha;

            callback?.Invoke();
            easePriority = 0;
        }

        private bool EaseIsPrior(int priority)
        {
            if (priority < easePriority) return false;
            else
            {
                if (easeRoutine != null) StopCoroutine(easeRoutine);
                easePriority = priority;
                return true;
            }
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            doBreathe = false;
            SpriteScale = 0f;
            easePriority = 0;
            easeRoutine = null;
        }
        private void Update()
        {
            if (id.canBreathe && doBreathe)
                SpriteScale = GameManager.Instance.PawnBreatheScale;
        }
        #endregion
    }

    public enum PawnInitState
    {
        Appear,
        DoNotAppear,
        Transparent,
    }
}
