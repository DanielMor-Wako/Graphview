using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private string filePath;
    private DSGraphView _targetGraphView;
    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<DSNode> Nodes => _targetGraphView.nodes.ToList().Cast<DSNode>().ToList();
    private SaveData _containerCache;
    
    public static GraphSaveUtility SetGraphInstance(DSGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void UpdateFilePath(string newFilePath)
    {
        filePath = newFilePath;
    }

    public void SaveGraph()
    {
        if (filePath == null)
            return;

        SaveData _refContainer = new SaveData();
        List<DSLinkSaveData> links = new List<DSLinkSaveData>();
        List<DSNodeSaveData> nodes = new List<DSNodeSaveData>();

        // Collecting Valid Edges(Ports): Where Output has a connection to an Input
        var connectPorts = Edges.Where(x => x.input.node != null).ToArray();
        for (int i = 0; i<connectPorts.Length; i++)
        {
            var outputNode = connectPorts[i].output.node as DSNode;
            var inputNode = connectPorts[i].input.node as DSNode;

            DSLinkSaveData portData = new DSLinkSaveData();
            portData.BaseNodeGuid = outputNode.ID;
            portData.PortName = outputNode.DialogueName;
            portData.TargetNodeGuid = inputNode.ID;

            links.Add(portData);
        }
        _refContainer.nodesLinks = links;

        // Collecting Nodes
        foreach (var DsNode in Nodes)
        {
            if (filePath == null)
                return;

            DSNodeSaveData nodeData = new DSNodeSaveData
            {
                ID = DsNode.ID,
                Name = DsNode.DialogueName,
                DialogueType = DsNode.DialogueType,
                Position = DsNode.GetPositionCordinates()
            };
            nodes.Add(nodeData);
        }
        _refContainer.nodes = nodes;
        
        // Saving the file
        string json = JsonUtility.ToJson(_refContainer);
        File.WriteAllText(filePath + "/save.txt", json);
    }

    public void LoadGraph()
    {
        if (filePath == null)
            return;

        if (!File.Exists(filePath + "/save.txt"))
        {
            EditorUtility.DisplayDialog("Data File Not Found.", "The file does not exist.", "Ok");
            return;
        }

        string json = File.ReadAllText(filePath + "/save.txt");
        _containerCache = JsonUtility.FromJson<SaveData>(json);
        
        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
        for (var i=0; i < Nodes.Count; i++)
        {
            var connections = _containerCache.nodesLinks.Where(x => x.BaseNodeGuid == Nodes[i].ID).ToList();
            for (var j=0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = Nodes.First(x => x.ID == targetNodeGuid);
                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
            }
        }
    }

    private void LinkNodes(Port _output, Port _input)
    {
        var tempEdge = new Edge
        {
            output = _output,
            input = _input
        };
        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }

    private void CreateNodes()
    {
        foreach (var nodeData in _containerCache.nodes)
        {
            var newNode = _targetGraphView.CreateNode(nodeData.DialogueType, nodeData.Position);
            newNode.ID = nodeData.ID;
            newNode.DialogueName = nodeData.Name;
            _targetGraphView.AddElement(newNode);
        }
    }

    private void ClearGraph()
    {
        foreach (var node in Nodes)
        {
            // Remove link / edge
            Edges.Where(x => x.input.node == node).ToList()
                .ForEach(edge => _targetGraphView.RemoveElement(edge));
            // Remove node
            _targetGraphView.RemoveElement(node);
        }
    }
}
