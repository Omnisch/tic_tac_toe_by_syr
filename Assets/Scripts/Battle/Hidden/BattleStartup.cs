using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    [CreateAssetMenu(menuName = "Omnis/Battle Startup")]
    public class BattleStartup : ScriptableObject
    {
        public List<StartupSet> startupSets;
    }

    [System.Serializable]
    public struct StartupSet
    {
        public GameMode mode;
        public List<PawnId> pawnIds;
    }

    public enum GameMode
    {
        Relax,
        NoDraw,
        Blindfold,
    }
}
