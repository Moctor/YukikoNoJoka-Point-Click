using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;

public class TextNode : Node
{
    public string nodeGUID;

    public string dialogueText;
    public List<string> textOption;
    public bool EntryPoint = false;
    public NodeType currentType;
}

   /* public enum NodeType
    {
        forText,
        forEvent
    };*/
