using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using DS.Enumerations;
using System;

public class DSNode : Node
{
    public string ID { get; set; }
    public string DialogueName { get; set; }
    public DSDialogueType DialogueType { get; set; }
    protected GraphView graphView;

    public virtual void Initialize(DSGraphView dsGraphView, Vector2 pos)
    {
        graphView = dsGraphView;
        ID = Guid.NewGuid().ToString();
        DialogueName = "NodeNameByType";
        SetPosition(new Rect(pos, Vector2.zero));
    }

    public virtual void Draw()
    {
        // Title Container
        TextField dialogueTextField = new TextField()
        {
            value = DialogueName
        };

        titleContainer.Insert(0, dialogueTextField);

        // Input Container
        Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        inputPort.portName = "In";
        inputContainer.Add(inputPort);

        // Output Container
        Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        outputPort.portName = "Out";
        outputContainer.Add(outputPort);

        #region ToDo: Node Extentions
        // Extention Container
        /*VisualElement customDataContainer = new VisualElement();

        Foldout textFoldout = new Foldout()
        {
            text = "Task Assignment"
        };

        TextField textTextField = new TextField()
        {
            value = Text
        };

        textFoldout.Add(textTextField);

        customDataContainer.Add(textFoldout);

        extensionContainer.Add(customDataContainer);

        RefreshExpandedState(); // RefreshExpendedState also calls RefreshPorts()
        */

        //RefreshPorts();
        #endregion

    }

    public Vector2 GetPositionCordinates()
    {
        return GetPosition().position;
    }
}
