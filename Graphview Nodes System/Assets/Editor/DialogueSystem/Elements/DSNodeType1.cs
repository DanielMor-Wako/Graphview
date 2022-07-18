using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DS.Enumerations;

public class DSNodeType1 : DSNode
{

    public override void Initialize(DSGraphView dsGraphView, Vector2 pos)
    {
        base.Initialize(dsGraphView, pos);

        DialogueName = "Node 1";
        DialogueType = DSDialogueType.NodeType1;
    }

    public override void Draw()
    {
        base.Draw();
    }
}
