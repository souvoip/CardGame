using static GameEventEditorWindow;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


public abstract class EditorEventElement
{
    public abstract EEditorEventElementType Type { get; }

    public bool isSelected;

    public abstract void RemoveNode();
}

public enum EEditorEventElementType
{
    EditorEventNode,
    EditorChoiceItem,
    EditorConnectionPoint,
    EditorConnectionLine
}

public class EditorEventNode : EditorEventElement
{
    public override EEditorEventElementType Type => EEditorEventElementType.EditorEventNode;
    // Consts
    protected const int TEXT_BORDER = 5;
    protected const int TITLE_HEIGHT = 18;
    protected const int TITLE_GAP = 4;
    protected const int TEXT_BOX_HEIGHT = 40;
    public const int LINE_WIDTH = 3;
    protected const float SPRITE_SZ = 40;
    protected const int NAME_HEIGHT = 12;

    // Static
    private static GUIStyle titleStyle;
    public static GUIStyle textStyle;

    protected static GUIStyle nameStyle;
    // Static styles
    protected static GUIStyle defaultNodeStyle;
    protected static GUIStyle selectedNodeStyle;

    // Static properties
    public static int Width { get { return 200; } }
    public static int Height { get { return 150; } }
    public static Color DefaultColor { get { return EventEditorUtility.Colour(0, 158, 118); } }
    public static Color SelectedColor { get { return EventEditorUtility.Colour(0, 201, 150); } }

    protected GUIStyle currentBoxStyle;

    public bool isStartNode;

    public Rect rect;
    public GameEventNode NodeData;
    public List<EditorChoiceItem> EditorChoices = new List<EditorChoiceItem>();

    public EditorConnectionPoint inPoint;

    private Action<EditorEventElement> OnSelectThisElement;
    private Action<EditorEventNode> OnClickRemoveNode;

