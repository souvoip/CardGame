using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using DialogueEditor;
using UnityEngine.TextCore.Text;

public class GameEventEditorWindow : EditorWindow
{
    private static GUIStyle _wrappedStyle;
    public static GUIStyle WrappedTextAreaStyle
    {
        get
        {
            if (_wrappedStyle == null)
            {
                _wrappedStyle = new GUIStyle(EditorStyles.textArea);
                _wrappedStyle.normal.textColor = Color.white;
                _wrappedStyle.wordWrap = true;
                _wrappedStyle.stretchHeight = false;
                _wrappedStyle.clipping = TextClipping.Clip;
            }
            return _wrappedStyle;
        }
    }

    private GameEventData currentData;
    public GameEventData CurrentData
    {
        get { return currentData; }
        set
        {
            if (value == currentData) { return; }
            currentData = value;
            LoadData();
        }
    }
    private List<EditorEventNode> nodes = new List<EditorEventNode>();
    private List<Connection> connections = new List<Connection>();

    private EditorEventNode selectedNode;
    public EditorEventElement selectedElement;
    private ConnectionPoint selectedOutPoint;
    private Vector2 offset;
    private Vector2 drag;

    private Rect panelRect;
    private GUIStyle panelStyle;
    private GUIStyle panelTitleStyle;
    private Rect panelResizerRect;
    private GUIStyle resizerStyle;

    [MenuItem("Tools/Game Event Editor")]
    public static void ShowWindow()
    {
        GetWindow<GameEventEditorWindow>("事件编辑器");
    }

