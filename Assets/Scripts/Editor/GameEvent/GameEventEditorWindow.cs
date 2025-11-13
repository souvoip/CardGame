using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using DialogueEditor;

public class GameEventEditorWindow : EditorWindow
{
    public int NodeIndexCounter = 0;

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

    public static bool IsSelectElement = false;

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

    private Rect panelRect;
    private GUIStyle panelStyle;
    private GUIStyle panelTitleStyle;
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
        DrawConnections();
        DrawPanel();
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
                    var nextNode = new GameEventChoiceNextNode();
                    EditorConnectionLine newLine = new EditorConnectionLine(tempConnectionLine.outPoint, tempConnectionLine.inPoint, SelectElement, nextNode, OnClickRemoveLine);
                    nextNode.RandomRatio = 1;
                    nextNode.NextNodeIndex = (newLine.inPoint.parentElement as EditorEventNode).NodeData.NodeIndex;
                    (newLine.outPoint.parentElement as EditorChoiceItem).ChoiceData.NextNodes.Add(nextNode);
                    connectionLines.Add(newLine);
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
            if (isShowPanel && panelRect.Contains(Event.current.mousePosition)) { return; }
            ClearSelectState();

            if (!ProcessNodeEvents(Event.current))
            {
                ProcessConnectLineEvents(Event.current);
            }
            if (!IsSelectElement)
            {
                selectedElement = null;
            }
            ProcessEvents(Event.current);
        }

        if (GUI.changed) Repaint();
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
    private bool isShowPanel = false;

    private void DrawPanel()
    {
        if (selectedElement == null)
        {
            isShowPanel = false;
            return;
        }
        isShowPanel = true;
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
                    eventNode.EditorChoices[i].RemoveNode();
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
            // 进入节点触发效果
            GUILayout.Space(10);
            EditorGUILayout.LabelField("进入节点触发效果", GUILayout.MinWidth(120), GUILayout.MaxWidth(120));
            DrawSelectEffect(eventNode.NodeData.EnterEffects);

            // 添加新选择效果按钮
            GUILayout.Space(10);
            if (GUILayout.Button("添加新效果", GUILayout.MaxWidth(160)))
            {
                eventNode.NodeData.EnterEffects.Add(new GameEventTriggerEffectChangeAttribute());
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
            DrawSelectEffect(choiceNode.ChoiceData.TriggerEffects);

            // 添加新选择效果按钮
            GUILayout.Space(10);
            if (GUILayout.Button("添加新选择", GUILayout.MaxWidth(160)))
            {
                choiceNode.ChoiceData.TriggerEffects.Add(new GameEventTriggerEffectChangeAttribute());
            }
        }
        else if (selectedElement is EditorConnectionLine)
        {
            var connectionLine = selectedElement as EditorConnectionLine;
            EditorGUILayout.LabelField($"链接的节点索引: {connectionLine.choiceNextNode.NextNodeIndex}", GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
            // 节点描述编辑
            EditorGUILayout.LabelField("进入该节点的概率", GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
            connectionLine.choiceNextNode.RandomRatio = EditorGUILayout.IntField(connectionLine.choiceNextNode.RandomRatio);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void DrawSelectEffect(List<GameEventTriggerEffect> effects)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("效果" + (i + 1), GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
            // 移除按钮
            GUILayout.Space(40);
            if (GUILayout.Button("移除", GUILayout.MaxWidth(60)))
            {
                effects.RemoveAt(i);
                GUILayout.EndHorizontal();
                break;
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("效果类型", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
            EditorGUI.BeginChangeCheck();
            // 获取当前类型或默认值
            var newType = (EEventEffectType)EditorGUILayout.EnumPopup(
                "",
                effects[i].EffectType, GUILayout.MinWidth(160), GUILayout.MaxWidth(160)
            );
            if (EditorGUI.EndChangeCheck())
            {
                effects[i] = newType switch
                {
                    EEventEffectType.ChangeAttribute => new GameEventTriggerEffectChangeAttribute(),
                    EEventEffectType.ChangeCard => new GameEventTriggerEffectChangeCard(),
                    EEventEffectType.JumpOtherRoom => new GameEventTriggerEffectJumpOther(),
                    EEventEffectType.GetItem => new GameEventTriggerEffectGetItem(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            switch (effects[i].EffectType)
            {
                case EEventEffectType.ChangeAttribute:
                    var ca = (effects[i] as GameEventTriggerEffectChangeAttribute);
                    EditorGUILayout.LabelField("要改变属性类型", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
                    // 获取当前类型或默认值
                    ca.ChangeAttribute = (ERoleAttribute)EditorGUILayout.EnumPopup(ca.ChangeAttribute);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("改变值", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
                    ca.ChangeValue = EditorGUILayout.IntField(ca.ChangeValue);
                    EditorGUILayout.EndHorizontal();
                    // 按照百分比来改变
                    EditorGUILayout.LabelField("按百分比进行改变", GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    //EditorGUI.BeginChangeCheck();
                    EditorGUILayout.BeginHorizontal();
                    ca.CalculateAttribute = (ERoleAttribute)EditorGUILayout.EnumPopup(ca.CalculateAttribute);
                    EditorGUILayout.LabelField("改变比率", GUILayout.MinWidth(60), GUILayout.MaxWidth(60));
                    ca.Rate = EditorGUILayout.FloatField(ca.Rate);
                    EditorGUILayout.EndHorizontal();
                    break;
                case EEventEffectType.ChangeCard:
                    var cc = (effects[i] as GameEventTriggerEffectChangeCard);
                    EditorGUILayout.LabelField("卡牌相关效果", GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    break;
                case EEventEffectType.JumpOtherRoom:
                    var jo = (effects[i] as GameEventTriggerEffectJumpOther);
                    EditorGUILayout.LabelField("跳转其他功能", GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
                    break;
                case EEventEffectType.GetItem:
                    var gi = (effects[i] as GameEventTriggerEffectGetItem);
                    EditorGUILayout.LabelField("获取物品的ID，-1为随机获取", GUILayout.MinWidth(160), GUILayout.MaxWidth(160));
                    gi.itemID = EditorGUILayout.IntField(gi.itemID);
                    if (gi.itemID == -1)
                    {
                        // 随机物品相关设置
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField("随机数量", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
                        gi.randomCount = EditorGUILayout.IntField(gi.randomCount);
                        GUILayout.EndHorizontal();
                        if(gi.randomItemIDs == null)
                        {
                            gi.randomItemIDs = new List<int>();
                        }
                        EditorGUILayout.LabelField("随机物品库", GUILayout.MinWidth(50), GUILayout.MaxWidth(60));
                        for (int j = 0; j < gi.randomItemIDs.Count; j++)
                        {
                            GUILayout.BeginHorizontal();
                            gi.randomItemIDs[j] = EditorGUILayout.IntField(gi.randomItemIDs[j]);
                            if (GUILayout.Button("移除"))
                            {
                                gi.randomItemIDs.RemoveAt(j);
                                GUILayout.EndHorizontal();
                                break;
                            }
                            GUILayout.EndHorizontal();
                        }
                        // 添加
                        if (GUILayout.Button("添加"))
                        {
                            gi.randomItemIDs.Add(gi.randomItemIDs.Count + 1);
                        }
                    }
                    break;
            }
            EditorGUILayout.LabelField($"=== 效果{i + 1} 结束标记 ===", GUILayout.MinWidth(180), GUILayout.MaxWidth(180));
            GUILayout.Space(10);
        }
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

    private bool ProcessNodeEvents(Event e)
    {
        bool temp = false;
        if (nodes != null)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].ProcessEvents(e))
                {
                    GUI.changed = true;
                    temp = true;
                    break;
                }
            }
        }
        return temp;
    }

    private bool ProcessConnectLineEvents(Event e)
    {
        bool temp = false;
        if (connectionLines != null)
        {
            for (int i = connectionLines.Count - 1; i >= 0; i--)
            {
                if (connectionLines[i].ProcessEvents(e))
                {
                    GUI.changed = true;
                    temp = true;
                    break;
                }
            }
        }
        return temp;
    }

    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("创建节点"), false, () => CreateNode(mousePosition));
        if (selectedElement != null)
        {
            menu.AddItem(new GUIContent("删除选择的节点"), false, RemoveSelectElement);
        }
        menu.ShowAsContext();
    }

    private void RemoveSelectElement()
    {
        if (selectedElement == null) return;
        selectedElement.RemoveNode();
        //if (selectedElement is EditorEventNode)
        //{
        //    OnClickRemoveNode(selectedElement as EditorEventNode);
        //}
        if (selectedElement is EditorChoiceItem)
        {
            (selectedElement as EditorChoiceItem).RemoveNode();
        }
        else if (selectedElement is EditorConnectionLine)
        {
            (selectedElement as EditorConnectionLine).RemoveNode();
            connectionLines.Remove(selectedElement as EditorConnectionLine);
        }

        selectedElement = null;
    }

    private EditorEventNode CreateNode(Vector2 position, bool isStartNode = false)
    {
        if (currentData == null) return null;
        var nodeData = new GameEventNode() { NodeIndex = ++NodeIndexCounter };
        var newNode = new EditorEventNode(position,
            nodeData,
            SelectElement,
            OnClickRemoveNode, isStartNode);
        if (!currentData.AllNodes.Exists(x => x.NodeIndex == nodeData.NodeIndex))
        {
            currentData.AllNodes.Add(nodeData);
        }
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
        if (!currentData.AllNodes.Exists(x => x.NodeIndex == nodeData.NodeIndex))
        {
            currentData.AllNodes.Add(nodeData);
        }
        nodes.Add(newNode);
        currentData.StartNode = nodes[0].NodeData; // 第一个节点作为起始节点
        SaveData();
        return newNode;
    }

    private void OnClickRemoveNode(EditorEventNode node)
    {
        // 删除节点
        // 如果节点是起始节点，则无法删除
        if (node.isStartNode)
        {
            Debug.LogError("起始节点无法删除");
            return;
        }
        DeleteNode(node);
    }

    private void OnClickRemoveLine(EditorConnectionLine line)
    {
        connectionLines.Remove(line);
    }

    private void DeleteNode(EditorEventNode node)
    {
        if (nodes.Contains(node))
        {
            nodes.Remove(node);
            currentData.AllNodes.Remove(node.NodeData);
            SaveData();
        }
    }

    private void OnDrag(Vector2 delta)
    {
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
        for (int i = 0; i < currentData.AllNodes.Count; i++)
        {
            if (currentData.AllNodes[i].NodeIndex != currentData.StartNode.NodeIndex)
            {
                CreateNode(currentData.AllNodes[i].EditorPos, currentData.AllNodes[i]);
            }
        }
        // 创建其他节点，item1 节点，item2 节点深度
        Queue<EditorEventNode> nodeQueue = new Queue<EditorEventNode>();
        for (int i = 0; i < nodes.Count; i++)
        {
            nodeQueue.Enqueue(nodes[i]);
        }

        CreatorChildNodes(nodeQueue);
    }

    private void CreatorChildNodes(Queue<EditorEventNode> nodeQueue)
    {
        while (nodeQueue.Count > 0)
        {
            var node = nodeQueue.Dequeue();

            for (int i = 0; i < node.NodeData.Choices.Count; i++)
            {
                var choice = node.EditorChoices[i];
                for (int j = 0; j < choice.ChoiceData.NextNodes.Count; j++)
                {
                    var nextNode = choice.ChoiceData.NextNodes[j];
                    // 判断节点是否已经存在
                    if (!nodes.Exists(x => x.NodeData.NodeIndex == nextNode.NextNodeIndex))
                    {
                        var tempNode = currentData.AllNodes.Find(x => x.NodeIndex == nextNode.NextNodeIndex);

                        var newNode = CreateNode(tempNode.EditorPos, tempNode);
                        nodeQueue.Enqueue(newNode);
                        // 创建连接线
                        CreatorConnectLine(choice.outPoint, newNode.inPoint, nextNode);

                        NodeIndexCounter = Mathf.Max(NodeIndexCounter, newNode.NodeData.NodeIndex);
                    }
                    else
                    {
                        // 创建连接线
                        CreatorConnectLine(choice.outPoint, nodes.Find(x => x.NodeData.NodeIndex == nextNode.NextNodeIndex).inPoint, nextNode);
                    }

                }
            }
        }

    }

    private void CreatorConnectLine(EditorConnectionPoint outPoint, EditorConnectionPoint inPoint, GameEventChoiceNextNode data)
    {
        var line = new EditorConnectionLine(outPoint, inPoint, SelectElement, data, OnClickRemoveLine);
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

    private void ClearSelectState()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            IsSelectElement = false;
            if (selectedElement != null)
            {
                selectedElement.isSelected = false;
            }
        }
    }
}