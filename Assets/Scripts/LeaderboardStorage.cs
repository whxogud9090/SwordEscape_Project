using UnityEngine;

public static class LeaderboardStorage
{
    private const string KeyPrefix = "StageBestScore_";

    public static int GetBestScore(int stageIndex)
    {
        return PlayerPrefs.GetInt(GetKey(stageIndex), 0);
    }

    public static bool SaveBestScore(int stageIndex, int score)
    {
        int bestScore = GetBestScore(stageIndex);
        if (score <= bestScore)
        {
            return false;
        }

        PlayerPrefs.SetInt(GetKey(stageIndex), score);
        PlayerPrefs.Save();
        return true;
    }

    public static void DeleteStageScore(int stageIndex)
    {
        PlayerPrefs.DeleteKey(GetKey(stageIndex));
        PlayerPrefs.Save();
    }

    public static void DeleteAllScores(int stageCount)
    {
        for (int i = 1; i <= stageCount; i++)
        {
            PlayerPrefs.DeleteKey(GetKey(i));
        }

        PlayerPrefs.Save();
    }

    private static string GetKey(int stageIndex)
    {
        return KeyPrefix + stageIndex;
    }
}
