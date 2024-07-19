namespace Omnis.TicTacToe
{
    [System.Serializable]
    public struct PawnId
    {
        public Party party;
        public int type;
        public bool canBreathe;

        public PawnId(Party party, bool canBreathe = true)
        {
            this.party = party;
            this.type = 0;
            this.canBreathe = canBreathe;
        }
        public PawnId(Party party, PawnStage stage, bool canBreathe = true)
        {
            this.party = party;
            this.type = (int)stage;
            this.canBreathe = canBreathe;
        }
        public PawnId(Party party, ToolType type, bool canBreathe = true)
        {
            this.party = party;
            this.type = (int)type;
            this.canBreathe = canBreathe;
        }
        public PawnId(Party party, HintType type, bool canBreathe = true)
        {
            this.party = party;
            this.type = (int)type;
            this.canBreathe = canBreathe;
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
        Board,
        Tool,
        ToolInteracted,
    }
}
