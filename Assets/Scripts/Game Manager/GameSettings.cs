using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    [CreateAssetMenu(menuName = "Omnis/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Pawn")]
        public float amplitude;
        public float frequency;
        public List<PartySprites> partySprites;
    }

    [Serializable]
    public class PartySprites
    {
        public Party partyName;
        [SerializeField] public Sprite[] sprites;
    }
}
