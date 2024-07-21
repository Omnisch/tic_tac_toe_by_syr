using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class Player
    {
        #region Fields
        private readonly GridSet toolkit;
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
                if (value == null)
                {
                    secondTile = value;
                    return;
                }
                if (!FirstTile)
                {
                    return;
                }
                {
                    PawnId firstTileId = FirstTile.Pawns[0].Id;
                    PawnId secondTileId;
                    if (value.Pawns.Count > 0)
                        secondTileId = value.Pawns[0].Id;
                    else 
                        secondTileId = new(Party.Null);
                    PartySettings firstTilePartySetting = GameManager.Instance.Settings.partySettings[(int)firstTileId.party];
                    if (!firstTilePartySetting.targets[firstTileId.type].interactableParty.Contains(secondTileId.party))
                    {
                        Camera.main.GetComponentInParent<ChildrenDither>().Stroke(Vector3.one);
                        return;
                    }
                }

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