    public EditorEventNode(Vector2 position,
        GameEventNode data,
        Action<EditorEventElement> OnSelectThisElement,
        Action<EditorEventNode> OnClickRemoveNode, bool isStartNode = false)
    {
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle();
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.normal.textColor = Color.white;
        }
        if (textStyle == null)
        {
            textStyle = new GUIStyle();
            textStyle.normal.textColor = Color.white;
            textStyle.wordWrap = true;
            textStyle.stretchHeight = false;
            textStyle.clipping = TextClipping.Clip;
        }
        if (defaultNodeStyle == null || defaultNodeStyle.normal.background == null)
        {
            defaultNodeStyle = new GUIStyle();
            defaultNodeStyle.normal.background = EventEditorUtility.MakeTextureForNode(Width, Height, DefaultColor);
        }
        if (selectedNodeStyle == null || selectedNodeStyle.normal.background == null)
        {
            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EventEditorUtility.MakeTextureForNode(Width, Height, SelectedColor);
        }
        if (nameStyle == null)
        {
            nameStyle = new GUIStyle();
            nameStyle.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1);
            nameStyle.wordWrap = true;
            nameStyle.stretchHeight = false;
            nameStyle.alignment = TextAnchor.MiddleCenter;
            nameStyle.clipping = TextClipping.Clip;
        }

        rect = new Rect(position.x, position.y, Width, Height);
        NodeData = data;
        NodeData.EditorPos = position;
        this.OnSelectThisElement = OnSelectThisElement;
        this.OnClickRemoveNode = OnClickRemoveNode;
        inPoint = new EditorConnectionPoint(rect.position + new Vector2(3, 3), 14, 14, EConnectionPointType.In, this);
        //inPoint = new ConnectionPoint(this, EConnectionPointType.In);
        //outPoint = new ConnectionPoint(this, EConnectionPointType.Out);
        this.isStartNode = isStartNode;

        currentBoxStyle = defaultNodeStyle;

        for (int i = 0; i < NodeData.Choices.Count; i++)
        {
            // 创建新的选项
            EditorChoices.Add(new EditorChoiceItem(new Vector2(rect.x, rect.y + TITLE_HEIGHT + TITLE_GAP + NAME_HEIGHT + TEXT_BOX_HEIGHT + 10 + i * 20), rect.width, 20, NodeData.Choices[i], OnSelectThisElement, OnClickRemoveChoice));
        }

        // 调节自己的尺寸
        if (EditorChoices.Count > 3)
        {
            rect.height = Height + (EditorChoices.Count - 3) * 20;
            if (isSelected)
            {
                currentBoxStyle.normal.background = EventEditorUtility.MakeTextureForNode(Width, Height + (EditorChoices.Count - 3) * 20, SelectedColor);
            }
            else
            {
                currentBoxStyle.normal.background = EventEditorUtility.MakeTextureForNode(Width, Height + (EditorChoices.Count - 3) * 20, DefaultColor);
            }
        }
        else
        {
            rect.height = Height;
            if (isSelected)
            {
                currentBoxStyle = selectedNodeStyle;
            }
            else
            {
                currentBoxStyle = defaultNodeStyle;
            }
        }
    }

    private void OnClickRemoveChoice(EditorChoiceItem item)
    {
        NodeData.Choices.Remove(item.ChoiceData);
    }

    public override void RemoveNode()
    {
        inPoint?.RemoveNode();
        for(int i = 0; i < EditorChoices.Count; i++)
        {
            EditorChoices[i].RemoveNode();
        }
        OnClickRemoveNode(this);
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        NodeData.EditorPos = rect.position;
        for (int i = 0; i < EditorChoices.Count; i++)
        {
            EditorChoices[i].Drag(delta);
        }
        inPoint.Drag(delta);
    }

    public void Draw()
    {
        // Box
        GUI.Box(rect, "", currentBoxStyle);

        OnDraw();

        inPoint.Draw();
    }

    private void DrawChoice()
    {
        for (int i = 0; i < NodeData.Choices.Count; i++)
        {
            if (EditorChoices.Count <= i)
            {
                // 创建新的选项
                EditorChoices.Add(new EditorChoiceItem(new Vector2(rect.x, rect.y + TITLE_HEIGHT + TITLE_GAP + NAME_HEIGHT + TEXT_BOX_HEIGHT + 10 + i * 20), rect.width, 20, NodeData.Choices[i], OnSelectThisElement, OnClickRemoveChoice));
            }
            EditorChoices[i].UppdateData(NodeData.Choices[i]);
        }
        // 移除多余的选项
        if (EditorChoices.Count > NodeData.Choices.Count)
        {
            for (int i = EditorChoices.Count - 1; i >= NodeData.Choices.Count; i--)
            {
                EditorChoices.RemoveAt(i);
            }
        }

        for (int i = 0; i < EditorChoices.Count; i++)
        {
            EditorChoices[i].Draw();
        }
        // 调节自己的尺寸
        if (EditorChoices.Count > 3)
        {
            rect.height = Height + (EditorChoices.Count - 3) * 20;
            if (isSelected)
            {
                currentBoxStyle.normal.background = EventEditorUtility.MakeTextureForNode(Width, Height + (EditorChoices.Count - 3) * 20, SelectedColor);
            }
            else
            {
                currentBoxStyle.normal.background = EventEditorUtility.MakeTextureForNode(Width, Height + (EditorChoices.Count - 3) * 20, DefaultColor);
            }
        }
        else
        {
            rect.height = Height;
            if (isSelected)
            {
                currentBoxStyle = selectedNodeStyle;
            }
            else
            {
                currentBoxStyle = defaultNodeStyle;
            }
        }
    }

    public bool ProcessEvents(Event e)
    {
        if (rect.Contains(e.mousePosition))
        {
            if (inPoint.ProcessEvents(e)) { return true; }

            for (int i = 0; i < EditorChoices.Count; i++)
            {
                if (EditorChoices[i].ProcessEvents(e)) { return true; }
            }
        }

        switch (e.type)
        {
            case EventType.MouseDown:
                if (rect.Contains(e.mousePosition))
                {
                    isSelected = true;
                    currentBoxStyle = selectedNodeStyle;
                    GUI.changed = true;
                    OnSelectThisElement(this);
                    IsSelectElement = true;
                    return true;
                }
                else if (!rect.Contains(e.mousePosition))
                {
                    currentBoxStyle = defaultNodeStyle;
                    GUI.changed = true;
                    //OnSelectThisElement(null);
                }
                break;
            case EventType.MouseUp:
                if (e.button == 0 && rect.Contains(e.mousePosition))
                {

                }
                break;
            case EventType.MouseDrag:
                if (isSelected && e.button == 0)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Delete && isSelected)
                {
                    OnClickRemoveNode(this);
                    e.Use();
                }
                break;
        }
        return false;
    }

    private void OnDraw()
    {
        if (isStartNode)
        {
            DrawTitle(isSelected ? "[Root] (selected)." : "[Root]");
        }
        else
        {
            DrawTitle(isSelected ? $"Node {NodeData.NodeIndex} (selected)." : $"Node {NodeData.NodeIndex}");
        }

        const int NAME_PADDING = 1;
        Rect imgPath = new Rect(rect.x + TEXT_BORDER * 0.5f, rect.y + NAME_PADDING + TITLE_HEIGHT, rect.width - TEXT_BORDER * 0.5f, NAME_HEIGHT);
        GUI.Box(imgPath, "图片路径: " + NodeData.ImgPath, nameStyle);
        // Icon
        Rect icon = new Rect(rect.x + TEXT_BORDER * 0.5f, rect.y + TITLE_HEIGHT + TITLE_GAP + NAME_HEIGHT, SPRITE_SZ, SPRITE_SZ);
        if (NodeData.ImgPath != null)
        {
            GUI.DrawTexture(icon, Resources.Load<Texture>(ResourcesPaths.EventImgPath + NodeData.ImgPath), ScaleMode.ScaleToFit);
        }
        //DrawInternalText("图片路径: " + NodeData.ImgPath, SPRITE_SZ + 5, NAME_HEIGHT + NAME_PADDING);
        DrawInternalText(NodeData.StoryText, SPRITE_SZ + 5, NAME_HEIGHT + NAME_PADDING);

        // Draw Choices
        DrawChoice();
    }

    protected void DrawTitle(string text)
    {
        Rect title = new Rect(rect.x, rect.y, rect.width, 18);
        GUI.Label(title, text, titleStyle);
    }

    protected void DrawInternalText(string text, float leftOffset = 0, float heightOffset = 0)
    {
        Rect internalText = new Rect(rect.x + TEXT_BORDER + leftOffset,
            rect.y + TITLE_HEIGHT + TITLE_GAP + heightOffset,
            rect.width - TEXT_BORDER * 2 - leftOffset,
            TEXT_BOX_HEIGHT);
        GUI.Box(internalText, text, textStyle);
    }
}


