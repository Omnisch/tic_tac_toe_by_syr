using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public class GridSet : MonoBehaviour
    {
        #region Serialized Fields
        #endregion

        #region Fields
        protected List<GridTile> gridTiles;
        private SpriteRenderer background;
        #endregion

        #region Interfaces
        public List<GridTile> GridTiles => gridTiles;

        public bool Active
        {
            set
            {
                if (background) background.color = new Color(1f, 1f, 1f, value ? 1f : 0.5f);
                gridTiles.ForEach(gridTile => gridTile.Interactable = value);
            }
        }

        public GridTile FindFirstAvailable() => gridTiles.Find(tile => tile.Pawns.Count == 0);
        #endregion

        #region Unity Methods
        private void Start()
        {
            gridTiles = transform.GetComponentsInChildren<GridTile>().ToList();
            background = GetComponentInParent<SpriteRenderer>();
        }
        #endregion
    }
}
