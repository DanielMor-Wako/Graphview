using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DS.Enumerations;

public class DSNodeType2 : DSNode
{

    public override void Initialize(DSGraphView dsGraphView, Vector2 pos)
    {
        base.Initialize(dsGraphView, pos);

        DialogueName = "Node 2";
        DialogueType = DSDialogueType.NodeType2;
    }

    public override void Draw()
    {
        base.Draw();
    }
}
