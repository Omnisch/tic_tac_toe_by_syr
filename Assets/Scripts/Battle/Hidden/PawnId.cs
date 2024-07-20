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
        public PawnId(Party party, int type, bool canBreathe = true)
        {
            this.party = party;
            this.type = type;
            this.canBreathe = canBreathe;
        }
        public PawnId(Party party, PawnPhase phase, bool canBreathe = true)
        {
            this.party = party;
            this.type = (int)phase;
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
        public int NextType => (this.type + 1) % System.Enum.GetNames(typeof(PawnPhase)).Length;
    }

    public enum Party
    {
        Null,
        Nature,
        Artifact,
        Tool,
        Hint,
    }

    public enum PawnPhase
    {
        Phase0,
        Phase1,
        Phase2,
        Phase3,
    }

    public enum ToolType
    {
        Shovel1,
        Shovel2,
    }

    public enum HintType
    {
        Board,
        Tool,
        ToolInteracted,
    }
}
