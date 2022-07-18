using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    [field: SerializeField] public List<DSNodeSaveData> nodes;
    [field: SerializeField] public List<DSLinkSaveData> nodesLinks;
}