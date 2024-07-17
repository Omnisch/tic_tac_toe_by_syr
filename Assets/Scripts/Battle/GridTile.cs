using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Omnis.TicTacToe
{
    public class GridTile : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        #region Serialized Fields
        #endregion

        #region Fields
        private int x;
        private int y;
        #endregion

        #region Interfaces
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        public void OnPointerEnter(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
