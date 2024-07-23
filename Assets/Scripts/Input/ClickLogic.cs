using UnityEngine;
using UnityEngine.Events;

namespace Omnis
{
    [RequireComponent(typeof(Collider))]
    public class ClickLogic : PointerBase
    {
        #region Serialized Fields
        public UnityEvent enterCallback;
        public UnityEvent clickCallback;
        public UnityEvent exitCallback;
        #endregion

        #region Interfaces
        public override bool IsPointed
        {
            get => isPointed;
            set
            {
                isPointed = value;
                if (isPointed) enterCallback?.Invoke();
                else exitCallback?.Invoke();
            }
        }
        protected override void OnInteracted() => clickCallback?.Invoke();
        #endregion
    }
}
