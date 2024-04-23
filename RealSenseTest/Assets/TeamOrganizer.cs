using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamOrganizer : MonoBehaviour
{
    [Header("Links")]
    public DataCollector collector;
    private string Team1ID;
    private string Team2ID;

    [Header("UI Elements")]
    public TextMeshProUGUI Team1Name;
    public TextMeshProUGUI Team2Name;
    public TextMeshProUGUI Team1Score;
    public TextMeshProUGUI Team2Score;

    [Header("Team Colors")]
    public Material Team1Color;
    public Material Team2Color;

    private void Awake()
    {
        Team1ID = collector.TeamList[0].teamName;
        Team2ID = collector.TeamList[1].teamName;
        Team1Name.text = Team1ID;
        Team2Name.text = Team2ID;
    }

    public void AddTeamColor(Puck puck, Teams team)
    {
        if (team.teamName == Team1ID) puck.puckObj.GetComponent<MeshRenderer>().material = Team1Color;
        if (team.teamName == Team2ID) puck.puckObj.GetComponent<MeshRenderer>().material = Team2Color;
    }

    public void SendScoresToUI()
    {
        List<Teams> teamsList = collector.TeamList;
        int Team1ScoreValue = teamsList[0].score;
        int Team2ScoreValue = teamsList[1].score;
        Debug.Log($"{Team1ID}: {Team1ScoreValue} - {Team2ID}: {Team2ScoreValue}");

        Team1Score.text = Team1ScoreValue.ToString();
        Team2Score.text = Team2ScoreValue.ToString();
    }
}
