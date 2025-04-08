using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        //CardDataManager.Init();
        //var ta = CardDataManager.GetAtkCard(1);
        GameObject obj = new GameObject();
        var cb = obj.AddComponent<CharacterBase>();
        cb.ChangeAtkDamageEvent.Add(1, TestFunc);
        cb.ChangeAtkDamageEvent.Add(2, TestFunc);
        cb.CalculateAtkDamage(new Damage(10, 1));
    }

    public void TestFunc(Damage d)
    {
        d.DamageRate += 0.5f;
        d.DamageValue += 5;
    }
}
