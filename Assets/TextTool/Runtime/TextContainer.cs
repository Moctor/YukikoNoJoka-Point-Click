using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextContainer : ScriptableObject
{
    public List<NodeData> nodesData = new List<NodeData>();
    public List<LinksData> linksData = new List<LinksData>();
}
