using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class GraphFileUtility
{
    private TextGraphView m_targetGraphView;
    private TextContainer containerCache;
    private List<Edge> edges => m_targetGraphView.edges.ToList();
    private List<TextNode> nodes => m_targetGraphView.nodes.ToList().Cast<TextNode>().ToList();

    public static GraphFileUtility GetInstance(TextGraphView graphView)
    {
        return new GraphFileUtility
        {
            m_targetGraphView = graphView
        };
    }

    public void SaveGraph(string fileName)
    {
        if(!edges.Any())
        {
            return;
        }
        TextContainer textContainer = ScriptableObject.CreateInstance<TextContainer>();


        Edge[] connectedPorts = edges.Where(x => x.input.node != null).ToArray();
        for (int i = 0; i < connectedPorts.Length; i++)
        {
            TextNode inputNode = connectedPorts[i].input.node as TextNode;
            TextNode outputNode = connectedPorts[i].output.node as TextNode;

            int portCount = outputNode.outputContainer.Query<Port>().ToList().Count;
            List<Port> portList = outputNode.outputContainer.Query<Port>().ToList();

            string text = (outputNode.textOption != null)? outputNode.textOption[GetChoiceNum(connectedPorts[i].output.portName)]: "";
            

            textContainer.linksData.Add(new LinksData
            {
                originGUID = outputNode.nodeGUID,
                targetGUID = inputNode.nodeGUID,
                portName = connectedPorts[i].output.portName,
                textOption = text
            });
        }
        
        foreach (TextNode textNode in nodes/*.Where(nodes => !nodes.EntryPoint)*/)
        {

            textContainer.nodesData.Add(new NodeData
            {
                typeData = textNode.currentType,
                nodeGUID = textNode.nodeGUID,
                dialogueText = textNode.dialogueText,
                pos = textNode.GetPosition().position
            });
        }

        if(!Directory.Exists("Assets/Resources"))
        {
            Directory.CreateDirectory("Assets/Resources");
        }
        AssetDatabase.CreateAsset(textContainer, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {
        containerCache = Resources.Load<TextContainer>(fileName);

        if(containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graoh file does not exist!", "OK");
            return;
        }

        ClearGraph();
        LoadNodes();
        ConnectNodes();

    }

    private void ClearGraph()
    {
        foreach (TextNode textNode in nodes.Where(nodes => !nodes.EntryPoint))
        {
            m_targetGraphView.RemoveElement(textNode);
        }

        foreach (Edge edge in edges)
        {
            m_targetGraphView.RemoveElement(edge);
        }
    }

    private void LoadNodes()
    {
        foreach (NodeData nodeData in containerCache.nodesData)
        {
            m_targetGraphView.CreateNode(nodeData, containerCache.linksData);
        }
    }

    private void ConnectNodes()
    {
        foreach (TextNode textNode in nodes.Where(nodes => nodes.EntryPoint))
        {
            List<LinksData> connections = containerCache.linksData.Where(x => x.portName == "Next").ToList();
            string targetGUID = connections[0].targetGUID;
            TextNode targetNode = nodes.First(x => x.nodeGUID == targetGUID);
            m_targetGraphView.LinkNodes((Port)textNode.outputContainer[0], (Port)targetNode.inputContainer[0]);
        }

        int j = 1;
        foreach (TextNode outputNode in nodes.Where(nodes => !nodes.EntryPoint))
        {
            List<LinksData> connections = containerCache.linksData.Where(x => x.originGUID == nodes[j].nodeGUID).ToList();
            for (int k = 0; k < connections.Count(); k++)
            {
                string targetGUID = connections[k].targetGUID;
                TextNode targetNode = nodes.First(x => x.nodeGUID == targetGUID);
                m_targetGraphView.LinkNodes(outputNode.outputContainer[k].Q<Port>(), (Port)targetNode.inputContainer[0]);
            }
            j++;
        }
    }

    public int GetChoiceNum(string choice)
    {
        return int.Parse(choice.Replace("Choice ", "")) - 1;
    }
}