public class EditorChoiceItem : EditorEventElement
{
    public override EEditorEventElementType Type => EEditorEventElementType.EditorChoiceItem;

    public Rect rect;

    public Rect textRect;

    public GameEventChoice ChoiceData;

    public EditorConnectionPoint outPoint;

    public static GUIStyle boxSelectedStyle;
    public static GUIStyle boxUnselectedStyle;

    private Action<EditorEventElement> OnSelectThisElement;
    private Action<EditorChoiceItem> OnClickRemoveNode;

    public EditorChoiceItem(Vector2 position, float width, float height, GameEventChoice data, Action<EditorEventElement> onSelectThisElement, Action<EditorChoiceItem> onClickRemoveNode)
    {
        if (boxSelectedStyle == null || boxSelectedStyle.normal.background == null)
        {
            boxSelectedStyle = new GUIStyle();
            boxSelectedStyle.normal.background = EventEditorUtility.MakeTextureForBox((int)width, (int)height, Color.red);
        }
        if (boxUnselectedStyle == null || boxUnselectedStyle.normal.background == null)
        {
            boxUnselectedStyle = new GUIStyle();
            boxUnselectedStyle.normal.background = EventEditorUtility.MakeTextureForBox((int)width, (int)height, Color.blue);
        }

        rect = new Rect(position.x, position.y, width, height);
        textRect = new Rect(rect.x, rect.y, rect.width - 20, rect.height);
        ChoiceData = data;

        outPoint = new EditorConnectionPoint(new Vector2(rect.x + rect.width - 17, rect.y + 3), 14, 14, EConnectionPointType.Out, this);
        OnSelectThisElement = onSelectThisElement;
        OnClickRemoveNode = onClickRemoveNode;
    }

    public override void RemoveNode()
    {
        outPoint.RemoveNode();
        OnClickRemoveNode(this);
    }

    public void UppdateData(GameEventChoice data)
    {
        ChoiceData = data;
    }

    public void Draw()
    {
        // Box
        GUI.Box(rect, "", isSelected ? boxSelectedStyle : boxUnselectedStyle);
        OnDraw();
        outPoint.Draw();
    }

