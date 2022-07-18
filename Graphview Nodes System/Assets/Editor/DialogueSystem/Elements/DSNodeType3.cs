using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DS.Enumerations;

public class DSNodeType3 : DSNode
{

    public override void Initialize(DSGraphView dsGraphView, Vector2 pos)
    {
        base.Initialize(dsGraphView, pos);

        DialogueName = "Node 3";
        DialogueType = DSDialogueType.NodeType3;
    }

    public override void Draw()
    {
        base.Draw();
    }
}
