using UnityEngine;

namespace Omnis.TicTacToe
{
    [CreateAssetMenu(menuName = "Omnis/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Pawn")]
        public float amplitude;
        public float frequency;
    }
}
