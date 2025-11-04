using System;
using UnityEngine;

[Serializable]
public class NodeData
{
    public NodeType typeData;
    public string nodeGUID;
    public string dialogueText;
    public Vector2 pos;
}
public enum NodeType
{
    forText,
    forEvent
};
