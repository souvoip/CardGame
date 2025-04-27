using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimManager : MonoBehaviour
{
    public static BattleAnimManager Instance;

    public GameObject animPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayAnim(Vector3 position, BattleAnimData animData)
    {
        BattleAnimItem anim = Instantiate(animPrefab, transform).GetComponent<BattleAnimItem>();
        anim.PlayAnim(position, animData);
    }
}
