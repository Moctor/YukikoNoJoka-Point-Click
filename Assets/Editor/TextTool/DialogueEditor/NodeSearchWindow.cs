using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

// [CreateAssetMenu(fileName = "NodeSearchWindow", menuName = "Scriptable Objects/NodeSearchWindow")]
public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private TextGraphView graphView;
    private EditorWindow window;

    public void Init(TextGraphView instance, EditorWindow winstance)
    {
        graphView = instance;
        window = winstance;
    }
    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
            // new SearchTreeGroupEntry(new GUIContent("Create Node"), 1),
            new SearchTreeEntry(new GUIContent("Create Text Node"))
            {
                userData = new int(), 
                level = 1
            },
            new SearchTreeEntry(new GUIContent("Create Event Node"))
            {
                userData = new bool(), 
                level = 1
            }
        };
        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var worldMousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
        var localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);
        switch (SearchTreeEntry.userData)
        {
            case int i:
                graphView.CreateNode(NodeType.forText, localMousePosition);
                return true;
            case bool bi:
                graphView.CreateNode(NodeType.forEvent, localMousePosition);
                return true;
            default:
                return false;
        }
    }
}
