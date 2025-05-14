using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EventEditorUtility
{
    public static Texture2D MakeTextureForNode(int width, int height, Color col)
    {
        Texture2D t2d = new Texture2D(width, height);
        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                if (y > height - 20)
                {
                    t2d.SetPixel(x, y, col);
                }
                else
                {
                    t2d.SetPixel(x, y, Color.black);
                }
            }
        }
        t2d.Apply();
        return t2d;
    }

    public static Color Colour(float r, float g, float b)
    {
        return new Color(r / 255f, g / 255f, b / 255f, 1);
    }

    public static Texture2D MakeTextureForBox(int width, int height, Color col)
    {
        Texture2D t2d = new Texture2D(width, height);
        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                t2d.SetPixel(x, y, col);
            }
        }
        t2d.Apply();
        return t2d;
    }

    public static Texture2D MakeCircleTexture(int width, int height, Color col)
    {
        Texture2D tex = new Texture2D(width, height);

        // 计算圆心坐标（浮点数计算）
        float centerX = width / 2f;
        float centerY = height / 2f;

        // 计算半径（取宽高中的最小值作为直径）
        float radius = Mathf.Min(width, height) / 2f;

        // 遍历每个像素
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 计算当前像素到圆心的距离平方
                float dx = x - centerX;
                float dy = y - centerY;
                float distSqr = dx * dx + dy * dy;

                // 如果距离平方 <= 半径平方，则设置颜色
                if (distSqr <= radius * radius)
                {
                    tex.SetPixel(x, y, col);
                }
                else
                {
                    // 圆外部分设为透明
                    tex.SetPixel(x, y, Color.clear);
                }
            }
        }

        tex.Apply();
        return tex;
    }
}
