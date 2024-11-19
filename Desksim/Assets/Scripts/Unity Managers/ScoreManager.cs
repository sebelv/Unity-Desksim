using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.WSA;
using Unity.VisualScripting;
using System.Globalization;
using System;

public class ScoreManager : MonoBehaviour
{
    private string _path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/Desksim Scores/";

    [SerializeField] private Transform ScoreField;
    

    [SerializeField] private List<Vector2> _highScoreDay = new List<Vector2>();
    [SerializeField] private string _highScoreDayName;
    [SerializeField] private GameObject _dayObject;
    [SerializeField] private LineRenderer _dayRenderer;
    [SerializeField] private Vector3 _knobStartPosition;
    [SerializeField] private Vector2 _lastDayPosition;

    [SerializeField] private List<Vector2> _highScoreAll = new List<Vector2>();
    [SerializeField] private string _highScoreAllName;
    [SerializeField] private GameObject _allObject;
    [SerializeField] private LineRenderer _allRenderer;
    [SerializeField] private Vector2 _lastAllPosition;

    [SerializeField] private List<Vector2> _userScores = new List<Vector2>();
    [SerializeField] private string _username = "Anonym";

    [SerializeField] private float _time = 0;
    [SerializeField] private float _visualsTimer = 0;

    // Graphing scalers
    [SerializeField] float yMultiplier = 200f;
    [SerializeField] float xMultiplier = 20f;

    // Timescale is time between points in data, updateScale is how often the graph objects should move.
    [SerializeField] float timeScale = 2f;
    [SerializeField] float updateScale = 0.5f;

