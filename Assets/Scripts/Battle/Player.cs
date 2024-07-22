using UnityEngine;

namespace Omnis.TicTacToe
{
    public partial class Player
    {
        #region Fields
        private readonly GridSet toolkit;
        private GridTile firstTile;
        private GridTile secondTile;
        #endregion

        #region Interfaces
        public bool Active { set => toolkit.Active = value; }
        public GridSet Toolkit => toolkit;
        public GridTile FirstTile
        {
            get => firstTile;
            set
            {
                if (firstTile) firstTile.Picked = false;
                firstTile = value;
                if (firstTile) firstTile.Picked = true;
            }
        }
        public GridTile SecondTile
        {
            get => secondTile;
            set
            {
                if (!value)
                {
                    secondTile = value;
                    return;
                }
                if (!FirstTile) return;

                PawnId firstTileId = FirstTile.Pawns[0].Id;
                PawnId secondTileId;
                if (value.Pawns.Count > 0)
                    secondTileId = value.Pawns[0].Id;
                else 
                    secondTileId = new(Party.Null);
                PartySettings firstTilePartySetting = GameManager.Instance.Settings.partySettings[(int)firstTileId.party];
                if (!firstTilePartySetting.targets[firstTileId.type].interactableParty.Contains(secondTileId.party))
                {
                    Camera.main.GetComponentInParent<Logic>().Invoke();
                    return;
                }

                if (secondTile) secondTile.Picked = false;
                secondTile = value;
                if (secondTile) secondTile.Picked = true;
            }
        }

        public Player(GridSet toolkit)
        {
            this.toolkit = toolkit;
        }
        #endregion
    }
}
