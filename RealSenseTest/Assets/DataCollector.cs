using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

[System.Serializable]
public class Teams
{
    public string teamName;
    public List<Puck> playerPucks;
    public int score;

    public Teams(string name)
    {
        teamName = name;
    }
}

[System.Serializable]
public class Puck
{
    public string puckId;
    public Vector2 puckPos;
    public int value;

    public Puck(string id, float posX, float posY)
    {
        puckId = id;
        puckPos = new Vector2(posX, posY);
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
    public float screenWidth;
    public float screenHeight;

    [Header("Point Ranges")]
    public RectTransform footageArea;
    public float pointArea1;
    public float pointArea2;
    public float pointArea3;
    public float pointArea4;

    public void incomingMultipleQRMessages(ZXing.Result[] results)
    {
        foreach (var result in results)
        {
            string msg = result.Text;
            string[] splitMsg = msg.Split('/');
            string currentTeamName = splitMsg[0];
            string currentPuck = splitMsg[1];

            //Debug.Log($"Team name: {currentTeamName} - Puck: {currentPuck}");
            foreach (var team in TeamList)
            {
                //Debug.Log($"Saved team name: {team.teamName} / Puck's team name: {currentTeamName}");
                //Debug.Log(!String.Equals(team.teamName, currentTeamName));
                if (!String.Equals(team.teamName, currentTeamName) || team.playerPucks.Count == maxPucks / 2)
                {
                    //Debug.Log("Not part of current team"); 
                    continue;
                }

                // First puck of the team
                if (team.playerPucks.Count == 0)
                {
                    Debug.Log("First puck has been registered!");
                    Puck newPuck = new Puck(currentPuck, result.ResultPoints[0].X, result.ResultPoints[0].Y);
                    team.playerPucks.Add(newPuck);
                    SetPointValue(newPuck, team);
                    //Debug.Log("Added score!");
                }

                // Every puck after
                foreach (Puck puck in team.playerPucks)
                {
                    //Debug.Log($"Result points: {result.ResultPoints[0].X}");
                    //Debug.Log($"Puck is part of team {team.teamName}");
                    if (currentPuck == puck.puckId) 
                    { 
                        Debug.Log("Already registered puck");
                        CheckPosition(puck, result, team);
                        continue; 
                    }

                    team.score += 1;
                    Puck newPuck = new Puck(currentPuck, result.ResultPoints[0].X, result.ResultPoints[0].Y);
                    team.playerPucks.Add(newPuck);
                    SetPointValue(newPuck, team);
                    //Debug.Log("Added score!");
                    registeredPucks++;
                }
            }

            if (registeredPucks == maxPucks)
            {
                StartNewRound();
            }
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

    public void CheckPosition(Puck puck, ZXing.Result result, Teams team)
    {
        Vector2 resultPos = new Vector2(result.ResultPoints[0].X, result.ResultPoints[0].Y);
        if (puck.puckPos == resultPos) return;
        puck.puckPos = resultPos;
        SetPointValue(puck, team);
    }

    public void SetPointValue(Puck puck, Teams team)
    {
        float puckX = puck.puckPos.x;
        float areaHalfX = screenWidth / 2;
        Debug.Log($"Halfway point of area: {areaHalfX}");
        if(puckX < pointArea1 - areaHalfX || puckX > pointArea1 + areaHalfX)
        {
            puck.value = 2;
        }
        else
        {
            puck.value = 1;
        }

        int currentTotal = 0;
        foreach (Puck savedPuck in team.playerPucks)
        {
            currentTotal += savedPuck.value;
        }
        team.score = currentTotal;
    }


}
