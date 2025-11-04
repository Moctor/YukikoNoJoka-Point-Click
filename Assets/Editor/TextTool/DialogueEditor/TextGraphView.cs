using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class TextGraphView : GraphView
{
    private readonly Vector2 defautlNodeSize = new Vector2(150, 300);
    private NodeSearchWindow searchWindow;
    public TextGraphView(EditorWindow winstance)
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        AddElement(GenerateEntryPointNode());
        AddSearchWindow(winstance);
    }

    private void AddSearchWindow(EditorWindow winstance)
    {
        searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        searchWindow.Init(this, winstance);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    private Port GeneratePort(TextNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private TextNode GenerateEntryPointNode()
    {
        TextNode entryNode = new TextNode
        {
            title = "START",
            nodeGUID = Guid.NewGuid().ToString(),
            dialogueText = "ENTRYPOINT",
            EntryPoint = true,
            currentType = NodeType.forText
        };

        Port generatedPort = GeneratePort(entryNode, Direction.Output);
        generatedPort.portName = "Next";
        entryNode.outputContainer.Add(generatedPort);

        entryNode.RefreshExpandedState();
        entryNode.RefreshPorts();
        entryNode.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));

        return entryNode;
    }

    private TextNode CreateTextNode(Vector2 pos)
    {
        TextNode textNode = new TextNode
        {
            title = "Text Node",
            nodeGUID = Guid.NewGuid().ToString(),
            dialogueText = "Choice",
            textOption = new List<string>(),
            currentType = NodeType.forText
        };

        Port inputPort = GeneratePort(textNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        textNode.inputContainer.Add(inputPort);

        Button outputButton = new Button(() =>
        {
            AddOutputPort(textNode);
        });
        outputButton.text = "Add Choice";
        textNode.titleContainer.Add(outputButton);

        TextField textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt => 
        {
            textNode.dialogueText = evt.newValue;

        });
        textNode.mainContainer.Add(textField);

        textNode.RefreshExpandedState();
        textNode.RefreshPorts();
        textNode.SetPosition(new Rect(pos, defautlNodeSize));

        return textNode;
    }

    private TextNode CreateEventNode(Vector2 pos)
    {
        TextNode eventNode = new TextNode
        {
            title = "Event Node",
            nodeGUID = Guid.NewGuid().ToString(),
            dialogueText = "Choice",
            currentType = NodeType.forEvent
        };

        Port inputPort = GeneratePort(eventNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        eventNode.inputContainer.Add(inputPort);

        Button outputButton = new Button(() =>
        {
            AddEventPort(eventNode);
        });
        outputButton.text = "Add Choice";
        eventNode.titleContainer.Add(outputButton);

        TextField textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt => 
        {
            eventNode.dialogueText = evt.newValue;

        });
        eventNode.mainContainer.Add(textField);

        eventNode.RefreshExpandedState();
        eventNode.RefreshPorts();
        eventNode.SetPosition(new Rect(pos, defautlNodeSize));

        return eventNode;
    }

    private void AddOutputPort(TextNode textNode, LinksData links = null)
    {
        OutputPort generatedPort = new OutputPort();
        generatedPort.port = GeneratePort(textNode, Direction.Output);
        int outputPortCount = textNode.outputContainer.Query("connector").ToList().Count();

        generatedPort.port.portName = (links == null)? $"Choice {outputPortCount + 1}" : links.portName;
        textNode.textOption.Add((links == null)? generatedPort.port.portName : links.textOption);

        TextField textField = new TextField()
        {
            name = string.Empty,
            value = (links == null)? textNode.textOption[GetChoiceNum(generatedPort.port.portName)] : links.textOption
        };

        textField.RegisterValueChangedCallback(evt => textNode.textOption[GetChoiceNum(generatedPort.port.portName)] = evt.newValue);
        textField.value = (links == null)? textNode.textOption[GetChoiceNum(generatedPort.port.portName)] : links.textOption;
        // text[text.IndexOf(generatedPort.text)] = generatedPort.text;
        generatedPort.port.contentContainer.Add(new Label(""));
        generatedPort.port.contentContainer.Add(textField);

        // textNode.textOption.Add(textField.value);

        Debug.Log(textNode.textOption.Count());
        Button delButton = new Button(() => RemovePort(textNode, generatedPort))
        {
            text = "X"
        };
        generatedPort.port.contentContainer.Add(delButton);

        // Port generatedPort = GeneratePort(textNode, Direction.Output);
        textNode.outputContainer.Add(generatedPort.port);
        // text.Add(generatedPort.text);
        textNode.RefreshExpandedState();
        textNode.RefreshPorts();
    }

    private void AddEventPort(TextNode eventNode, LinksData links = null)
    {
        Port generatedPort = GeneratePort(eventNode, Direction.Output);
        int outputPortCount = eventNode.outputContainer.Query("connector").ToList().Count();

        generatedPort.portName = (links == null)? $"Event_{outputPortCount + 1}" : links.portName;

        TextField textField = new TextField()
        {
            name = string.Empty,
            value = generatedPort.portName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue.Replace(" ", "_"));
        textField.value = generatedPort.portName;
        generatedPort.contentContainer.Add(new Label(""));
        generatedPort.contentContainer.Add(textField);

        Button delButton = new Button(() => RemoveEvent(eventNode, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(delButton);

        eventNode.outputContainer.Add(generatedPort);
        eventNode.RefreshExpandedState();
        eventNode.RefreshPorts();

    }    

    private void RemovePort(TextNode textNode, OutputPort generatedPort)//May let generatedPort.text live, to test
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == generatedPort.port.portName && x.output.node == generatedPort.port.node).ToList();

        if (!targetEdge.Any())
        {
            textNode.textOption.RemoveAt(GetChoiceNum(generatedPort.port.portName));
            RemoveElement(generatedPort.port);
            for (int i = 0; i < textNode.outputContainer.Query("connector").ToList().Count(); i++)
            {
                textNode.outputContainer[i].Q<Port>().portName = $"Choice {i + 1}";

            }
            textNode.RefreshPorts();
            textNode.RefreshExpandedState();
            return;
        }

        Edge edge = targetEdge.First();
        edge.input.Disconnect(edge);
        RemoveElement(targetEdge.First());
        
        textNode.textOption.RemoveAt(GetChoiceNum(generatedPort.port.portName));
        RemoveElement(generatedPort.port);
        for (int i = 0; i < textNode.outputContainer.Query("connector").ToList().Count(); i++)
        {
            textNode.outputContainer[i].Q<Port>().portName = $"Choice {i + 1}";
        }

        textNode.RefreshExpandedState();        
        textNode.RefreshPorts();

        // int currentOuputCount = textNode.outputContainer.Query("connector").ToList().Count();

        // for (int i = 0; i < currentOuputCount; i++)
        // {
        //     Debug.Log(textNode.outputContainer[i].Q<Port>().portName);
        //     textNode.outputContainer[i].Q<Port>().portName = $"Choice {i + 1}";
        // }
        // textNode.RefreshExpandedState();        
        // textNode.RefreshPorts();
    }

    private void RemoveEvent(TextNode node, Port port)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == port.portName && x.output.node == port.node).ToList();

        if (!targetEdge.Any())
        {
            RemoveElement(port);
            node.RefreshPorts();
            node.RefreshExpandedState();
            return;
        }

        Edge edge = targetEdge.First();
        edge.input.Disconnect(edge);
        RemoveElement(targetEdge.First());

        RemoveElement(port);

        node.RefreshExpandedState();        
        node.RefreshPorts();
    }

    public void CreateNode(NodeType nodeType, Vector2 pos)
    {
        switch (nodeType)
        {
            case NodeType.forText:
                AddElement(CreateTextNode(pos));
                break;
            case NodeType.forEvent:
                AddElement(CreateEventNode(pos));
                break;
        }
        
    }

    public int GetChoiceNum(string choice)
    {
        return int.Parse(choice.Replace("Choice ", "")) - 1;
    }

    public void ShowList(TextNode node)
    {
        for (int i = 0; i < node.textOption.Count(); i++)
        {
            Debug.Log(node.textOption[i]);
        }
    }

    public void CreateNode(NodeData nodeData, List<LinksData> linksData)
    {
        if(nodeData.dialogueText == "ENTRYPOINT")
        {
            return;
        }
        string nodeTitle = "";
        switch (nodeData.typeData)
        {
            case NodeType.forText:
                nodeTitle = "Text Node";
                break;
            case NodeType.forEvent:
                nodeTitle = "Event Node";
                break;
        }
        
        TextNode node = new TextNode()
        {
            title = nodeTitle,
            nodeGUID = nodeData.nodeGUID,
            dialogueText = nodeData.dialogueText,
            textOption = (nodeData.typeData == NodeType.forText)? new List<string>(): null,
            currentType = nodeData.typeData
        };

        Port inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        node.inputContainer.Add(inputPort);

        foreach (LinksData links in linksData.Where(x => x.originGUID == node.nodeGUID))
        {
            switch (nodeData.typeData)
            {
                case NodeType.forText:
                    AddOutputPort(node, links);
                    break;
                case NodeType.forEvent:
                    AddEventPort(node, links);
                    break;
            }
        }

        Button outputButton = new Button();

        switch (node.currentType)
        {
            case NodeType.forText:
                outputButton = new Button(() => {AddOutputPort(node); });
                break;
            case NodeType.forEvent:
                outputButton = new Button(() => {AddEventPort(node); });
                break;
        }
        
        outputButton.text = "Add Choice";
        node.titleContainer.Add(outputButton);

        TextField textField = new TextField(string.Empty);
        textField.value = node.dialogueText;
        textField.RegisterValueChangedCallback(evt => 
        {
            node.dialogueText = evt.newValue;
        });
        node.mainContainer.Add(textField);

        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(nodeData.pos, defautlNodeSize));

        AddElement(node);
    }

    public void LinkNodes(Port output, Port input)
    {
        Edge tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);

        AddElement(tempEdge);
    }
}