    protected void OnDraw()
    {
        GUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("选择：" + ChoiceData.ChoiceText, GUILayout.MinWidth(180), GUILayout.MaxWidth(180));
        GUI.Box(textRect, "选择：" + ChoiceData.ChoiceText, EditorEventNode.textStyle);
        GUILayout.EndHorizontal();
    }

    public bool ProcessEvents(Event e)
    {
        if (outPoint.ProcessEvents(e)) { return true; }

        switch (e.type)
        {
            case EventType.MouseDown:
                if (rect.Contains(e.mousePosition))
                {
                    OnSelectThisElement(this);
                    IsSelectElement = true;
                    isSelected = true;
                    return true;
                }
                break;
            case EventType.MouseUp:
                break;
            case EventType.KeyDown:
                break;
        }
        return false;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        textRect.position += delta;
        outPoint.Drag(delta);
    }
}

public class EditorConnectionPoint : EditorEventElement
{
    public override EEditorEventElementType Type => EEditorEventElementType.EditorConnectionPoint;

    public EConnectionPointType ConnectionPointType;

    public GUIStyle inPointStyle;

    public GUIStyle outPointStyle;

    public Rect rect;

    public EditorEventElement parentElement;

    private Action<EditorEventElement> OnSelectThisElement;

    public bool IsValid = true;

    public EditorConnectionPoint(Vector2 position, float width, float height, EConnectionPointType pType, EditorEventElement parent)
    {
        if (inPointStyle == null || inPointStyle.normal.background == null)
        {
            inPointStyle = new GUIStyle();
            inPointStyle.normal.background = EventEditorUtility.MakeCircleTexture((int)width, (int)height, Color.red);
        }
        if (outPointStyle == null || outPointStyle.normal.background == null)
        {
            outPointStyle = new GUIStyle();
            outPointStyle.normal.background = EventEditorUtility.MakeCircleTexture((int)width, (int)height, Color.green);
        }
        ConnectionPointType = pType;
        parentElement = parent;
        rect = new Rect(position.x, position.y, width, height);
    }

    public override void RemoveNode()
    {
        IsValid = false;
    }

    public void Draw()
    {
        if (ConnectionPointType == EConnectionPointType.In)
        {
            GUI.Box(rect, "", inPointStyle);
        }
        else
        {
            GUI.Box(rect, "", outPointStyle);
        }

        // Test Draw Line
        //Vector2 start = rect.center;
        //Vector2 end = new Vector2(rect.center.x + 200, rect.center.y + 200);
        //Vector2 toStart = new Vector2(50, 20);
        //Vector2 toEnd = new Vector2(80, 10);

        //Handles.DrawBezier(start, end, start + toStart, end + toEnd, Color.red, null, 10);
    }
    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (rect.Contains(e.mousePosition))
                {
                    // 创建链接线
                    GameEventEditorWindow.isCreatingLine = true;
                    if (ConnectionPointType == EConnectionPointType.Out)
                    {
                        tempConnectionLine = new EditorConnectionLine(this, null, null, null, null);
                    }
                    else
                    {
                        tempConnectionLine = new EditorConnectionLine(null, this, null, null, null);
                    }
                    tempConnectionLine.isCreating = true;
                    return true;
                }
                break;
            case EventType.MouseUp:
                break;
            case EventType.KeyDown:
                break;
        }
        return false;
    }
}

public enum EConnectionPointType
{
    In,
    Out
}

public class EditorConnectionLine : EditorEventElement
{
    public override EEditorEventElementType Type => EEditorEventElementType.EditorConnectionLine;

    public EditorConnectionPoint outPoint;

    public EditorConnectionPoint inPoint;

    public Vector2 mousePos;

    public bool isCreating = false;

    private Action<EditorEventElement> OnSelectThisElement;

    private Action<EditorConnectionLine> OnRemoveThisLine;

    public GameEventChoiceNextNode choiceNextNode;

    public EditorConnectionLine(EditorConnectionPoint outPos, EditorConnectionPoint inPos, Action<EditorEventElement> OnSelectThisElement, GameEventChoiceNextNode choiceNext, Action<EditorConnectionLine> OnRemoveThisLine)
    {
        this.outPoint = outPos;
        this.inPoint = inPos;
        if (this.outPoint == null && this.inPoint == null)
        {
            Debug.LogError("错误创建");
        }
        else if (this.outPoint != null)
        {
            mousePos = outPos.rect.center;
        }
        else
        {
            mousePos = inPos.rect.center;
        }
        this.OnSelectThisElement = OnSelectThisElement;
        choiceNextNode = choiceNext;
        this.OnRemoveThisLine = OnRemoveThisLine;
    }

