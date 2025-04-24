using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTools
{
    /// <summary>  
    /// 获取两个向量之间的弧度值0-2π  
    /// </summary>    /// <param name="positionA">点A坐标</param>  
    /// <param name="positionB">点B坐标</param>  
    /// <returns></returns>
    public static float GetAngleInDegrees(Vector3 positionA, Vector3 positionB)
    {
        // 计算从A指向B的向量  
        Vector3 direction = positionB - positionA;
        // 将向量标准化  
        Vector3 normalizedDirection = direction.normalized;
        // 计算夹角的弧度值  
        float dotProduct = Vector3.Dot(normalizedDirection, Vector3.up);
        float angleInRadians = Mathf.Acos(dotProduct);

        //判断夹角的方向：通过计算一个参考向量与两个物体之间的叉乘，可以确定夹角是顺时针还是逆时针方向。这将帮助我们将夹角的范围扩展到0到360度。  
        Vector3 cross = Vector3.Cross(normalizedDirection, Vector3.up);
        if (cross.z > 0)
        {
            angleInRadians = 2 * Mathf.PI - angleInRadians;
        }
        // 将弧度值转换为角度值  
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        return angleInDegrees;
    }
    public static string GetCardTypeeString(ECardType cardType)
    {
        switch(cardType)
        {
            case ECardType.Atk:
                return "攻击";
            case ECardType.Skill:
                return "技能";
            case ECardType.Ability:
                return "能力";
            case ECardType.State:
                return "状态";
            default:
                return "其它";
        }
    }

    public static int CalculateDamage(CharacterBase attacker, CharacterBase defender, Damage damage)
    {
        var tempDamage = new Damage(damage);
        attacker.CalculateAtkDamage(tempDamage);
        if(defender != null)
        {
            defender.CalculateHitDamage(tempDamage);
        }
        return tempDamage.GetDamageValue();
    }

    public static void GetDetailInfosWithCardFeatures(ECardFeatures features, ref List<DetailInfo> detailInfos)
    {
        if((features & ECardFeatures.Void) == ECardFeatures.Void)
        {
            var info = new DetailInfo();
            info.Title = "虚无";
            info.Description = "如果回合结束还在手牌中则消耗";
            detailInfos.Add(info);
        }
        if ((features & ECardFeatures.Hold) == ECardFeatures.Hold)
        {
            var info = new DetailInfo();
            info.Title = "保留";
            info.Description = "回合结束时保留";
            detailInfos.Add(info);
        }
        if ((features & ECardFeatures.Fixed) == ECardFeatures.Fixed)
        {
            var info = new DetailInfo();
            info.Title = "固有";
            info.Description = "在回合开始时获取";
            detailInfos.Add(info);
        }
        if ((features & ECardFeatures.Cost) == ECardFeatures.Cost)
        {
            var info = new DetailInfo();
            info.Title = "消耗";
            info.Description = "使用后在本次战斗中移除";
            detailInfos.Add(info);
        }
    }

    public static float GetAngleBetweenObjects(this Transform tran1, Transform tran2)
    {
        Vector2 direction = tran2.position - tran1.position;
        float angle = Mathf.Atan2(direction.y, direction.x);
        angle = Mathf.Rad2Deg * angle;
        return angle;
    }
}
