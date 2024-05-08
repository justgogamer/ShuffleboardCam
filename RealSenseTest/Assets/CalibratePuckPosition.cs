using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ZXing;

public class CalibratePuckPosition : MonoBehaviour
{
    public TextMeshProUGUI calibrationText;
    public BackgroundSwitcher backgroundSwitcher;
    public GameObject calibrateScreenObj;
    public GameObject borderParent;
    public GameObject fieldsParent;
    public GameObject scoreOverview;
    public DataCollector collector;
    public GameObject confirmBtn;
    public GameObject applyScreen;
    private Vector2 screenCenter = new (Screen.width * 0.5f , Screen.height * 0.5f);
    private float[] newCorrection = new float[3];
    private Puck demonstratePuck = null;
    private Puck appliedPuck = null;

    public void InitializeCalibration()
    {
        collector.RefreshGame();
        borderParent.SetActive(false);
        fieldsParent.SetActive(false);
        scoreOverview.SetActive(false);
        calibrateScreenObj.SetActive(true);
        if(!backgroundSwitcher.videoBackground.activeInHierarchy) backgroundSwitcher.ToggleBackground();
        confirmBtn.SetActive(false);
        collector.isPaused = true;
    }

    public void CalibratingPuckPos(Result[] results)
    {
        // Only one puck should be present!
        Result result = results[0];
        float currentPosX = result.ResultPoints[0].X;
        float currentPosY = result.ResultPoints[0].Y;

        if (demonstratePuck == null)
        {
            demonstratePuck = new("DemonstrativePuck", currentPosX, currentPosY)
            {
                puckObj = Instantiate(collector.puckPrefab, collector.gameFieldObj.transform)
            };
            demonstratePuck.puckObj.name = demonstratePuck.puckId;
        }
        demonstratePuck.puckObj.transform.position = new Vector3(currentPosX, currentPosY, demonstratePuck.puckObj.transform.position.z);

        float posXDiff = 0 - currentPosX;
        float posYDiff = 0 - currentPosY;
        float posCorrection;

        //Debug.Log($"Differences - X: {posXDiff} & Y: {posYDiff}");

        //if(posXDiff > posYDiff)
        //{
        //    posCorrection = posYDiff;
        //    posXDiff -= posYDiff;
        //    posYDiff = 0;
        //}
        //else
        //{
        //    posCorrection = posXDiff;
        //    posYDiff -= posXDiff;
        //    posXDiff = 0;
        //}

        posCorrection = 0;

        newCorrection =  new float[] { posCorrection, posXDiff, posYDiff};

        if (appliedPuck == null)
        {
            appliedPuck = new("AppliedPuck", currentPosX, currentPosY)
            {
                puckObj = Instantiate(collector.puckPrefab, collector.gameFieldObj.transform)
            };
            appliedPuck.puckObj.name = appliedPuck.puckId;
        }
        appliedPuck.puckObj.transform.position = new Vector3(currentPosX + posXDiff, currentPosY + posYDiff, appliedPuck.puckObj.transform.position.z);

        calibrationText.text = $"Measured calibration - Position correction: {newCorrection[0]}, X correction: {newCorrection[1]}, Y correction {newCorrection[2]}";
        confirmBtn.SetActive(true);
    }

    public void ApplyNewCorrection()
    {
        collector.ApplyCorrection(newCorrection);
        applyScreen.SetActive(true);
        calibrateScreenObj.SetActive(false);
        Destroy(demonstratePuck.puckObj);
        Destroy(appliedPuck.puckObj);
        appliedPuck = null;
        demonstratePuck = null;
    }

    public void ReturnToGame()
    {
        borderParent.SetActive(true);
        fieldsParent.SetActive(true);
        scoreOverview.SetActive(true);
        calibrateScreenObj.SetActive(false);
        applyScreen.SetActive(false);
        if (!backgroundSwitcher.normalBackground.activeInHierarchy) backgroundSwitcher.ToggleBackground();
        collector.RefreshGame();
        collector.isPaused = false;
    }
}
