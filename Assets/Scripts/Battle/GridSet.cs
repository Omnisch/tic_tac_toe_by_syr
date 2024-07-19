using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public abstract class GridSet : MonoBehaviour
    {
        #region Fields
        protected List<GridTile> gridTiles;
        #endregion

        #region Interfaces
        public List<GridTile> GridTiles => gridTiles;
        #endregion

        #region Functions
        #endregion

        #region Unity Methods
        protected void Start()
        {
            gridTiles = transform.GetComponentsInChildren<GridTile>().ToList();
        }
        #endregion
    }
}
