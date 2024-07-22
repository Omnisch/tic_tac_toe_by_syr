using System;
using System.Collections.Generic;
using UnityEngine;

namespace Omnis.TicTacToe
{
    [CreateAssetMenu(menuName = "Omnis/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Pawn")]
        public GameObject pawnPrefab;
        [Min(0f)] public float breathAmplitude;
        [Min(0.01f)] public float breathFrequency;
        [Space]
        [Range(1f, 100f)] public float lerpSpeed;
        [Range(0f,   2f)] public float highlightScale;
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
