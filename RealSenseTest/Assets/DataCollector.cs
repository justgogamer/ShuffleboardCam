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
    public string origin;

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
    public bool isCalibrating = false;
    public bool isPaused = false;
    public CalibratePuckPosition calibratePuckPosition;
    public GameObject mainCamera;
    

    public void incomingMultipleQRMessages(ZXing.Result[] results, int width, int height)
    {
        screenWidth = width; screenHeight = height;

        if (isCalibrating)
        {
            calibratePuckPosition.CalibratingPuckPos(results);
            return;
        }

        if (isPaused)
        {
            return;
        }

        foreach (var result in results)
        {
            string msg = result.Text;
            string[] splitMsg = msg.Split('/');
            string currentTeamName = splitMsg[0];
            string currentPuck = splitMsg[1];

            //Debug.Log($"Team name: {currentTeamName} - Puck: {currentPuck}");
            foreach (var team in TeamList)
            {
                if (!String.Equals(team.teamName, currentTeamName) || team.playerPucks.Count == maxPucks / 2)
                {
                    //Debug.Log("Not part of current team"); 
                    continue;
                }

                // First puck of the team
                if (team.playerPucks.Count == 0)
                {
                    //Debug.Log("First puck has been registered!");
                    CreatePuck(currentPuck, result, team);
                }

                // Every puck after
                foreach (Puck puck in team.playerPucks)
                {
                    //Debug.Log($"Result points: {result.ResultPoints[0].X}");
                    //Debug.Log($"Puck is part of team {team.teamName}");
                    if (currentPuck == puck.puckId) 
                    { 
                        //Debug.Log("Already registered puck");
                        CheckPosition(puck, result);
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
        newPuckObj.name = $"{team.teamName}: {currentPuck}";
        team.playerPucks.Add(newPuck);
        SetPointValue();
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

    public void CheckPosition(Puck puck, ZXing.Result result)
    {
        Vector2 resultPos = new Vector2(result.ResultPoints[0].X, result.ResultPoints[0].Y);
        float xDistanceToCenter = Vector2.Distance(resultPos, Vector2.zero);
        float correctionFactor = Mathf.Clamp01(xDistanceToCenter / (Screen.width / 2f));
        Vector2 correctedPos = new Vector2(resultPos.x + posXOffset, -(resultPos.y + posYOffset)) + new Vector2(1,0) * correctionFactor;
        if (puck.puckPos == correctedPos) return;
        puck.puckPos = correctedPos;

        Vector3 newPos = new Vector3(correctedPos.x, correctedPos.y, puck.puckObj.transform.position.z);
        puck.puckObj.GetComponent<PuckScript>().targetPos = newPos;
        //puck.puckObj.transform.position = newPos;

        SetPointValue();
    }

    public void SetPointValue()
    {
        //TeamList[0] == Red
        //TeamList[1] == Yellow
        Vector3 team1FarthestPos = GetFarthestPosition(TeamList[0]);
        Vector3 team2FarthestPos = GetFarthestPosition(TeamList[1]);

        //Red team pucks vs farthest yellow
        int currentTotalT1 = 0;
        foreach (Puck savedPuck in TeamList[0].playerPucks)
        {
            PuckScript puckScript = savedPuck.puckObj.GetComponent<PuckScript>();
            if (savedPuck.puckPos.x > team2FarthestPos.x) continue;
            //Debug.Log($"Puck {savedPuck.puckObj.name} exceeded the furthest puck of team {TeamList[0].teamName}!");
            //Debug.Log($"Earned {puckScript.currentValue} points!");
            currentTotalT1 += puckScript.currentValue;
        }

        //Yellow team pucks vs farthest red
        int currentTotalT2 = 0;
        foreach (Puck savedPuck in TeamList[1].playerPucks)
        {
            PuckScript puckScript = savedPuck.puckObj.GetComponent<PuckScript>();
            if (savedPuck.puckPos.x > team1FarthestPos.x) continue;
            //Debug.Log($"Puck {savedPuck.puckObj.name} exceeded the furthest puck of team {TeamList[1].teamName}!");
            //Debug.Log($"Earned {puckScript.currentValue} points!");
            currentTotalT2 += puckScript.currentValue;
        }

        TeamList[0].score = currentTotalT1;
        TeamList[1].score = currentTotalT2;
        teamOrganizer.SendScoresToUI();
    }

    public void ToggleCalibrating()
    {
        isCalibrating = !isCalibrating;
        Debug.Log("Toggled calibrating!");
    }

    public void ApplyCorrection(float[] correction)
    {
        positionCorrection = correction[0];
        posXOffset = correction[1];
        posYOffset = correction[2];
        isCalibrating = false;
    }

    public void RefreshGame()
    {
        foreach (Teams team in TeamList)
        {
            foreach(Puck puck in team.playerPucks)
            {
                Destroy(puck.puckObj);
            }
            team.playerPucks.Clear();
            team.score = 0;
        }
    }

    private Vector3 GetFarthestPosition(Teams team)
    {
        Vector3 pos = new Vector3(200, -60, 0); ;
        foreach(Puck puck in team.playerPucks) if (puck.puckPos.x < pos.x) pos = puck.puckPos;

        //Debug.Log($"{team.teamName}'s farthest position is {pos}");
        return pos;
    }
}
