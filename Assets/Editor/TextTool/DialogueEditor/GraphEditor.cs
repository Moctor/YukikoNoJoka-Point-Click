using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphEditor : EditorWindow 
{
    private TextGraphView graphView;
    
    [MenuItem("Text/Text Graph")]
    private static void ShowWindow() 
    {
        GetWindow<GraphEditor>().titleContent = new GUIContent("Text Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }
    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();

        Button textNodeCreate = new Button(() => {graphView.CreateNode(NodeType.forText, Vector2.zero);});
        textNodeCreate.text = "Text Node";

        Button eventNodeCreate = new Button(() => {graphView.CreateNode(NodeType.forEvent, Vector2.zero);});
        eventNodeCreate.text = "Event Node";

        TextField fileName = new TextField("File Name");
        fileName.SetValueWithoutNotify("New Narrative");
        fileName.MarkDirtyRepaint();
        fileName.RegisterValueChangedCallback(evt => fileName.value = evt.newValue);
        Button saveGraph = new Button(() => RequestFileOperation(true, fileName.value));
        saveGraph.text = "Save Graph";

        Button loadGraph = new Button(() => RequestFileOperation(false, fileName.value));
        loadGraph.text = "Load Graph";

        toolbar.Add(fileName);
        toolbar.Add(saveGraph);
        toolbar.Add(loadGraph);
        toolbar.Add(textNodeCreate);
        toolbar.Add(eventNodeCreate);

        rootVisualElement.Add(toolbar);
    }

    private void ConstructGraphView()
    {
        graphView = new TextGraphView(this)
        {
            name = "Text Graph"
        };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void RequestFileOperation(bool save, string fileName)
    {
        if(string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name", "Please enter a valid name for your file", "OK");
        }
        GraphFileUtility fileUtility = GraphFileUtility.GetInstance(graphView);

        if(save)
        {
            fileUtility.SaveGraph(fileName);
        }
        else
        {
            fileUtility.LoadGraph(fileName);
        }
    }
}
