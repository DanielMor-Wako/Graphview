using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections.Generic;

public class DSEditorWindow : EditorWindow
{
    // References
    public DSGraphView graphView;
    public TextField fileLocation;
    Button saveButton;
    Button loadButton;

    [MenuItem("Window/DS/Nodes Graph")]
    public static void OpenWindow()
    {
        DSEditorWindow editorWindow = GetWindow<DSEditorWindow>();
        editorWindow.titleContent = new GUIContent("Nodes Graph");
    }

    public void OnEnable()
    {
        AddGraphView();
        
        AddToolbar();
        AddStyles();

        LoadFromJson();
    }

    public void RefreshToolBarButtonsAvailablity(int numberOfExistingNodes)
    {
        bool hasAnyNodesOnScreen = numberOfExistingNodes > 0;
        saveButton.SetEnabled(hasAnyNodesOnScreen);
    }

    private void AddToolbar()
    {
        Toolbar toolbar = new Toolbar();
        
        loadButton = new Button() { text = "Load" };
        loadButton.clicked += LoadFromJson;
        toolbar.Add(loadButton);

        saveButton = new Button() { text = "Save"};
        saveButton.RegisterValueChangedCallback(OnValueChange);
        saveButton.clicked += SaveToJson;
        toolbar.Add(saveButton);

        RefreshToolBarButtonsAvailablity(graphView.nodes.ToList().Count);

        fileLocation = new TextField()
        {
            value = Application.dataPath,
            label = "File Name:"
        };
        toolbar.Add(fileLocation);
        
        rootVisualElement.Add(toolbar);
    
    }

    public string GetSaveFileLocation()
    {
        return fileLocation.value;
    }

    public void SaveToJson()
    {
        // to scriptable object
        var saveUtility = GraphSaveUtility.SetGraphInstance(graphView);
        saveUtility.UpdateFilePath(GetSaveFileLocation());
        saveUtility.SaveGraph();
        RefreshToolBarButtonsAvailablity(0); //estatics, removes the save option after saving
    }

    public void LoadFromJson()
    {
        var saveUtility = GraphSaveUtility.SetGraphInstance(graphView);
        saveUtility.UpdateFilePath(GetSaveFileLocation());
        saveUtility.LoadGraph();
        RefreshToolBarButtonsAvailablity(0); //estatics, removes the save option after loading
    }

    private void OnValueChange(ChangeEvent<string> evt)
    {
        Debug.Log("value changed");
    }

    private void AddGraphView()
    {
        graphView = new DSGraphView(this);
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);

        var dataUtility = DataController.SetGraphInstance(graphView);
        dataUtility.InitilizeData();
    }

    private void AddStyles()
    {
        StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSVariables.uss");
        if (styleSheet != null)
            rootVisualElement.styleSheets.Add(styleSheet);
    }
}