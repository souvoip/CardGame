using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    public Slider slider;          // 关联的Slider组件
    public Gradient gradient;      // 血量颜色渐变（可选）
    public Image fill;             // 填充的Image组件

    // 初始化最大血量
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        // 渐变颜色初始化（可选）
        fill.color = gradient.Evaluate(1f);
    }

    // 更新当前血量
    public void SetHealth(float health)
    {
        slider.value = health;
        // 根据血量比例更新颜色（可选）
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
