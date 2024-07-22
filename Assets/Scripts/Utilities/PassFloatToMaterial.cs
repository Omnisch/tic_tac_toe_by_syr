using System.Collections;
using UnityEngine;

namespace Omnis
{
    public class PassFloatToMaterial : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Material material;
        [SerializeField] private string nameOfSetFloat;
        [SerializeField][Range(0.0001f, 1f)] private float lerpSpeed = 0.1f;
        #endregion

        #region Fields
        private float floatToPass;
        private float FloatToPass
        {
            get => floatToPass;
            set
            {
                floatToPass = value;
                material.SetFloat(nameOfSetFloat, floatToPass);
            }
        }
        #endregion

        #region Interfaces
        public void LerpFromZeroToOne()
        {
            StopAllCoroutines();
            StartCoroutine(Lerp(0f, 1f));
        }
        public void LerpFromOneToZero()
        {
            StopAllCoroutines();
            StartCoroutine(Lerp(1f, 0f));
        }
        #endregion

        #region Functions
        private IEnumerator Lerp(float fromPercentage, float toPercentage)
        {
            FloatToPass = fromPercentage;

            while (Mathf.Abs(FloatToPass - toPercentage) > Mathf.Epsilon)
            {
                FloatToPass = Mathf.Lerp(FloatToPass, toPercentage, lerpSpeed);
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }

            FloatToPass = toPercentage;
        }
        #endregion
    }
}
