using UnityEngine;
public static class StaticJSONObject
{
    public static bool GetField(this JSONObject data, string fieldName, ref bool value)
    {
        try
        {
            if (data.HasField(fieldName))
            {
                value = data.GetField(fieldName).b;
            }
            else
            {
                return false;
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("转换失败，试图将 " + data.data + "转换为 bool");
            //Debug.LogException(e);
            return false;
        }
    }

    public static bool GetField<T>(this JSONObject data, string fieldName, ref T value) where T : System.Enum
    {
        try
        {
            if (data.HasField(fieldName))
            {
                int i = (int)data.GetField(fieldName).i;
                value = (T)System.Enum.ToObject(typeof(T), i);
            }
            else
            {
                return false;
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("转换失败，试图将 " + data.data + "转换为 Enum");
            //Debug.LogException(e);
            return false;
        }
    }

    public static bool GetField(this JSONObject data, string fieldName, ref float value)
    {
        try
        {
            if (data.HasField(fieldName))
            {
                value = data.GetField(fieldName).f;
            }
            else
            {
                return false;
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("转换失败，试图将 " + data.data + "转换为 float");
            //Debug.LogException(e);
            return false;
        }
    }

    public static bool GetField(this JSONObject data, string fieldName, ref int value)
    {
        try
        {
            if (data.HasField(fieldName))
            {
                value = (int)data.GetField(fieldName).i;
            }
            else
            {
                return false;
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("转换失败，试图将 " + data.data + "转换为 int");
            //Debug.LogException(e);
            return false;
        }
    }

    public static bool GetField(this JSONObject data, string fieldName, ref long value)
    {
        try
        {
            if (data.HasField(fieldName))
            {
                value = data.GetField(fieldName).i;
            }
            else
            {
                return false;
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("转换失败，试图将 " + data.data + "转换为 long");
            //Debug.LogException(e);
            return false;
        }
    }

    public static bool GetField(this JSONObject data, string fieldName, ref string value)
    {
        try
        {
            if (data.HasField(fieldName))
            {
                value = data.GetField(fieldName).str;
            }
            else
            {
                Debug.Log("字段 " + fieldName + " 不存在");
                return false;
            }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("转换失败，试图将 " + data.data + "转换为 string");
            //Debug.LogException(e);
            return false;
        }
    }

}
