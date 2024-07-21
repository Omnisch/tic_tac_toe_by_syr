using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Omnis.TicTacToe
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Logic adminLogic;
        #endregion

        #region Fields
        private List<Collider> PointerHits;
        #endregion

        #region Interfaces
        public void SetInputEnabled(bool value)
        {
            enabled = value;
            Cursor.visible = value;
        }
        #endregion

        #region Functions
        private void FlushInput() {}
        #endregion

        #region Unity Methods
        private void Start()
        {
            foreach (var map in playerInput.actions.actionMaps)
                map.Enable();

            PointerHits = new();
        }

        private void OnEnable()
        {
            playerInput.enabled = true;
            FlushInput();
        }

        private void OnDisable()
        {
            playerInput.enabled = false;
        }
        #endregion

        #region Handlers
        protected void OnInteract()
        {
            PointerHits.ForEach(hit => hit.SendMessage("OnInteract", options: SendMessageOptions.DontRequireReceiver));
        }

        protected void OnDebugTest()
        {
            foreach (var pointerHit in PointerHits)
                Debug.Log(pointerHit);
            adminLogic.Invoke();
        }

        protected void OnPointer(InputValue value)
        {
            Ray r = Camera.main.ScreenPointToRay(value.Get<Vector2>());
            var newHits = Physics.RaycastAll(r).Select(hit => hit.collider).ToList();
            foreach (var hit in PointerHits.Except(newHits).ToList())
                if (hit) hit.SendMessage("OnPointerExit", options: SendMessageOptions.DontRequireReceiver);
            foreach (var hit in newHits.Except(PointerHits).ToList())
                if (hit) hit.SendMessage("OnPointerEnter", options: SendMessageOptions.DontRequireReceiver);
            PointerHits = newHits;
        }
        #endregion
    }
}
