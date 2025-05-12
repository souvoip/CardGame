using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GameEventEditorWindow : EditorWindow
{
    private GameEventData currentData;
    private List<EventEditorNode> nodes = new List<EventEditorNode>();
    private List<Connection> connections = new List<Connection>();

    private EventEditorNode selectedNode;
    private ConnectionPoint selectedOutPoint;
    private Vector2 offset;
    private Vector2 drag;

    [MenuItem("Tools/Game Event Editor")]
    public static void ShowWindow()
    {
        GetWindow<GameEventEditorWindow>("事件编辑器");
    }

    private void OnEnable()
    {
        wantsMouseMove = true;

        LoadData();
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawConnections();
        DrawToolbar();

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        currentData = (GameEventData)EditorGUILayout.ObjectField(currentData, typeof(GameEventData), false);
        if (GUILayout.Button("新建", EditorStyles.toolbarButton))
        {
            CreateNewData();
        }
        if (GUILayout.Button("保存", EditorStyles.toolbarButton) && currentData != null)
        {
            SaveData();
        }
        if(currentData != null)
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

    private EventEditorNode CreateNode(Vector2 position, bool isStartNode = false)
    {
        if (currentData == null) return null;

        var newNode = new EventEditorNode(position, 200, 150,
            new GameEventNode(),
            OnClickOutPoint,
            OnClickRemoveNode, isStartNode);

        nodes.Add(newNode);
        currentData.StartNode = nodes[0].NodeData; // 第一个节点作为起始节点
        SaveData();
        return newNode;
    }

    private EventEditorNode CreateNode(Vector2 position, GameEventNode nodeData, bool isStartNode = false)
    {
        if (currentData == null) return null;

        var newNode = new EventEditorNode(position, 200, 150,
            nodeData,
            OnClickOutPoint,
            OnClickRemoveNode, isStartNode);

        nodes.Add(newNode);
        currentData.StartNode = nodes[0].NodeData; // 第一个节点作为起始节点
        SaveData();
        return newNode;
    }


    private void OnClickRemoveNode(EventEditorNode node)
    {
    }

    private void DeleteNode(EventEditorNode node)
    {
        if (nodes.Contains(node))
        {
            nodes.Remove(node);
            RemoveConnections(node);
            SaveData();
        }
    }

    private void RemoveConnections(EventEditorNode node)
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
        public EventEditorNode node;
        public ConnectionPointType Type;

        public ConnectionPoint(EventEditorNode node, ConnectionPointType type)
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
        if (currentData == null) { return; }
        nodes.Clear();
        connections.Clear();
        if (currentData.StartNode != null)
        {
            CreateNode(new Vector2(100, 100), currentData.StartNode, true);
        }
        else
        {
            currentData.StartNode = CreateNode(new Vector2(100, 100), true).NodeData;
        }
    }
}