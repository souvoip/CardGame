using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using DialogueEditor;
using UnityEngine.TextCore.Text;
using static PlasticPipe.Server.MonitorStats;

public class GameEventEditorWindow : EditorWindow
{
    public static bool isCreatingLine = false;

    public static EditorConnectionLine tempConnectionLine;

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
    private List<EditorConnectionLine> connectionLines = new List<EditorConnectionLine>();

    //private EditorEventNode selectedNode;
    public EditorEventElement selectedElement;
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

        if (!isCreatingLine && tempConnectionLine != null)
        {
            // 判断临时链接线是否有效
            var points = GetAllConnectionPoints();
            for (int i = 0; i < points.Count; i++)
            {
                // 判断鼠标位置是否在连接点内
                if (points[i].rect.Contains(Event.current.mousePosition))
                {
                    tempConnectionLine.ResePoint(points[i]);
                    break;
                }
            }
            if (tempConnectionLine.IsValid())
            {
                // 判断是否在同一个节点内
                if (!IsSameNode(tempConnectionLine.inPoint, tempConnectionLine.outPoint))
                {
                    EditorConnectionLine newLine = new EditorConnectionLine(tempConnectionLine.outPoint, tempConnectionLine.inPoint);
                    connectionLines.Add(newLine);
                    (newLine.outPoint.parentElement as EditorChoiceItem).ChoiceData.NextNodes.Add(new GameEventChoiceNextNode() { RandomRatio = 1, NextNode = (newLine.inPoint.parentElement as EditorEventNode).NodeData });
                }
            }
            // 删除临时连接线
            tempConnectionLine = null;
        }

