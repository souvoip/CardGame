using static GameEventEditorWindow;
using System;
using UnityEditor;
using UnityEngine;
using DialogueEditor;
using System.Collections.Generic;
using Codice.CM.Common.Mount;
using UnityEngine.TextCore.Text;

public abstract class EditorEventElement
{
    public abstract EEditorEventElementType Type { get; }
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
    protected static GUIStyle textStyle;

    protected static GUIStyle npcNameStyle;
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
    public List<EditorChoiceItem> EditorChoices = new List<EditorChoiceItem>();

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public bool isSelected;

    private Action<ConnectionPoint> OnClickOutPoint;
    private Action<EditorEventElement> OnSelectThisElement;
    private Action<EditorEventNode> OnClickRemoveNode;

    public EditorEventNode(Vector2 position, float width, float height,
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
            defaultNodeStyle.normal.background = DialogueEditorUtil.MakeTextureForNode(Width, Height, DefaultColor);
        }
        if (selectedNodeStyle == null || selectedNodeStyle.normal.background == null)
        {
            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = DialogueEditorUtil.MakeTextureForNode(Width, Height, SelectedColor);
        }
        if (npcNameStyle == null)
        {
            npcNameStyle = new GUIStyle();
            npcNameStyle.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1);
            npcNameStyle.wordWrap = true;
            npcNameStyle.stretchHeight = false;
            npcNameStyle.alignment = TextAnchor.MiddleCenter;
            npcNameStyle.clipping = TextClipping.Clip;
        }

        rect = new Rect(position.x, position.y, width, height);
        NodeData = data;
        this.OnSelectThisElement = OnSelectThisElement;
        this.OnClickRemoveNode = OnClickRemoveNode;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out);
        this.isStartNode = isStartNode;

        currentBoxStyle = defaultNodeStyle;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        for (int i = 0; i < EditorChoices.Count; i++)
        {
            EditorChoices[i].Drag(delta);
        }
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

    private void DrawChoice()
    {
        for (int i = 0; i < NodeData.Choices.Count; i++)
        {
            if (EditorChoices.Count <= i)
            {
                // 创建新的选项
                EditorChoices.Add(new EditorChoiceItem(new Vector2(rect.x, rect.y + TITLE_HEIGHT + TITLE_GAP + NAME_HEIGHT + TEXT_BOX_HEIGHT + 10 + i * 20), rect.width - 20, 20, NodeData.Choices[i]));
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
                    OnSelectThisElement(this);
                }
                else if (!rect.Contains(e.mousePosition))
                {
                    isSelected = false;
                    currentBoxStyle = defaultNodeStyle;
                    GUI.changed = true;
                    OnSelectThisElement(null);
                }
                break;
            case EventType.MouseUp:
                if (e.button == 0 && rect.Contains(e.mousePosition))
                {
                    //if (outPoint.ProcessEvents(e))
                    //{
                    //    OnClickOutPoint(outPoint);
                    //}
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

        const int NAME_PADDING = 1;
        Rect imgPath = new Rect(rect.x + TEXT_BORDER * 0.5f, rect.y + NAME_PADDING + TITLE_HEIGHT, rect.width - TEXT_BORDER * 0.5f, NAME_HEIGHT);
        GUI.Box(imgPath, "图片路径: " + NodeData.ImgPath, npcNameStyle);
        // Icon
        Rect icon = new Rect(rect.x + TEXT_BORDER * 0.5f, rect.y + TITLE_HEIGHT + TITLE_GAP + NAME_HEIGHT, SPRITE_SZ, SPRITE_SZ);
        if (NodeData.ImgPath != null)
        {
            GUI.DrawTexture(icon, Resources.Load<Texture>("Image/Event/" + NodeData.ImgPath), ScaleMode.ScaleToFit);
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

    public GameEventChoice ChoiceData;

    public EditorChoiceItem(Vector2 position, float width, float height, GameEventChoice data)
    {
        rect = new Rect(position.x, position.y, width, height);
        ChoiceData = data;
    }

    public void UppdateData(GameEventChoice data)
    {
        ChoiceData = data;
    }

    public void Draw()
    {
        // Box

        OnDraw();
    }

    protected void OnDraw()
    {
        GUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("选择：" + ChoiceData.ChoiceText, GUILayout.MinWidth(180), GUILayout.MaxWidth(180));
        GUI.Box(rect, "选择：" + ChoiceData.ChoiceText);
        GUILayout.EndHorizontal();
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
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
    }
}

public class EditorConnectionPoint: EditorEventElement
{
    public override EEditorEventElementType Type => EEditorEventElementType.EditorConnectionPoint;

}

public class EditorConnectionLine: EditorEventElement
{
    public override EEditorEventElementType Type => EEditorEventElementType.EditorConnectionLine;

}