using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using DS.Enumerations;
using System.Linq;

public class DSGraphView : GraphView
{
    DSEditorWindow editorWindow;
    List<DSNode> DSNodes => nodes.ToList().Cast<DSNode>().ToList();

    public DSGraphView(DSEditorWindow dsEditorWindow)
    {
        editorWindow = dsEditorWindow;

        AddManipulators();
        AddGridBackground();

        AddStyles();

        graphViewChanged = OnGraphChange;
    }
    
    private GraphViewChange OnGraphChange(GraphViewChange change)
    {
        if (change.edgesToCreate != null)
        {
            foreach (Edge edge in change.edgesToCreate)
            {
                //...
            }
            editorWindow.RefreshToolBarButtonsAvailablity(nodes.ToList().Count);
        }

        if (change.elementsToRemove != null)
        {
            foreach (GraphElement e in change.elementsToRemove)
            {
                //Debug.Log(e.GetType().ToString()+" got deleted! refreshing tool bar");
                if (e.GetType() == typeof(Edge))
                {
                    //Debug.Log("deleted an edge!");
                    editorWindow.RefreshToolBarButtonsAvailablity(nodes.ToList().Count);
                }
                else
                {
                    //Debug.Log("deleted a node!");
                    editorWindow.RefreshToolBarButtonsAvailablity(nodes.ToList().Count - 1);
                }
            }
        }

        if (change.movedElements != null)
        {
            foreach (GraphElement e in change.movedElements)
            {
                //... notify user about elements?
            }
            editorWindow.RefreshToolBarButtonsAvailablity(nodes.ToList().Count);
        }
        return change;
    }
    
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        // Getting list of all valid ports by
        // direction (Input -> only to -> Output) | (EntryNode is not EndPort) | The type of nood can connect to one specified other node type
        List<Port> validConnections = ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node &&
        ( // case 1: user pressed the output port, and is looking for valid input port
        (endPort.direction == Direction.Input && DataController.IsValidConnectionByType(startPort.node.Q<DSNode>().DialogueType, endPort.node.Q<DSNode>().DialogueType))
        || // case 2: user pressed the input port, and is looking for valid output port
        (endPort.direction == Direction.Output && DataController.IsValidConnectionByType(endPort.node.Q<DSNode>().DialogueType, startPort.node.Q<DSNode>().DialogueType))
        )
        ).ToList();
        
        return validConnections;
    }

    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(CreateNodeContextualMenu("Add Node 1", DSDialogueType.NodeType1));
        this.AddManipulator(CreateNodeContextualMenu("Add Node 2", DSDialogueType.NodeType2));
        this.AddManipulator(CreateNodeContextualMenu("Add Node 3", DSDialogueType.NodeType3));

        #region TODO: Extra features..
        /* 
        // BlackBoard: Ability to edit the "allowed connection type" for each of the node's type.
        var blackboard = new Blackboard(this);
        Button saveButton = new Button() { text = "Save" };
        saveButton.clicked += editorWindow.SaveToJson;
        blackboard.Add(saveButton);
        this.Add(blackboard);
        */
        #endregion
    }

    private IManipulator CreateNodeContextualMenu(string titleName, DSDialogueType nodeType)
    {
        ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction(titleName, actionEvent => AddElement(
                CreateNode(nodeType, actionEvent.eventInfo.localMousePosition)))
        );
        return menuManipulator;
    }

    public DSNode CreateNode(DSDialogueType nodeType, Vector2 position)
    {
        Type _nodeType = Type.GetType($"DS{nodeType}");

        DSNode node = (DSNode) Activator.CreateInstance(_nodeType);
        node.Initialize(this, position);
        node.Draw();

        editorWindow.RefreshToolBarButtonsAvailablity(nodes.ToList().Count + 1);

        return node;
    }

    private void AddGridBackground()
    {
        GridBackground gridBg = new GridBackground();

        gridBg.StretchToParentSize();

        Insert(0, gridBg);
    }
    
    private void AddStyles()
    {
        StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyles.uss");
        if (styleSheet != null)
            styleSheets.Add(styleSheet);
    }
}
