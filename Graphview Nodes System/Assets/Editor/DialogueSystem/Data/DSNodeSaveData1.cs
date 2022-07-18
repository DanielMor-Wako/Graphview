using System;
using UnityEngine;
using DS.Enumerations;

[Serializable]
public struct DSNodeSaveData
{
    [field: SerializeField] public string ID { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public DSDialogueType DialogueType { get; set; }
    [field: SerializeField] public Vector2 Position { get; set; }
}