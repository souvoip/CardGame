using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DetailInfo
{
    /// <summary>
    /// 标题
    /// </summary>
    [field: SerializeField]
    public string Title { get; set; }
    /// <summary>
    /// 图标
    /// </summary>
    [field: SerializeField]
    public Sprite Icon { get; set; }
    /// <summary>
    /// 内容
    /// </summary>
    [field: SerializeField]
    public string Description { get; set; }
}
