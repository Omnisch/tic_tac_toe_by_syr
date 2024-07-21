using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    [CreateAssetMenu(menuName = "Omnis/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Startups")]
        public BattleStartup startups;
        [Header("Pawn")]
        public float amplitude;
        public float frequency;
        [Space]
        public float scalingSpeed;
        public float highlightScale;
        public List<PartySettings> partySettings;
    }

    [Serializable]
    public class PartySettings
    {
        public Party partyName;
        [SerializeField] public List<Sprite> sprites;
        [SerializeField] public List<PartyTarget> targets;
    }

    [Serializable]
    public struct PartyTarget
    {
        public List<Party> interactableParty;
    }
}
