using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Teams
{
    public string teamName;
    public string[] playerPucks;
    public int score;

    public Teams(string name)
    {
        teamName = name;
    }
}

[System.Serializable]
public class SavedScores
{
    public int round;
    public int score;
    public string teamName;

    public SavedScores(int r, int s, string t) 
    {
        round = r;
        score = s;
        teamName = t;
    }
}

public class DataCollector : MonoBehaviour
{
    public List<Teams> TeamList = new List<Teams>();
    [SerializeField] private List<SavedScores> ScoresList = new List<SavedScores>();
    [SerializeField] private int currentRound = 0;
    [SerializeField] private int registeredPucks = 0;
    [SerializeField] private int maxPucks = 8;

    public void incomingQRMessage(string msg)
    {
        string[] splitMsg = msg.Split('/');
        string currentTeamName = splitMsg[0];
        string currentPuck = splitMsg[1];
        Debug.Log($"Team name: {currentTeamName} - Puck: {currentPuck}");
        try{
            foreach (var team in TeamList)
            {
                if (currentTeamName != team.teamName ||team.playerPucks.Length == maxPucks / 2) continue;
                foreach (var puck in team.playerPucks)
                {
                    if (currentPuck == puck) continue;
                    team.score += 1;
                    team.playerPucks[team.playerPucks.Length - 1] = currentPuck;
                    Debug.Log("Added score!");
                    registeredPucks++;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
        if (registeredPucks == maxPucks)
        {
            StartNewRound();
        }
    }

    public void StartNewRound()
    {
        foreach (var team in TeamList)
        {
            ScoresList.Add(new SavedScores(currentRound, team.score, team.teamName));
            team.score = 0;
            team.playerPucks = new string[0];
        }
        currentRound++;
    }
}
