using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class StageResult
{
    public string playerName;
    public string stageName;
    public int score;
    public string savedAt;

    public StageResult(string playerName, string stageName, int score)
    {
        this.playerName = playerName;
        this.stageName = stageName;
        this.score = score;
        savedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

[Serializable]
public class StageResultList
{
    public List<StageResult> results = new List<StageResult>();
}

public static class StageDataManager
{
    private const string FileName = "stage_rankings.json";

    private static string SavePath
    {
        get { return Path.Combine(Application.persistentDataPath, FileName); }
    }

    public static void SaveStageResult(string playerName, string stageName, int score)
    {
        StageResultList list = LoadAllResults();
        list.results.Add(new StageResult(playerName, stageName, score));

        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(SavePath, json);

        Debug.Log("Ranking saved: " + SavePath);
    }

    public static StageResultList LoadAllResults()
    {
        if (!File.Exists(SavePath))
        {
            return new StageResultList();
        }

        string json = File.ReadAllText(SavePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new StageResultList();
        }

        StageResultList list = JsonUtility.FromJson<StageResultList>(json);
        return list ?? new StageResultList();
    }

    public static List<StageResult> LoadStageResults(string stageName, int maxCount = 20)
    {
        StageResultList list = LoadAllResults();

        return list.results
            .Where(result => result.stageName == stageName)
            .OrderByDescending(result => result.score)
            .Take(maxCount)
            .ToList();
    }

    public static List<string> LoadStageNames()
    {
        StageResultList list = LoadAllResults();

        List<string> names = list.results
            .Select(result => result.stageName)
            .Distinct()
            .OrderBy(stageName => stageName)
            .ToList();

        if (names.Count == 0)
        {
            names.Add("Stage1");
        }

        return names;
    }
}
