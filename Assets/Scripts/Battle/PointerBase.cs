using UnityEngine;

namespace Omnis.TicTacToe
{
    public abstract class PointerBase : MonoBehaviour
    {
        #region Fields
        protected bool isPointed;
        #endregion

        #region Interfaces
        public virtual bool IsPointed
        {
            get => isPointed;
            set => isPointed = value;
        }
        #endregion

        #region Handlers
        protected virtual void OnPointerEnter()
        {
            IsPointed = true;
        }

        protected virtual void OnPointerExit()
        {
            IsPointed = false;
        }

        protected virtual void OnInteract()
        {
            Debug.Log("Interacted.");
        }
        #endregion
    }
}
