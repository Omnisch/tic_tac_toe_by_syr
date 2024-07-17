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

        #region Functions
        private void FlushInput() {}
        #endregion

        #region Unity Methods
        private void Start()
        {
            foreach (var map in playerInput.actions.actionMaps)
                map.Enable();
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
            Debug.Log("L");
        }

        protected void OnDebugTest()
        {
            Debug.Log("F");
            adminLogic.Invoke();
        }
        #endregion
    }
}