        if (isCreatingLine)
        {
            tempConnectionLine.ProcessEvents(Event.current);
            Repaint();
        }
        else
        {
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);
        }

        if (GUI.changed) Repaint();
    }

    private void Update()
    {

    }

    private void DrawConnections()
    {
        tempConnectionLine?.Draw();

        for (int i = 0; i < connectionLines.Count; i++)
        {
            connectionLines[i].Draw();
        }
    }

    private Vector2 panelVerticalScroll;
    private void DrawPanel()
    {
        if (selectedElement == null) { return; }

        panelRect = new Rect(position.width - 180, 17, 180, position.height - 17);
        if (panelStyle.normal.background == null)
        {
            InitGUIStyles();
        }
        GUILayout.BeginArea(panelRect, panelStyle);
        GUILayout.BeginVertical();
        panelVerticalScroll = GUILayout.BeginScrollView(panelVerticalScroll);

        GUILayout.Space(10);

        if (selectedElement is EditorEventNode)
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
                EditorGUILayout.LabelField($"选择{i + 1}", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
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
            // 添加新选择按钮
            GUILayout.Space(10);
            if (GUILayout.Button("添加新选择", GUILayout.MaxWidth(160)))
            {
                eventNode.NodeData.Choices.Add(new GameEventChoice());
            }
        }
        else if (selectedElement is EditorChoiceItem)
        {
            var choiceNode = selectedElement as EditorChoiceItem;
            // 节点描述编辑
            EditorGUILayout.LabelField("选择描述", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
            choiceNode.ChoiceData.ChoiceText = GUILayout.TextArea(choiceNode.ChoiceData.ChoiceText);

            // 选择效果编辑
            GUILayout.Space(10);
            EditorGUILayout.LabelField("选择效果", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
            for (int i = 0; i < choiceNode.ChoiceData.TriggerEffects.Count; i++)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("效果" + i + 1, GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
                // 移除按钮
                GUILayout.Space(40);
                if (GUILayout.Button("移除", GUILayout.MaxWidth(60)))
                {
                    choiceNode.ChoiceData.TriggerEffects.RemoveAt(i);
                    GUILayout.EndHorizontal();
                    break;
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.LabelField("效果类型", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
                EditorGUI.BeginChangeCheck();
                // 获取当前类型或默认值
                var newType = (EEnemyActionType)EditorGUILayout.EnumPopup(
                    "",
                    choiceNode.ChoiceData.TriggerEffects[i].EffectType, GUILayout.MinWidth(160), GUILayout.MaxWidth(160)
                );
                if (EditorGUI.EndChangeCheck())
                {
                }

                switch (choiceNode.ChoiceData.TriggerEffects[i].EffectType)
                {
                    case EEventEffectType.ChangeAttribute:
                        EditorGUILayout.LabelField("要改变属性类型", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
                        EditorGUI.BeginChangeCheck();
                        // 获取当前类型或默认值
                        var attributeType = (EEnemyActionType)EditorGUILayout.EnumPopup(
                            "",
                            (choiceNode.ChoiceData.TriggerEffects[i] as GameEventTriggerEffectChangeAttribute).Attribute, GUILayout.MinWidth(160), GUILayout.MaxWidth(160)
                        );
                        if (EditorGUI.EndChangeCheck())
                        {
                            (choiceNode.ChoiceData.TriggerEffects[i] as GameEventTriggerEffectChangeAttribute).Attribute = (ERoleAttribute)attributeType;
                        }
                        EditorGUILayout.LabelField("改变值", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
                        (choiceNode.ChoiceData.TriggerEffects[i] as GameEventTriggerEffectChangeAttribute).ChangeValue = EditorGUILayout.IntField((choiceNode.ChoiceData.TriggerEffects[i] as GameEventTriggerEffectChangeAttribute).ChangeValue);
                        break;
                }
            }
            // 添加新选择效果按钮
            GUILayout.Space(10);
            if (GUILayout.Button("添加新选择", GUILayout.MaxWidth(160)))
            {
                choiceNode.ChoiceData.TriggerEffects.Add(new GameEventTriggerEffectChangeAttribute());
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
            currentData.ID = EditorGUILayout.IntField(currentData.ID, GUILayout.MaxWidth(40));
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
                if (e.button == 2 || (e.button == 0 && selectedElement == null))
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

        menu.ShowAsContext();
    }

    private EditorEventNode CreateNode(Vector2 position, bool isStartNode = false)
    {
        if (currentData == null) return null;
        var newNode = new EditorEventNode(position,
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
        var newNode = new EditorEventNode(position,
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
        if (node.isStartNode) { return; }
        DeleteNode(node);
    }

    private void DeleteNode(EditorEventNode node)
    {
        if (nodes.Contains(node))
        {
            nodes.Remove(node);
            SaveData();
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

    private void CreateNewData()
    {
        currentData = CreateInstance<GameEventData>();
        AssetDatabase.CreateAsset(currentData, "Assets/Resources/Data/GameEvent/GameEvent.asset");
        AssetDatabase.SaveAssets();
        LoadData();
    }

    private void SaveData()
    {
        if (currentData == null) return;

        currentData.StartNode = nodes.Count > 0 ? nodes[0].NodeData : null;

        EditorUtility.SetDirty(currentData);
        AssetDatabase.SaveAssets();
    }

    private void LoadData()
    {
        Vector2 start = new Vector2(78, 100);

        nodes.Clear();
        connectionLines.Clear();
        if (currentData == null) { return; }
        if (currentData.StartNode != null)
        {
            CreateNode(currentData.StartNode.EditorPos, currentData.StartNode, true);
        }
        else
        {
            currentData.StartNode = CreateNode(start, true).NodeData;
        }
        // 创建其他节点，item1 节点，item2 节点深度
        Queue<(EditorEventNode, int)> nodeQueue = new Queue<(EditorEventNode, int)>();
        nodeQueue.Enqueue((nodes[0], 0));

        while (nodeQueue.Count > 0)
        {
            var node = nodeQueue.Dequeue();
            CreatorChildNode(node, nodeQueue);
        }
    }

    private void CreatorChildNode((EditorEventNode, int) node, Queue<(EditorEventNode, int)> nodeQueue)
    {
        for (int i = 0; i < node.Item1.NodeData.Choices.Count; i++)
        {
            var choice = node.Item1.EditorChoices[i];
            for (int j = 0; j < choice.ChoiceData.NextNodes.Count; j++)
            {
                var nextNode = choice.ChoiceData.NextNodes[j];
                var newNode = CreateNode(nextNode.NextNode.EditorPos, nextNode.NextNode);
                nodeQueue.Enqueue((newNode, node.Item2 + 1));
                // 创建连接线
                CreatorConnectLine(choice.outPoint, newNode.inPoint);
            }
        }
    }

    private void CreatorConnectLine(EditorConnectionPoint outPoint, EditorConnectionPoint inPoint)
    {
        var line = new EditorConnectionLine(outPoint, inPoint);
        connectionLines.Add(line);
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

    private List<EditorConnectionPoint> GetAllConnectionPoints()
    {
        List<EditorConnectionPoint> points = new List<EditorConnectionPoint>();
        foreach (var node in nodes)
        {
            points.Add(node.inPoint);
            foreach (var outPoint in node.EditorChoices)
            {
                points.Add(outPoint.outPoint);
            }
        }
        return points;
    }

    /// <summary>
    /// 两个链接点是否在同一个节点
    /// </summary>
    private bool IsSameNode(EditorConnectionPoint inPoint, EditorConnectionPoint outPoint)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].inPoint == inPoint)
            {
                for (int j = 0; j < nodes[i].EditorChoices.Count; j++)
                {
                    if (nodes[i].EditorChoices[j].outPoint == outPoint)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        Debug.LogError("存在异常节点");
        return false;
    }
}