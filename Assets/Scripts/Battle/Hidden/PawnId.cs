namespace Omnis.TicTacToe
{
    [System.Serializable]
    public struct PawnId
    {
        public Party party;
        public int type;
        public BreathType breathType;

        public PawnId(Party party, BreathType breathType = BreathType.None)
        {
            this.party = party;
            this.type = 0;
            this.breathType = breathType;
        }
        public PawnId(Party party, int type, BreathType breathType = BreathType.None)
        {
            this.party = party;
            this.type = type;
            this.breathType = breathType;
        }
        public PawnId(Party party, PawnPhase phase, BreathType breathType = BreathType.None)
        {
            this.party = party;
            this.type = (int)phase;
            this.breathType = breathType;
        }
        public PawnId(Party party, ToolType type, BreathType breathType = BreathType.None)
        {
            this.party = party;
            this.type = (int)type;
            this.breathType = breathType;
        }
        public PawnId(Party party, HintType type, BreathType breathType = BreathType.None)
        {
            this.party = party;
            this.type = (int)type;
            this.breathType = breathType;
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
        Hammer1,
        Hammer2,
    }

    public enum HintType
    {
        Board,
        Tool,
        ToolInteracted,
        Lock,
    }

    public enum BreathType
    {
        None,
        Breath,
        Floating,
        Rolling,
    }
}
