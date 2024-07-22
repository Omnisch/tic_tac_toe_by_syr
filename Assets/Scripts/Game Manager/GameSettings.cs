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
        [Space]
        public float scalingSpeed;
        public float highlightScale;
        [Header("Rule Settings")]
        public List<GameModeSetting> gameModeSettings;
        public List<PartySettings> partySettings;
        public List<WhatCountAsARow> whatCountAsARow;
    }

    public enum GameMode
    {
        Relax,
        NoDraw,
        Blindfold,
    }

    [Serializable]
    public struct GameModeSetting
    {
        public GameMode modeName;
        public bool allowSkip;
        public bool reloadWhenSkip;
        public bool doBlindfold;
        public List<PawnId> startup;
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

    [System.Serializable]
    public struct WhatCountAsARow
    {
        [Min(0)] public List<int> indices;
    }
}
