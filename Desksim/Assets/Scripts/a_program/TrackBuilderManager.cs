 using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class TrackBuilderManager : MonoBehaviour
{
    [SerializeField] private Transform trackParent;
    [SerializeField] private List<Track> allTracks = new List<Track>();
    [SerializeField] private List<Section> sectionList = new List<Section>();
    [SerializeField] private List<SwitchTrack> trackList = new List<SwitchTrack>();
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private string scenarioName = "";

    // Start is called before the first frame update
    void Start()
    {
        MyXmlReader.readFile(Application.dataPath + "/StreamingAssets/A_Sectionsfolders/track/" + scenarioName + ".xml");
        BaseObjectXML baseObject = MyXmlReader.getBaseObjectXML();
        MyXmlCreatorTRACK xmlCreatorTRACK = new MyXmlCreatorTRACK();
        xmlCreatorTRACK.createObjects(baseObject.getTopObject().getObjectList());
        for(int i = 0; i < xmlCreatorTRACK.trackCount; i++)
        {
            /*Vector3 startPos = new Vector3(0,0,0);
            Quaternion startRot = new Quaternion();
            for(int x = 0; x < allTracks.Count; x++)
            {
              startPos = allTracks[x].getEndVertex();
              //startRot = allTracks[x].getPivot().rotation;
            }*/
            GameObject newObject = new GameObject();
            newObject.transform.parent = trackParent.transform;
            newObject.name = "Track Section";
            Track newTrack = newObject.AddComponent<Track>();
            //print(xmlCreatorTRACK.getStartVertex(i) + " - Start");
            //print(xmlCreatorTRACK.getEndVertex(i) + "- End");
            newTrack.setStartVertex(xmlCreatorTRACK.getStartVertex(i));
            newTrack.setEndVertex(xmlCreatorTRACK.getEndVertex(i));
            allTracks.Add(newTrack);
            
        }

        // sluttvinkel som brukes av neste seksjon til startkorrigering
        Vector3 startCorrOffsVec = new Vector3();
        Vector3 endCorrOffsVec = new Vector3();
        lesFilStartEndCorrection(startCorrOffsVec, endCorrOffsVec, scenarioName);

        // les kml trase
        KmlSplineTrase kmlst = etablerKMLTrase(scenarioName, Vector3.zero, startCorrOffsVec, endCorrOffsVec);
        for(int i = 0; i < allTracks.Count; i++)
        {
            allTracks[i].init(kmlst);
        }

    // store section data
    Section section = new Section();
    sectionList.Add(section);
    section.setName(name);
    section.setTrase(kmlst);

    foreach (TrackElement t in trackList)
    {
      section.addTrackNodes(t.getAllTrackNodes());
    }

    // koble sporelementer
    List<TrackNode> allStartTrackNodes = new List<TrackNode>();
    foreach (TrackElement t in trackList)
    {
      foreach (TrackNode tn in t.getStartTrackNodes())
      {
        allStartTrackNodes.Add(tn);
      }
    }

    float trackLength = 0;

    List<TrackNode> allEndTrackNodes = new List<TrackNode>();
    foreach (TrackElement t in trackList)
    {
      foreach (TrackNode tn in t.getEndTrackNodes())
      {
        allEndTrackNodes.Add(tn);

        // finn sporlengde
        if (tn.getTempPoint().z > trackLength)
        {
          trackLength = tn.getTempPoint().z;
        }
      }
    }


    if (trackLength > (kmlst.getLengde() + 50))
    {
      Application.Quit();
    }

    foreach (TrackNode ts in allStartTrackNodes)
    {
      foreach (TrackNode te in allEndTrackNodes)
      {
        if (Vector3.Distance(ts.getTempPoint(), te.getTempPoint())  < 0.5f)
        {
          ts.forrige = te;
          te.neste = ts;
        }
      }
    }

    // generer endelige end og start node lister
    section.generateAllStartTrackNodeList();
    section.generateAllEndTrackNodeList();

    // hent ut switch positioner
    foreach (TrackElement t in trackList)
    {
      /*
      if ((t is SwitchTrack) && !(t is CurveTrack))
      {
        switchPositionerList.add(((SwitchTrack) t).getSwitchPositioner());
        section.addSwitchPositioner(((SwitchTrack) t).getSwitchPositioner());
        section.addSoundTriggers(((SwitchTrack) t).getSoundTriggerList());
        
        if (t instanceof KryssTrack)
        {
          switchPositionerList.add(((KryssTrack) t).getSwitchPositioner2());
          section.addSwitchPositioner(((KryssTrack) t).getSwitchPositioner2());          
        }
      }*/
    }
    }

    private void lesFilStartEndCorrection(Vector3 startCorr, Vector3 endCorr, string name)
  {
    print(Application.dataPath + "/StreamingAssets/A_Sectionsfolders/start_end_corr/" + name + ".cor");
    string startEndCorr = TekstfilLeser.lesFil(Application.dataPath + "/StreamingAssets/A_Sectionsfolders/start_end_corr/" + name + ".cor");
    if (startEndCorr == null)
    {
      return;
    }

    string s = startEndCorr.Replace(" ", "");
    string[] sa1 = s.Split(";");

    foreach (string s1 in sa1)
    {
      string[] sa2 = s1.Split("=");

      if (sa2[0] == "startcorrection")
      {
        extractVector(sa2[1], startCorr);
      }
      else
      {
        if (sa2[0] == "endcorrection")
        {
          extractVector(sa2[1], endCorr);
        }
      }
    } 
  }

    private KmlSplineTrase etablerKMLTrase(string sectionFile, Vector3 startWorldPoint,
          Vector3 startCorrOffs, Vector3 endCorrOffs)
    {
        KmlSplineTrase kmlst = new KmlSplineTrase(new Vector3(),new Vector3(),0.0);
        kmlst.InitSplineTrase(0, Application.dataPath + "/StreamingAssets/A_Sectionsfolders/kml/" + sectionFile + ".kml",
            startWorldPoint, startCorrOffs, endCorrOffs);
    AltTextFileReader atr = new AltTextFileReader(Application.dataPath + "/StreamingAssets/A_Sectionsfolders/alt/" + sectionFile + ".alt");
    List<Vector3> elevationProfileList = atr.getPointListe();
    Debug.Log(atr.getPointListe().Count + " - " + atr.getPointListe()[0].x);
    if (elevationProfileList != null)
    {
      KmlSpline spline = kmlst.getSpline();
      //spline.etablerElevationProfile(elevationProfileList);  
    }

    return kmlst;
  }

    private void extractVector(string s, Vector3 vec)
  {
    print(s);
    string[] sa = s.Split(",");
    if (sa.Length != 3)
    {
      return;
    }
        float x = float.Parse(sa[0], CultureInfo.InvariantCulture);
        float y = float.Parse(sa[1], CultureInfo.InvariantCulture);
        float z = float.Parse(sa[2], CultureInfo.InvariantCulture);
        vec.Set(x,y,z);
  }
}
