using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NameData
{
    public List<string> single_surnames;
    public List<string> double_surnames;
    public List<NamePart> male_names;
    public List<NamePart> female_names;
}

[System.Serializable]
public class NamePart
{
    public List<string> part1;
    public List<string> part2;
}

public static class XianxiaNameGenerator
{
    private static NameData nameData;

    // 初始化加载数据
    public static void Initialize()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("XianxiaNames");
        nameData = JsonUtility.FromJson<NameData>(jsonFile.text);
    }

    // 生成男性名字（示例：南宫千绝、墨凌渊）
    public static string GenerateMaleName()
    {
        return GenerateName(nameData.male_names, true);
    }

    // 生成女性名字（示例：慕容雪瑶、苏倾璃）
    public static string GenerateFemaleName()
    {
        return GenerateName(nameData.female_names, false);
    }

    private static string GenerateName(List<NamePart> nameParts, bool isMale)
    {
        // 选择姓氏（20%概率使用复姓）
        string surname = Random.Range(0f, 1f) < 0.2f ?
            nameData.double_surnames[Random.Range(0, nameData.double_surnames.Count)] :
            nameData.single_surnames[Random.Range(0, nameData.single_surnames.Count)];

        // 选择名字模板
        NamePart template = nameParts[Random.Range(0, nameParts.Count)];

        // 构建名字（50%概率双字名）
        string givenName = Random.Range(0f, 1f) < 0.5f ?
            template.part1[Random.Range(0, template.part1.Count)] +
            template.part2[Random.Range(0, template.part2.Count)] :
            template.part2[Random.Range(0, template.part2.Count)];

        // 添加修饰词（10%概率）
        if (Random.Range(0f, 1f) < 0.1f)
        {
            string[] modifiers = isMale ?
                new[] { "尘", "子", "阳", "锋" } :
                new[] { "儿", "仙", "月", "瑶" };
            givenName += modifiers[Random.Range(0, modifiers.Length)];
        }

        return $"{surname}{givenName}";
    }
}