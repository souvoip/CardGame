using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public abstract void Show(object args = null);

    public abstract void Hide();
}
