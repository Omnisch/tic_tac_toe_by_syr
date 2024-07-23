using UnityEngine;

namespace Omnis.TicTacToe
{
    public abstract class PointerBase : MonoBehaviour
    {
        #region Fields
        protected bool interactable;
        protected bool isPointed;
        #endregion

        #region Interfaces
        public virtual bool Interactable
        {
            get => interactable;
            set => interactable = value;
        }
        public virtual bool IsPointed
        {
            get => isPointed;
            set => isPointed = value;
        }
        #endregion

        #region Functions
        protected virtual void OnInteracted() {}
        #endregion

        #region Unity Methods
        protected virtual void Start()
        {
            interactable = true;
            isPointed = false;
        }
        #endregion

        #region Handlers
        private void OnPointerEnter()
        {
            if (!Interactable) return;
            
            IsPointed = true;
        }

        private void OnPointerExit()
        {
            if (!Interactable) return;

            IsPointed = false;
        }

        private void OnInteract()
        {
            if (!Interactable) return;

            OnInteracted();
        }
        #endregion
    }
}