    // Graph Boundaries
    [SerializeField] float boundaryX = 750;
    [SerializeField] float boundaryY = 370;
    // How much the graphed lines should simplify (to avoid filling them with thousands of useless points to render)
    [SerializeField] float simplifyScale = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        SetStartValues();
        WriteToFile("");
    }

    public void SetName(string username)
    {
        _username = username;
    }

    void Update()
    {
        if(_visualsTimer > updateScale)
        {
            IterateVisuals();
            _visualsTimer = 0;
            if(_time > timeScale)
            {
                _time = 0;
                IterateTime();
            }
        }
        else
        {
            _visualsTimer += Time.deltaTime;
            _time += Time.deltaTime;
        }
    }

    private void SetStartValues()
    {
        ReadCompeteFiles();
        _knobStartPosition = _allObject.transform.position;
        _dayRenderer.transform.localScale = new Vector3(xMultiplier, yMultiplier, 1);
        _allRenderer.transform.localScale = new Vector3(xMultiplier, yMultiplier, 1);
    }

    private void ReadCompeteFiles()
    {
        // Set up the path to the score file and check that the directory exists. If not, create the necessary empty files.

        if(!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
            File.Create(_path + "HighScores.txt");
        }
        // Read the score file:
        StreamReader inp_stm = new StreamReader(_path + "HighScores.txt");
        // Read alltime highscore.
        string inp_ln = inp_stm.ReadLine();
        print(inp_ln);
        string[] highScores = inp_ln.Split(";");
        for(int i = 0; i < highScores.Length; i++)
        {
            if(i == highScores.Length - 1)
            {
                string[] highScoreSplit = highScores[i].Split("+|+");
                _highScoreAllName = highScoreSplit[1];
                string[] score = highScoreSplit[0].Split(",");
                _highScoreAll.Add(new Vector2(float.Parse(score[0], CultureInfo.InvariantCulture), float.Parse(score[1], CultureInfo.InvariantCulture)));
            }
            else
            {
                string[] score = highScores[i].Split(",");
                _highScoreAll.Add(new Vector2(float.Parse(score[0], CultureInfo.InvariantCulture), float.Parse(score[1], CultureInfo.InvariantCulture)));
            }
        }
        for(int i = 0; i < 9; i++)
        {
            inp_stm.ReadLine();
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
                _highScoreDayName = highScoreSplit[1];
                string[] score = highScoreSplit[0].Split(",");
                _highScoreDay.Add(new Vector2(float.Parse(score[0], CultureInfo.InvariantCulture), float.Parse(score[1], CultureInfo.InvariantCulture)));
            }
            else
            {
                string[] score = highScores[i].Split(",");
                _highScoreDay.Add(new Vector2(float.Parse(score[0], CultureInfo.InvariantCulture), float.Parse(score[1], CultureInfo.InvariantCulture)));
            }
        }
        inp_stm.Close( );  
    }

    private void IterateTime()
    {
        // If there are points left from read data, use them.
        if(_highScoreDay.Count > 0)
        {
            _lastDayPosition = _highScoreDay[0];
            _highScoreDay.Remove(_highScoreDay[0]);
        }
        // If not, then just move horizontally with time.
        else
        {
            _lastDayPosition.x += timeScale;
        }
        // If there are points left from read data, use them.
        if(_highScoreAll.Count > 0)
        {
            _lastAllPosition = _highScoreAll[0];
            _highScoreAll.Remove(_highScoreAll[0]);
        }
        // If not, then just move horizontally with time.
        else
        {
            _lastAllPosition.x += timeScale;
        }
        // If there are too many points in the lines, simplify the lines by cutting away very close values.
        if(ObtainPositions(_dayRenderer).Count > 200)
        {
            _dayRenderer.Simplify(simplifyScale);
            _allRenderer.Simplify(simplifyScale);
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
            _dayRenderer.transform.localScale = new Vector3(newMultiplier, _dayRenderer.transform.localScale.y, 1);
            _allRenderer.transform.localScale = new Vector3(newMultiplier, _allRenderer.transform.localScale.y, 1);
        }
        else
        {
            noMultiplier = boundaryValue/yMultiplier;
            newMultiplier = boundaryY/noMultiplier;
            print("y");
            print(noMultiplier + " - nomultiplier");
            print(newMultiplier + "new multiplier");

            yMultiplier = newMultiplier;
            _dayRenderer.transform.localScale = new Vector3(_dayRenderer.transform.localScale.x, newMultiplier, 1);
            _allRenderer.transform.localScale = new Vector3(_allRenderer.transform.localScale.x, newMultiplier, 1);
        }
    }

    private void IterateVisuals()
    {
        if(_highScoreDay.Count > 0)
        {
            Vector3 unscaledPosition = new Vector3(((_lastDayPosition.x)*(1-(_time/timeScale)))+((_highScoreDay[0].x)*(_time/timeScale)), ((_lastDayPosition.y)*(1-(_time/timeScale)))+((_highScoreDay[0].y)*(_time/timeScale)), 0);
            _dayObject.transform.position = _knobStartPosition;
            _dayObject.transform.position += new Vector3(unscaledPosition.x*_dayObject.transform.lossyScale.x*xMultiplier, unscaledPosition.y*_dayObject.transform.lossyScale.y*yMultiplier, 0);
            List<Vector3> dayPositions = ObtainPositions(_dayRenderer);
            _dayRenderer.positionCount += 1;
            dayPositions.Add(unscaledPosition);
            print(dayPositions.Count);
            _dayRenderer.SetPositions(dayPositions.ToArray());
            if(unscaledPosition.x*xMultiplier > boundaryX)
            {
                UpdateScale(true, unscaledPosition.x*xMultiplier);
            }
            if(unscaledPosition.y*yMultiplier > boundaryY)
            {
                UpdateScale(false, unscaledPosition.y*yMultiplier);
            }
        }
        else
        {
            Vector3 unscaledPosition = new Vector3(((_lastDayPosition.x)*(1-(_time/timeScale)))+((_lastDayPosition.x+timeScale)*(_time/timeScale)), _lastDayPosition.y, 0);
            _dayObject.transform.position = _knobStartPosition;
            _dayObject.transform.position += new Vector3(unscaledPosition.x*_dayObject.transform.lossyScale.x*xMultiplier, unscaledPosition.y*_dayObject.transform.lossyScale.y*yMultiplier, 0);
            List<Vector3> dayPositions = ObtainPositions(_dayRenderer);
            _dayRenderer.positionCount += 1;
            dayPositions.Add(unscaledPosition);
            print(dayPositions.Count);
            _dayRenderer.SetPositions(dayPositions.ToArray());
            if(unscaledPosition.x*xMultiplier > boundaryX)
            {
                UpdateScale(true, unscaledPosition.x*xMultiplier);
            }
        }
        if(_highScoreAll.Count > 0)
        {
            Vector3 unscaledPosition = new Vector3(((_lastAllPosition.x)*(1-(_time/timeScale)))+((_highScoreAll[0].x)*(_time/timeScale)), ((_lastAllPosition.y)*(1-(_time/timeScale)))+((_highScoreAll[0].y)*(_time/timeScale)), 0);
            _allObject.transform.position = _knobStartPosition;
            _allObject.transform.position += new Vector3(unscaledPosition.x*_allObject.transform.lossyScale.x*xMultiplier, unscaledPosition.y*_allObject.transform.lossyScale.x*yMultiplier, 0);
            List<Vector3> allPositions = ObtainPositions(_allRenderer);
            _allRenderer.positionCount += 1;
            allPositions.Add(unscaledPosition);
            _allRenderer.SetPositions(allPositions.ToArray());
            if(unscaledPosition.x*xMultiplier > boundaryX)
            {
                UpdateScale(true, unscaledPosition.x*xMultiplier);
            }
            if(unscaledPosition.y*yMultiplier > boundaryY)
            {
                UpdateScale(false, unscaledPosition.y*yMultiplier);
            }
        }
        else
        {
            Vector3 unscaledPosition = new Vector3(((_lastAllPosition.x)*(1-(_time/timeScale)))+((_lastAllPosition.x+timeScale)*(_time/timeScale)), _lastAllPosition.y, 0);
            _allObject.transform.position = _knobStartPosition;
            _allObject.transform.position += new Vector3(unscaledPosition.x*_allObject.transform.lossyScale.x*xMultiplier, unscaledPosition.y*_allObject.transform.lossyScale.x*yMultiplier, 0);
            List<Vector3> allPositions = ObtainPositions(_allRenderer);
            _allRenderer.positionCount += 1;
            allPositions.Add(unscaledPosition);
            _allRenderer.SetPositions(allPositions.ToArray());
            if(unscaledPosition.x*xMultiplier > boundaryX)
            {
                UpdateScale(true, unscaledPosition.x*xMultiplier);
            }
        }
    }

    public void WriteToFile(string lastRecordString)
    {
        DateTime date = new DateTime();
        date = DateTime.Now;
        int allPlacement = -1;
        int dayPlacement = -1;

        string userString = "";
        for(int i = 0; i < _userScores.Count; i++)
        {
            userString += _userScores[i].x + "," + _userScores[i].y;
        }
        userString += "+|+" + _username + "+|+" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year;

        StreamReader inp_stm = new StreamReader(_path + "HighScores.txt");
        List<string> highscoreLines = new List<string>();

        for (int i = 0; i < 21; i++)
        {
            highscoreLines.Add(inp_stm.ReadLine());
        }

        for(int i = 0; i < 10; i++)
        {
            if (highscoreLines[i] != "null")
            {
                string[] scoreText = highscoreLines[i].Split("+|+")[0].Split(";");
                if (float.Parse(scoreText[scoreText.Length - 1].Split(",")[1], CultureInfo.InvariantCulture) > _userScores[_userScores.Count - 1].y)
                {
                    allPlacement = i;
                    print(allPlacement + " - AllPlacement");
                    break;
                }
            }
            else
            {
                allPlacement = i;
                print(allPlacement + " - AllPlacement");
                break;
            }
        }

        bool newDay = false;
        if (highscoreLines[10].Split("+|+")[2] != userString.Split("+|+")[2])
        {
            newDay = true;
        }
        else
        {
            for (int i = 10; i < 20; i++)
            {
                if (highscoreLines[i] != "null")
                {
                    string[] scoreText = highscoreLines[i].Split("+|+")[0].Split(";");
                    if (float.Parse(scoreText[scoreText.Length - 1].Split(",")[1], CultureInfo.InvariantCulture) > _userScores[_userScores.Count - 1].y)
                    {
                        dayPlacement = i;
                        print(dayPlacement + " - DayPlacement");
                        break;
                    }
                }
                else
                {
                    dayPlacement = i;
                    print(dayPlacement + " - DayPlacement");
                    break;
                }
            }
        }

        inp_stm.Close();

        StreamWriter inp_stw = new StreamWriter(_path + "HighScores.txt");
        for(int i = 0; i < 20; i++)
        {
            if(i < 10)
            {
                if (allPlacement <= i)
                {
                    if (allPlacement == i)
                    {
                        inp_stw.WriteLine(userString);
                    }
                    else
                    {
                        inp_stw.WriteLine(highscoreLines[i - 1]);
                    }
                }
                else
                {
                    inp_stw.WriteLine(highscoreLines[i]);
                }
            }
            else
            {
                if(newDay)
                {
                    if(i == 10)
                    {
                        inp_stw.WriteLine(userString);
                    }
                    else
                    {
                        inp_stw.WriteLine("null");
                    }
                }
                else
                {
                    if (dayPlacement <= i)
                    {
                        if (dayPlacement == i)
                        {
                            inp_stw.WriteLine(userString);
                        }
                        else
                        {
                            if (dayPlacement != -1)
                            {
                                inp_stw.WriteLine(highscoreLines[i - 1]);
                            }
                            else
                            {
                                inp_stw.WriteLine(highscoreLines[i - 2]);
                            }
                        }
                    }
                    else
                    {
                        inp_stw.WriteLine(highscoreLines[i]);
                    }
                }
            }
        }
        inp_stw.Close();
    }
}
