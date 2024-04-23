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
    public GameObject puckObj;

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
    public int screenWidth;
    public int screenHeight;
    public GameObject puckPrefab;
    public GameObject gameFieldObj;
    public float positionCorrection = 500;
    public float posYOffset;
    public float posXOffset;
    public TeamOrganizer teamOrganizer;

    [Header("Point Ranges")]
    public RectTransform footageArea;
    public float pointArea1;
    public float pointArea2;
    public float pointArea3;
    public float pointArea4;

    public void incomingMultipleQRMessages(ZXing.Result[] results, int width, int height)
    {
        screenWidth = width; screenHeight = height;
        Debug.Log(width);
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
                    CreatePuck(currentPuck, result, team);
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
                    CreatePuck(currentPuck, result, team);
                }
            }

            if (registeredPucks == maxPucks)
            {
                StartNewRound();
            }
        }
    }

    public void CreatePuck(string currentPuck, Result result, Teams team)
    {
        Puck newPuck = new Puck(currentPuck, (result.ResultPoints[0].X - positionCorrection) + posXOffset, -(result.ResultPoints[0].Y - positionCorrection) + posYOffset);
        GameObject newPuckObj = Instantiate(puckPrefab, gameFieldObj.transform);
        newPuck.puckObj = newPuckObj;
        team.playerPucks.Add(newPuck);
        SetPointValue(newPuck, team);
        registeredPucks++;
        teamOrganizer.AddTeamColor(newPuck, team);
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
        Vector2 resultPos = new Vector2((result.ResultPoints[0].X - positionCorrection) + posXOffset, -(result.ResultPoints[0].Y - positionCorrection)+posYOffset);
        if (puck.puckPos == resultPos) return;
        puck.puckPos = resultPos;
        puck.puckObj.transform.position = new Vector3(resultPos.x, resultPos.y, puck.puckObj.transform.position.z);
        SetPointValue(puck, team);
    }

    public void SetPointValue(Puck puck, Teams team)
    {
        PuckScript puckLogic = puck.puckObj.GetComponent<PuckScript>();
        puck.value = puckLogic.currentValue;

        int currentTotal = 0;
        foreach (Puck savedPuck in team.playerPucks)
        {
            currentTotal += savedPuck.value;
        }
        team.score = currentTotal;
        teamOrganizer.SendScoresToUI();
    }


}
