using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

[DisallowMultipleComponent]
[RequireComponent(typeof(TMP_Text))]
public class TMPPartialShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public string shakeTag = "shake";
    public float shakeAmount = 3f;
    public float shakeSpeed = 5f;
    public bool debugMode;

    private TMP_Text _textComponent;
    private List<int> _shakingIndices = new List<int>();
    private string _lastProcessedText;

    void Awake()
    {
        _textComponent = GetComponent<TMP_Text>();
        ProcessText(true);

        TimerTools.Timer.Once(10, () =>
        {
            SetShakingText("Hello <color=red><shake>World</shake></color>!");
        });
    }

    void Update()
    {
        ApplyShake();
    }

    void ProcessText(bool forceUpdate)
    {
        string originalText = _textComponent.text;
        if (string.IsNullOrEmpty(originalText))
        {
            _lastProcessedText = "";
            _shakingIndices.Clear();
            return;
        }

        StringBuilder cleanText = new StringBuilder(originalText.Length);
        _shakingIndices.Clear();
        bool inShakeTag = false;
        int cleanIndex = 0;

        for (int i = 0; i < originalText.Length;)
        {
            if (originalText[i] == '<')
            {
                int tagEnd = originalText.IndexOf('>', i);
                if (tagEnd == -1) break;

                string fullTag = originalText.Substring(i, tagEnd - i + 1);

                // 处理shake标签
                if (fullTag.StartsWith($"<{shakeTag}") && !fullTag.StartsWith("</"))
                {
                    inShakeTag = true;
                }
                else if (fullTag == $"</{shakeTag}>")
                {
                    inShakeTag = false;
                }
                else
                {
                    // 保留其他标签
                    cleanText.Append(fullTag);
                }

                i = tagEnd + 1;
            }
            else
            {
                cleanText.Append(originalText[i]);

                if (inShakeTag)
                {
                    _shakingIndices.Add(cleanIndex);
                    if (debugMode)
                        Debug.Log($"Shaking char at: {cleanIndex} ('{originalText[i]}')");
                }

                cleanIndex++;
                i++;
            }
        }

        _lastProcessedText = cleanText.ToString();

        // 只有文本实际变化时才更新
        if (forceUpdate || _textComponent.text != _lastProcessedText)
        {
            if (debugMode)
                Debug.Log($"Updating text from:\n{_textComponent.text}\nto:\n{_lastProcessedText}");

            _textComponent.SetText(_lastProcessedText);
        }
    }

    void ApplyShake()
    {
        if (!_textComponent.havePropertiesChanged && _shakingIndices.Count == 0)
            return;

        _textComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = _textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            Vector3[] vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            Vector3 offset = _shakingIndices.Contains(i) ? CalculateOffset(i) : Vector3.zero;

            for (int j = 0; j < 4; j++)
                vertices[charInfo.vertexIndex + j] += offset;
        }

        UpdateMesh(textInfo);
    }

    Vector3 CalculateOffset(int charIndex)
    {
        float time = Time.time * shakeSpeed;
        return new Vector3(
            Mathf.Sin(time + charIndex * 0.3f) * shakeAmount,
            Mathf.Cos(time * 1.3f + charIndex * 0.7f) * shakeAmount,
            0
        );
    }

    void UpdateMesh(TMP_TextInfo textInfo)
    {
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            _textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    public void SetShakingText(string newText)
    {
        _textComponent.textInfo.ClearMeshInfo(true);
        _textComponent.SetText(newText);
        ProcessText(true);
    }
}