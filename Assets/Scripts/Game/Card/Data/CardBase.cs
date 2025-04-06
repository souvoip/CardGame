using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBase
{
    /// <summary>
    /// ø®∆¨ID
    /// </summary>
    public int ID;
    /// <summary>
    /// ø®∆¨√˚◊÷
    /// </summary>
    public string Name;
    /// <summary>
    /// ø®∆¨√Ë ˆ
    /// </summary>
    public string Desc;
    /// <summary>
    /// ø®∆¨¿‡–Õ
    /// </summary>
    public ECardType Type;
    /// <summary>
    /// ø®∆¨œ°”–∂»
    /// </summary>
    public int Rare;


    public virtual void LoadData(JSONObject data)
    {

    }
}

