namespace Omnis.TicTacToe
{
    public enum GameMode
    {
        Standard,
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
