using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class Player
    {
        #region Fields
        private GridSet toolkit;
        private bool active;
        private GridTile firstTile;
        private GridTile secondTile;
        #endregion

        #region Interfaces
        public bool Active
        {
            get => active;
            set
            {
                active = value;
                if (active) toolkit.ActiveAll();
                else toolkit.DeactiveAll();
            }
        }
        public GridTile FirstTile
        {
            get => firstTile;
            set
            {
                if (firstTile) firstTile.Selected = false;
                firstTile = value;
                if (firstTile) firstTile.Selected = true;
            }
        }
        public GridTile SecondTile
        {
            get => secondTile;
            set
            {
                if (!FirstTile) return;
                if (secondTile) secondTile.Selected = false;
                secondTile = value;
                if (secondTile) secondTile.Selected = true;
            }
        }

        public Player(GridSet toolkit)
        {
            this.toolkit = toolkit;
        }
        #endregion

        #region Functions
        #endregion
    }
}
