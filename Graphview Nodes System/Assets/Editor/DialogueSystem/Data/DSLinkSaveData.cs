using System;
using UnityEngine;

[Serializable]
public struct DSLinkSaveData
{
    [field: SerializeField] public string BaseNodeGuid { get; set; }
    [field: SerializeField] public string PortName { get; set; }
    [field: SerializeField] public string TargetNodeGuid { get; set; }
}