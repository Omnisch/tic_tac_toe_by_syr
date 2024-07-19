using UnityEngine;

namespace Omnis.TicTacToe
{
    [System.Serializable]
    public struct PawnId
    {
        public Party party;
        public int type;
        public Transform parent;
        public bool canHighlight;

        public PawnId(Party party, Transform parent, bool canHighlight = true)
        {
            this.party = party;
            this.type = 0;
            this.parent = parent;
            this.canHighlight = canHighlight;
        }
        public PawnId(Party party, PawnStage stage, Transform parent, bool canHighlight = true)
        {
            this.party = party;
            this.type = (int)stage;
            this.parent = parent;
            this.canHighlight = canHighlight;
        }
        public PawnId(Party party, ToolType type, Transform parent, bool canHighlight = true)
        {
            this.party = party;
            this.type = (int)type;
            this.parent = parent;
            this.canHighlight = canHighlight;
        }
        public PawnId(Party party, HintType type, Transform parent, bool canHighlight = true)
        {
            this.party = party;
            this.type = (int)type;
            this.parent = parent;
            this.canHighlight = canHighlight;
        }

        public bool SameWith(PawnId other) => this.party == other.party && this.type == other.type;
    }

    public enum Party
    {
        Null,
        Nature,
        Artifact,
        Tool,
        Hint,
    }

    public enum PawnStage
    {
        Phase0,
        Phase1,
        Phase2,
        Phase3,
    }

    public enum ToolType
    {
        Shovel,
    }

    public enum HintType
    {
        BoardHover,
        ToolHover,
        ToolInteracted,
    }
}
