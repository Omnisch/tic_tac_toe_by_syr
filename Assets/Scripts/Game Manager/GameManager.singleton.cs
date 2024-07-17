using System.Collections;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class GameManager
    {
        #region Serialized Fields
        private static GameManager instance;
        #endregion

        #region Interfaces
        public static GameManager Instance => instance;
        #endregion

        #region Functions
        private bool EnsureSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return false;
            }
            else
            {
                instance = this;
                StartCoroutine(DontDestroySelfOnLoadCoroutine());
                return true;
            }
        }

        private IEnumerator DontDestroySelfOnLoadCoroutine()
        {
            yield return new WaitUntil(() => gameObject.scene.isLoaded);
            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }
}
