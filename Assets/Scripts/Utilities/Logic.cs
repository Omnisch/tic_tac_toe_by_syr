using UnityEngine;
using UnityEngine.Events;

namespace Omnis
{
    public class Logic : MonoBehaviour
    {
        #region Serialized fields
        public UnityEvent callback;
        #endregion

        #region Interfaces
        [ContextMenu("Invoke")]
        public virtual void Invoke()
        {
            callback?.Invoke();
        }
        #endregion
    }
}
