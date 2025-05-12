using static GameEventEditorWindow;
using System;
using UnityEditor;
using UnityEngine;
using DialogueEditor;
using UnityEngine.TextCore.Text;

public class EventEditorNode
{
    // Static
    private static GUIStyle titleStyle;
    protected static GUIStyle textStyle;
    // Static styles
    protected static GUIStyle defaultNodeStyle;
    protected static GUIStyle selectedNodeStyle;

    // Static properties
    public static int Width { get { return 200; } }
    public static int Height { get { return 80; } }
    public static Color DefaultColor { get { return DialogueEditorUtil.Colour(0, 158, 118); } }
    public static Color SelectedColor { get { return DialogueEditorUtil.Colour(0, 201, 150); } }

    protected GUIStyle currentBoxStyle;


    public bool isStartNode;

    public Rect rect;
    public GameEventNode NodeData;
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public bool isSelected;

    private Action<ConnectionPoint> OnClickOutPoint;
    private Action<EventEditorNode> OnClickRemoveNode;

    public EventEditorNode(Vector2 position, float width, float height,
        GameEventNode data,
        Action<ConnectionPoint> OnClickOutPoint,
        Action<EventEditorNode> OnClickRemoveNode, bool isStartNode = false)
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
            defaultNodeStyle.normal.background = DialogueEditorUtil.MakeTextureForNode(Width, Height, DefaultColor);
        }
        if (selectedNodeStyle == null || selectedNodeStyle.normal.background == null)
        {
            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = DialogueEditorUtil.MakeTextureForNode(Width, Height, SelectedColor);
        }

        rect = new Rect(position.x, position.y, width, height);
        NodeData = data;
        this.OnClickOutPoint = OnClickOutPoint;
        this.OnClickRemoveNode = OnClickRemoveNode;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out);
        this.isStartNode = isStartNode;

        currentBoxStyle = defaultNodeStyle;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void Draw()
    {
        // Box
        GUI.Box(rect, "", currentBoxStyle);

        OnDraw();

        //GUI.Box(rect, isStartNode ? "Root" : "Node", GetBoxStyle());
        //GUILayout.BeginArea(rect);

        //NodeData.StoryText = EditorGUILayout.TextArea(NodeData.StoryText, GUILayout.Height(50));
        //NodeData.ImgPath = EditorGUILayout.TextField("图片路径", NodeData.ImgPath);

        //GUILayout.Space(10);
        //if (GUILayout.Button("添加选项"))
        //{
        //    NodeData.Choices.Add(new GameEventChoice());
        //}

        //for (int i = 0; i < NodeData.Choices.Count; i++)
        //{
        //    DrawChoice(NodeData.Choices[i], i);
        //}

        //GUILayout.EndArea();

        //inPoint.Draw();
        //outPoint.Draw();
    }

    private void DrawChoice(GameEventChoice choice, int index)
    {
        GUILayout.BeginHorizontal();
        choice.ChoiceText = EditorGUILayout.TextField(choice.ChoiceText);
        if (GUILayout.Button("×", GUILayout.Width(20)))
        {
            NodeData.Choices.RemoveAt(index);
            return;
        }
        GUILayout.EndHorizontal();
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0 && rect.Contains(e.mousePosition))
                {
                    isSelected = true;
                    currentBoxStyle = selectedNodeStyle;
                    GUI.changed = true;
                }
                break;

            case EventType.MouseUp:
                if (e.button == 0 && rect.Contains(e.mousePosition))
                {
                    if (outPoint.ProcessEvents(e))
                    {
                        OnClickOutPoint(outPoint);
                    }
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
            DrawTitle(isSelected ? "Node (selected)." : "Node");
        }
    }

    protected void DrawTitle(string text)
    {
        Rect title = new Rect(rect.x, rect.y, rect.width, 18);
        GUI.Label(title, text, titleStyle);
    }
}