    public override void RemoveNode()
    {
        // 移除连接关系
        if (outPoint != null)
        {
            (outPoint.parentElement as EditorChoiceItem).ChoiceData.NextNodes.Remove(choiceNextNode);
        }
        OnRemoveThisLine?.Invoke(this);
    }

    public void ResetOutPoint(EditorConnectionPoint outPos)
    {
        this.outPoint = outPos;
    }

    public void ResetInPoint(EditorConnectionPoint inPos)
    {
        this.inPoint = inPos;
    }

    public void ResePoint(EditorConnectionPoint point)
    {
        if (point.ConnectionPointType == EConnectionPointType.In)
        {
            ResetInPoint(point);
        }
        else
        {
            ResetOutPoint(point);
        }
    }

    public void Draw()
    {
        if (outPoint == null && inPoint == null)
        {
            Debug.LogError("错误创建");
            return;
        }
        if (!isCreating)
        {
            if (!IsValid()) { RemoveNode(); }
        }
        Vector2 start;
        Vector2 end;
        if (outPoint != null) { start = outPoint.rect.center; }
        else { start = mousePos; }
        if (inPoint != null) { end = inPoint.rect.center; }
        else { end = mousePos; }
        if (Vector2.Distance(start, end) < 10) { return; }
        Vector2 toStart = (start - end).normalized;
        Vector2 toEnd = (end - start).normalized;
        Handles.DrawBezier(start, end, start + toStart, end + toEnd, isSelected ? Color.red : Color.yellow, null, 10);
    }

    public bool IsMouseOnLine(Vector2 mousePos)
    {
        if (outPoint == null || inPoint == null) { return false; }
        Vector2 start = outPoint.rect.center;
        Vector2 end = inPoint.rect.center;

        // 计算贝塞尔曲线的控制点
        Vector2 p0 = start;
        Vector2 p1 = 2 * start - end;
        Vector2 p2 = 2 * end - start;
        Vector2 p3 = end;

        int segments = 4;       // 离散化段数，可调整精度
        float threshold = 5f;    // 阈值，对应线宽的一半

        Vector2 previousPoint = p0;
        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector2 currentPoint = CalculateBezierPoint(t, p0, p1, p2, p3);
            if (IsNearSegment(mousePos, previousPoint, currentPoint, threshold))
            {
                return true;
            }
            previousPoint = currentPoint;
        }
        return false;
    }

    // 计算贝塞尔曲线上的点
    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float uu = u * u;
        float uuu = uu * u;
        float tt = t * t;
        float ttt = tt * t;

        Vector2 point = uuu * p0;
        point += 3 * uu * t * p1;
        point += 3 * u * tt * p2;
        point += ttt * p3;

        return point;
    }

    // 判断点是否靠近线段（距离小于阈值）
    private bool IsNearSegment(Vector2 point, Vector2 a, Vector2 b, float threshold)
    {
        Vector2 ab = b - a;
        Vector2 ap = point - a;

        float abLengthSq = ab.sqrMagnitude;
        if (abLengthSq == 0)
            return ap.sqrMagnitude <= threshold * threshold;

        float t = Vector2.Dot(ap, ab) / abLengthSq;
        t = Mathf.Clamp01(t);

        Vector2 closest = a + t * ab;
        return (point - closest).sqrMagnitude <= threshold * threshold;
    }

    public bool ProcessEvents(Event e)
    {
        if (isCreating)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    mousePos = e.mousePosition;
                    break;
                case EventType.MouseUp:
                    isCreatingLine = false;
                    break;
                case EventType.MouseDrag:
                    mousePos = e.mousePosition;
                    break;
            }
        }
        else
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (IsMouseOnLine(e.mousePosition))
                    {
                        OnSelectThisElement.Invoke(this);
                        isSelected = true;
                        IsSelectElement = true;
                        return true;
                    }
                    break;
            }
        }
        return false;
    }

    // 是否有效
    public bool IsValid()
    {
        if(outPoint != null && inPoint != null)
        {
            if(outPoint.IsValid && inPoint.IsValid)
            {
                return true;
            }
        }
        return false;
    }
}