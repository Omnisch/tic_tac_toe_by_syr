namespace Omnis.TicTacToe
{
    public enum Party
    {
        Null,
        Nature,
        Artifact,
    }

    public enum PawnStage
    {
        Phase0,
        Phase1,
        Phase2,
        Phase3,
    }

    [System.Serializable]
    public struct PawnId
    {
        public Party party;
        public PawnStage stage;
        public UnityEngine.Transform parent;

        public PawnId(Party party, PawnStage stage, UnityEngine.Transform parent)
        {
            this.party = party;
            this.stage = stage;
            this.parent = parent;
        }
        public PawnId(Party party, UnityEngine.Transform parent)
        {
            this.party = party;
            this.stage = PawnStage.Phase0;
            this.parent = parent;
        }
    }
}
