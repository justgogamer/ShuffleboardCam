using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Teams
{
    public string teamName;
    public List<string> playerPucks;
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
        //Debug.Log($"Team name: {currentTeamName} - Puck: {currentPuck}");
        foreach (var team in TeamList)
        {
            //Debug.Log($"Saved team name: {team.teamName} / Puck's team name: {currentTeamName}");
            //Debug.Log(!String.Equals(team.teamName, currentTeamName));
            if (!String.Equals(team.teamName, currentTeamName) || team.playerPucks.Count == maxPucks / 2) { 
                //Debug.Log("Not part of current team"); 
                continue; 
            }

            // First puck of the team
            if (team.playerPucks.Count == 0)
            {
                Debug.Log("First puck has been registered!");
                team.score += 1;
                team.playerPucks.Add(currentPuck);
                Debug.Log("Added score!");
            }

            // Every puck after
            foreach (var puck in team.playerPucks)
            {
                //Debug.Log($"Puck is part of team {team.teamName}");
                if (currentPuck == puck) { Debug.Log("Already registered puck"); continue; }
                team.score += 1;
                team.playerPucks.Add(currentPuck);
                Debug.Log("Added score!");
                registeredPucks++;
            }
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
            team.playerPucks.Clear();
        }
        currentRound++;
    }
}
