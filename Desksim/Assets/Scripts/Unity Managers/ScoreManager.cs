using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.WSA;
using Unity.VisualScripting;
using System.Globalization;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Transform ScoreField;
    

    [SerializeField] private List<Vector2> highScoreDay = new List<Vector2>();
    [SerializeField] private string highScoreDayName;
    [SerializeField] private GameObject dayObject;
    [SerializeField] private LineRenderer dayRenderer;
    [SerializeField] private Vector3 knobStartPosition;
    [SerializeField] private Vector2 lastDayPosition;

    [SerializeField] private List<Vector2> highScoreAll = new List<Vector2>();
    [SerializeField] private string highScoreAllName;
    [SerializeField] private GameObject allObject;
    [SerializeField] private LineRenderer allRenderer;
    [SerializeField] private Vector2 lastAllPosition;

    [SerializeField] private float time = 0;
    [SerializeField] private float visualsTimer = 0;

    // Graphing scalers
    [SerializeField] float yMultiplier = 200f;
    [SerializeField] float xMultiplier = 20f;

    // Timescale is time between points in data, updateScale is how often the graph objects should move.
    [SerializeField] float timeScale = 2f;
    [SerializeField] float updateScale = 0.5f;

    // Graph Boundaries
    [SerializeField] float boundaryX = 750;
    [SerializeField] float boundaryY = 370;

    // Start is called before the first frame update
    void Start()
    {
        SetStartValues();
    }

    void Update()
    {
        if(visualsTimer > updateScale)
        {
            IterateVisuals();
            visualsTimer = 0;
            if(time > timeScale)
            {
                time = 0;
                IterateTime();
            }
        }
        else
        {
            visualsTimer += Time.deltaTime;
            time += Time.deltaTime;
        }
    }

    private void SetStartValues()
    {
        ReadCompeteFiles();
        knobStartPosition = allObject.transform.position;
        dayRenderer.transform.localScale = new Vector3(xMultiplier, yMultiplier, 1);
        allRenderer.transform.localScale = new Vector3(xMultiplier, yMultiplier, 1);
    }

    private void ReadCompeteFiles()
    {
        // Set up the path to the score file and check that the directory exists. If not, create the necessary empty files.
        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Desksim Scores/";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            File.Create(path + "HighScores.txt");
        }
        // Read the score file:
        StreamReader inp_stm = new StreamReader(path + "HighScores.txt");
        // Read alltime highscore.
        string inp_ln = inp_stm.ReadLine();
        print(inp_ln);
        string[] highScores = inp_ln.Split(";");
        for(int i = 0; i < highScores.Length; i++)
        {
            if(i == highScores.Length - 1)
            {
                string[] highScoreSplit = highScores[i].Split("+|+");
                highScoreAllName = highScoreSplit[1];
                string[] score = highScoreSplit[0].Split(",");
                highScoreAll.Add(new Vector2(float.Parse(score[0], CultureInfo.InvariantCulture), float.Parse(score[1], CultureInfo.InvariantCulture)));
            }
            else
            {
                string[] score = highScores[i].Split(",");
                highScoreAll.Add(new Vector2(float.Parse(score[0], CultureInfo.InvariantCulture), float.Parse(score[1], CultureInfo.InvariantCulture)));
            }
        }
        // Read daily highscore.
        inp_ln = inp_stm.ReadLine();
        print(inp_ln);
        highScores = inp_ln.Split(";");
        for(int i = 0; i < highScores.Length; i++)
        {
            if(i == highScores.Length - 1)
            {
                string[] highScoreSplit = highScores[i].Split("+|+");
                highScoreDayName = highScoreSplit[1];
                string[] score = highScoreSplit[0].Split(",");
                highScoreDay.Add(new Vector2(float.Parse(score[0], CultureInfo.InvariantCulture), float.Parse(score[1], CultureInfo.InvariantCulture)));
            }
            else
            {
                string[] score = highScores[i].Split(",");
                highScoreDay.Add(new Vector2(float.Parse(score[0], CultureInfo.InvariantCulture), float.Parse(score[1], CultureInfo.InvariantCulture)));
            }
        }
        inp_stm.Close( );  
    }

    private void IterateTime()
    {
        if(highScoreDay.Count > 0)
        {
            lastDayPosition = highScoreDay[0];
            highScoreDay.Remove(highScoreDay[0]);
        }
        if(highScoreAll.Count > 0)
        {
            lastAllPosition = highScoreAll[0];
            highScoreAll.Remove(highScoreAll[0]);
        }
    }

    private List<Vector3> ObtainPositions(LineRenderer line)
    {
        List<Vector3> positions = new List<Vector3>();
        for(int i = 0; i < line.positionCount; i++)
        {
            positions.Add(line.GetPosition(i));
        }
        return positions;
    }

    // Updates the boundary scale to make sure the graph stays inside and records values.
    private void UpdateScale(bool horizontal, float boundaryValue)
    {
        float noMultiplier;
        float newMultiplier;

        if(horizontal)
        {
            noMultiplier = boundaryValue/xMultiplier;
            print("x");
            print(noMultiplier + " - nomultiplier");
            newMultiplier = boundaryX/noMultiplier;
            print(newMultiplier + "new multiplier");

            xMultiplier = newMultiplier;
            dayRenderer.transform.localScale = new Vector3(newMultiplier, dayRenderer.transform.localScale.y, 1);
            allRenderer.transform.localScale = new Vector3(newMultiplier, allRenderer.transform.localScale.y, 1);
        }
        else
        {
            noMultiplier = boundaryValue/yMultiplier;
            newMultiplier = boundaryY/noMultiplier;
            print("y");
            print(noMultiplier + " - nomultiplier");
            print(newMultiplier + "new multiplier");

            yMultiplier = newMultiplier;
            dayRenderer.transform.localScale = new Vector3(dayRenderer.transform.localScale.x, newMultiplier, 1);
            allRenderer.transform.localScale = new Vector3(allRenderer.transform.localScale.x, newMultiplier, 1);
        }
    }

    private void IterateVisuals()
    {
        if(highScoreDay.Count > 0)
        {
            Vector3 unscaledPosition = new Vector3(((lastDayPosition.x)*(1-(time/timeScale)))+((highScoreDay[0].x)*(time/timeScale)), ((lastDayPosition.y)*(1-(time/timeScale)))+((highScoreDay[0].y)*(time/timeScale)), 0);
            dayObject.transform.position = knobStartPosition;
            dayObject.transform.position += new Vector3(unscaledPosition.x*dayObject.transform.lossyScale.x*xMultiplier, unscaledPosition.y*dayObject.transform.lossyScale.y*yMultiplier, 0);
            List<Vector3> dayPositions = ObtainPositions(dayRenderer);
            dayRenderer.positionCount += 1;
            dayPositions.Add(unscaledPosition);
            print(dayPositions.Count);
            dayRenderer.SetPositions(dayPositions.ToArray());
            if(unscaledPosition.x*xMultiplier > boundaryX)
            {
                UpdateScale(true, unscaledPosition.x*xMultiplier);
            }
            if(unscaledPosition.y*yMultiplier > boundaryY)
            {
                UpdateScale(false, unscaledPosition.y*yMultiplier);
            }
        }
        if(highScoreAll.Count > 0)
        {
            Vector3 unscaledPosition = new Vector3(((lastAllPosition.x)*(1-(time/timeScale)))+((highScoreAll[0].x)*(time/timeScale)), ((lastAllPosition.y)*(1-(time/timeScale)))+((highScoreAll[0].y)*(time/timeScale)), 0);
            allObject.transform.position = knobStartPosition;
            allObject.transform.position += new Vector3(unscaledPosition.x*allObject.transform.lossyScale.x*xMultiplier, unscaledPosition.y*allObject.transform.lossyScale.x*yMultiplier, 0);
            List<Vector3> allPositions = ObtainPositions(allRenderer);
            allRenderer.positionCount += 1;
            allPositions.Add(unscaledPosition);
            allRenderer.SetPositions(allPositions.ToArray());
            if(unscaledPosition.x*xMultiplier > boundaryX)
            {
                UpdateScale(true, unscaledPosition.x*xMultiplier);
            }
            if(unscaledPosition.y*yMultiplier > boundaryY)
            {
                UpdateScale(false, unscaledPosition.y*yMultiplier);
            }
        }
    }
}