    private void OnEnable()
    {
        wantsMouseMove = true;
        InitGUIStyles();
        LoadData();
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawPanel();
        DrawConnections();
        DrawToolbar();

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private Vector2 panelVerticalScroll;
    private void DrawPanel()
    {
        if(selectedElement == null) { return; }

        panelRect = new Rect(position.width - 180, 17, 180, position.height - 17);
        if (panelStyle.normal.background == null)
        {
            InitGUIStyles();
        }
        GUILayout.BeginArea(panelRect, panelStyle);
        GUILayout.BeginVertical();
        panelVerticalScroll = GUILayout.BeginScrollView(panelVerticalScroll);

        GUILayout.Space(10);

        if(selectedElement is EditorEventNode)
        {
            var eventNode = selectedElement as EditorEventNode;
            // 节点描述编辑
            EditorGUILayout.LabelField("事件描述", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
            eventNode.NodeData.StoryText = GUILayout.TextArea(eventNode.NodeData.StoryText);
            //eventNode.NodeData.StoryText = EditorGUILayout.TextArea(eventNode.NodeData.StoryText, GUILayout.MaxWidth(160), GUILayout.MinHeight(300));
            GUILayout.Space(10);
            EditorGUILayout.LabelField("图片路径", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
            eventNode.NodeData.ImgPath = GUILayout.TextArea(eventNode.NodeData.ImgPath);
            GUILayout.Space(10);
            EditorGUILayout.LabelField("事件选择", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
            for (int i = 0; i < eventNode.NodeData.Choices.Count; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"选择{i+1}", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
                // 移除按钮
                GUILayout.Space(40);
                if (GUILayout.Button("移除", GUILayout.MaxWidth(60)))
                {
                    eventNode.NodeData.Choices.RemoveAt(i);
                    GUILayout.EndHorizontal();
                    break;
                }
                GUILayout.EndHorizontal();
                eventNode.NodeData.Choices[i].ChoiceText = GUILayout.TextArea(eventNode.NodeData.Choices[i].ChoiceText);

                GUILayout.Space(10);
            }
            // 添加按钮
            GUILayout.Space(10);
            if (GUILayout.Button("添加新选择", GUILayout.MaxWidth(160)))
            {
                eventNode.NodeData.Choices.Add(new GameEventChoice());
            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void DrawToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        CurrentData = (GameEventData)EditorGUILayout.ObjectField(currentData, typeof(GameEventData), false);
        if (GUILayout.Button("新建", EditorStyles.toolbarButton))
        {
            CreateNewData();
        }
        if (GUILayout.Button("保存", EditorStyles.toolbarButton) && currentData != null)
        {
            SaveData();
        }
        if (currentData != null)
        {
            EditorGUILayout.LabelField("事件名字: ", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
            currentData.EventName = EditorGUILayout.TextField(currentData.EventName, GUILayout.MaxWidth(200));

            EditorGUILayout.LabelField("事件ID: ", GUILayout.MinWidth(50), GUILayout.MaxWidth(50));
            currentData.ID = int.Parse(EditorGUILayout.TextField(currentData.ID.ToString(), GUILayout.MaxWidth(40)));
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawNodes()
    {
        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,
                new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,
                new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.EndGUI();
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    ShowContextMenu(e.mousePosition);
                }
                break;
            case EventType.MouseDrag:
                if (e.button == 2 || (e.button == 0 && selectedNode == null))
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);
                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("创建节点"), false, () => CreateNode(mousePosition));
        if (selectedNode != null)
        {
            menu.AddItem(new GUIContent("删除节点"), false, () => DeleteNode(selectedNode));
        }
        menu.ShowAsContext();
    }

    private EditorEventNode CreateNode(Vector2 position, bool isStartNode = false)
    {
        if (currentData == null) return null;
        Debug.Log(position);
        var newNode = new EditorEventNode(position, 200, 150,
            new GameEventNode(),
            SelectElement,
            OnClickRemoveNode, isStartNode);

        nodes.Add(newNode);
        currentData.StartNode = nodes[0].NodeData; // 第一个节点作为起始节点
        SaveData();
        return newNode;
    }

    private EditorEventNode CreateNode(Vector2 position, GameEventNode nodeData, bool isStartNode = false)
    {
        if (currentData == null) return null;
        var newNode = new EditorEventNode(position, 200, 150,
            nodeData,
            SelectElement,
            OnClickRemoveNode, isStartNode);

        nodes.Add(newNode);
        currentData.StartNode = nodes[0].NodeData; // 第一个节点作为起始节点
        SaveData();
        return newNode;
    }


    private void OnClickRemoveNode(EditorEventNode node)
    {
        // 删除节点
        // 如果节点是起始节点，则无法删除
        if(node.isStartNode) { return; }
        DeleteNode(node);
    }

    private void DeleteNode(EditorEventNode node)
    {
        if (nodes.Contains(node))
        {
            nodes.Remove(node);
            RemoveConnections(node);
            SaveData();
        }
    }

    private void RemoveConnections(EditorEventNode node)
    {
        List<Connection> connectionsToRemove = new List<Connection>();

        foreach (var connection in connections)
        {
            if (connection.InNode == node.inPoint || connection.OutNode == node.outPoint)
            {
                connectionsToRemove.Add(connection);
            }
        }

        foreach (var connection in connectionsToRemove)
        {
            connections.Remove(connection);
        }
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;
        offset += delta;

        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                node.Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;
        if (selectedOutPoint != null && selectedOutPoint.Type == ConnectionPointType.Out)
        {
            CreateConnection();
            selectedOutPoint = null;
        }
    }

    private void CreateConnection()
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(selectedOutPoint, selectedNode.inPoint, RemoveConnection));
        SaveData();
    }

    private void RemoveConnection(Connection connection)
    {
        connections.Remove(connection);
        SaveData();
    }

    private void CreateNewData()
    {
        currentData = CreateInstance<GameEventData>();
        AssetDatabase.CreateAsset(currentData, "Assets/Resources/Data/GameEvent/GameEvent.asset");
        AssetDatabase.SaveAssets();
        nodes.Clear();
        connections.Clear();
    }

    private void SaveData()
    {
        if (currentData == null) return;

        currentData.StartNode = nodes.Count > 0 ? nodes[0].NodeData : null;

        EditorUtility.SetDirty(currentData);
        AssetDatabase.SaveAssets();
    }

    public class ConnectionPoint
    {
        public Rect rect;
        public EditorEventNode node;
        public ConnectionPointType Type;

        public ConnectionPoint(EditorEventNode node, ConnectionPointType type)
        {
            this.node = node;
            this.Type = type;
            rect = new Rect(0, 0, 20f, 20f);
        }

        public void Draw()
        {
            rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

            switch (Type)
            {
                case ConnectionPointType.In:
                    rect.x = node.rect.x - rect.width + 8f;
                    break;
                case ConnectionPointType.Out:
                    rect.x = node.rect.x + node.rect.width - 8f;
                    break;
            }

            if (GUI.Button(rect, ""))
            {
                ProcessEvents(Event.current);
            }
        }

        public bool ProcessEvents(Event e)
        {
            if (rect.Contains(e.mousePosition))
            {
                return true;
            }
            return false;
        }
    }

    public class Connection
    {
        public ConnectionPoint OutNode;
        public ConnectionPoint InNode;
        private Action<Connection> OnClickRemoveConnection;

        public Connection(ConnectionPoint outPoint, ConnectionPoint inPoint, Action<Connection> OnClickRemoveConnection)
        {
            OutNode = outPoint;
            InNode = inPoint;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }

        public void Draw()
        {
            Handles.DrawBezier(
                OutNode.rect.center,
                InNode.rect.center,
                OutNode.rect.center + Vector2.right * 50f,
                InNode.rect.center - Vector2.right * 50f,
                Color.white,
                null,
                2f
            );

            if (Handles.Button((OutNode.rect.center + InNode.rect.center) * 0.5f,
                Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                OnClickRemoveConnection?.Invoke(this);
            }
        }
    }

    public enum ConnectionPointType
    {
        In,
        Out
    }

    private void LoadData()
    {
        nodes.Clear();
        connections.Clear();
        if (currentData == null) { return; }
        if (currentData.StartNode != null)
        {
            CreateNode(new Vector2(78, 397), currentData.StartNode, true);
        }
        else
        {
            currentData.StartNode = CreateNode(new Vector2(78, 397), true).NodeData;
        }
    }

    private void InitGUIStyles()
    {
        // Panel style
        panelStyle = new GUIStyle();
        panelStyle.normal.background = DialogueEditorUtil.MakeTexture(10, 10, DialogueEditorUtil.GetEditorColor());

        // Panel title style
        panelTitleStyle = new GUIStyle();
        panelTitleStyle.alignment = TextAnchor.MiddleCenter;
        panelTitleStyle.fontStyle = FontStyle.Bold;
        panelTitleStyle.wordWrap = true;
        if (EditorGUIUtility.isProSkin)
        {
            panelTitleStyle.normal.textColor = DialogueEditorUtil.ProSkinTextColour;
        }


        // Resizer style
        resizerStyle = new GUIStyle();
        resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;
    }

    private void SelectElement(EditorEventElement element)
    {
        selectedElement = element;
    }
}