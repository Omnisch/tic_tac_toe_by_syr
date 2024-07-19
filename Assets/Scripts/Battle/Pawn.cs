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

        public void Appear()
        {
            doBreathe = false;
            StopAllCoroutines();
            StartCoroutine(EaseScale(1f, () => doBreathe = true));
        }

        public void Cover()
        {
            SpriteScale = 2f;
            SpriteAlpha = 0f;
            doBreathe = false;
            StartCoroutine(EaseScale(1f, () => doBreathe = true));
            StartCoroutine(EaseAlpha(1f));
        }

        public void Disappear()
        {
            doBreathe = false;
            StopAllCoroutines();
            StartCoroutine(EaseScale(0f));
        }

        public void DisappearAndDestroy()
        {
            doBreathe = false;
            StopAllCoroutines();
            StartCoroutine(EaseScale(0f, () => Destroy(gameObject)));
        }

        public void FlashOff()
        {
            doBreathe = false;
            StopAllCoroutines();
            Destroy(gameObject);
        }

        public void ToNeutralScale()
        {
            doBreathe = false;
            StopAllCoroutines();
            StartCoroutine(EaseScale(1f, () => doBreathe = true));
        }

        public void Highlight()
        {
            doBreathe = false;
            StopAllCoroutines();
            StartCoroutine(EaseScale(GameManager.Instance.Settings.highlightScale));
        }
        #endregion

        #region Functions
        private IEnumerator EaseScale(float destScale, System.Action callback = null)
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

            callback?.Invoke();
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
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            doBreathe = false;
            SpriteScale = 0f;
            transform.localScale = Vector3.zero;
        }
        private void Update()
        {
            if (id.canBreathe && doBreathe)
                SpriteScale = GameManager.Instance.PawnBreatheScale;
        }
        #endregion
    }
}
