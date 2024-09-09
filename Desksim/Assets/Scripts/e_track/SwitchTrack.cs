using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrack // : TrackElement
{
    /*
  protected Transform pivotAll;
  protected Transform pivotMovables;
  protected TrackNode first;
  protected float length = 10000;
  protected Vector3 startVertex = new Vector3(0,0,0);
  protected Vector3 endVertex = new Vector3(0,0,20);
  protected List<TrackNode> allTrackNodes = new List<TrackNode>();
  protected List<TrackNode> startTrackNodes = new List<TrackNode>();
  protected List<TrackNode> endTrackNodes = new List<TrackNode>();
  protected List<TrackNode> straightTrackNodes = new List<TrackNode>(); // to be able to remove them in subclass
  protected float radius = 300;
  protected float switchRotY = 0.0f;
  protected Quaternion rotYQuat = new Quaternion();
  protected Transform rotYTransform;
//  protected float switchPosAlpha = 0.0f;
  protected bool directionForward = true;
  protected bool divergingLeft = true;
//  protected Node movableCurvePivot;
//  protected Node movableStraightPivot;
//  protected float currentStraightAngle = 0.0f;
//  protected float currentDivergingAngle = 0.0f;
  protected SwitchPositioner switchPositioner;// = new SwitchPositioner();
  protected Vector3 switchEngineXOffsetVector = new Vector3(-1.5f,0,0);
  protected string id;
  protected string info;
  // trackNodes
  protected TrackNode tnStraightStraightStart;
  protected TrackNode tnStraightStraightEnd;  
  protected TrackNode tnDivergingStraightStart;
  protected TrackNode tnDivergingStraightEnd;
  protected TrackNode tnSwitchDivergingEnd;
  protected TrackNode tnSwitchStraightEnd;
  protected TrackNode tnSwitchStraightStart;
  protected TrackNode tnSwitchDivergingStart;
  protected TrackNode tnStart;
  
  // switch sound
  protected List<TrackConnectedObject> soundTriggerList = new List<TrackConnectedObject>();
  
  
  public SwitchTrack(bool curveTr)
  {
    switchPositioner = new SwitchPositioner(curveTr);    
  }

  public string getId()
  {
    return id;
  }

  public void setId(string id)
  {
    this.id = id;
  }

  public string getInfo()
  {
    return info;
  }

  public void setInfo(string info)
  {
    this.info = info;
  }
  
  public void setStartVertex(Vector3 startVertex) 
  {
    this.startVertex = startVertex;
  }
  
  public void setSwitchEngineXOffset(float x)
  {
    switchEngineXOffsetVector.x = x;
  }

  public void setRotation(float rotation, Transform transform) 
  {
    rotYTransform = transform;
    switchRotY = rotation;
    Vector3 vec = new Vector3(0,1,0);
    float angle = Mathf.Deg2Rad * switchRotY;
    rotYQuat = new Quaternion();
    rotYQuat.ToAngleAxis(out angle, out vec);
    rotYTransform.rotation = rotYQuat;
    
    if (switchRotY >= -90 && switchRotY <= 90)
      directionForward = true;
    else
      directionForward = false;
  }

  public void setRadius(float radius) 
  {
    this.radius = radius;
  }
  
  public void setDivergingLeft(bool divergingLeft)
  {
    this.divergingLeft = divergingLeft;
  }

  public void init(KmlSplineTrase kmlSplineTrase)
  {
    trackMesh(kmlSplineTrase);

    //establishTrackNodes(kmlSplineTrase);
    establishTrackNodesStraight(kmlSplineTrase);
    establishTrackNodesDiverging(kmlSplineTrase);
    connectTrackNodesSwitchPartsStraightParts();


    // init switchpos
    switchPositioner.setId(id);
    switchPositioner.setInfo(info);
    switchPositioner.setDivergingLeft(divergingLeft);
    switchPositioner.setSwitchPosAlpha(0);
    switchPositioner.setTrackPoint(new Vector3(startVertex.x,0,startVertex.z));
    switchPositioner.setOffsetPoint(switchEngineXOffsetVector);
    switchPositioner.setNoTrackNode(true);
    switchPositioner.getTrackNode().getTempPoint().x = startVertex.x;
    switchPositioner.getTrackNode().getTempPoint().y = 0;
    switchPositioner.getTrackNode().getTempPoint().z = startVertex.z;
    if (directionForward)
    {
      switchPositioner.setDirection(1); 
    }
    else
    {
      switchPositioner.setDirection(-1); 
    }
    
    if (id.Contains("swapside"))
    {
      if (directionForward)
      {
        switchPositioner.setDirection(-1); 
      }
      else
      {
        switchPositioner.setDirection(1); 
      }
    }

    if (!(this typeof CurveTrack))
    {
      TrackConnectedObject stDivSt = new TrackConnectedObject();
      stDivSt.setTypeIndex(TrackConnectedObject.TYPE_INDEX_SOUNDTRIGGER_1);
      tnSwitchDivergingStart.setTrackConnectedObject(stDivSt);
      
      TrackConnectedObject stDivEn = new TrackConnectedObject();
      stDivEn.setTypeIndex(TrackConnectedObject.TYPE_INDEX_SOUNDTRIGGER_1);
      tnSwitchDivergingEnd.setTrackConnectedObject(stDivEn);
      
      TrackConnectedObject stStrSt = new TrackConnectedObject();
      stStrSt.setTypeIndex(TrackConnectedObject.TYPE_INDEX_SOUNDTRIGGER_2);
      tnSwitchStraightStart.setTrackConnectedObject(stStrSt);
      
      TrackConnectedObject stStrEn = new TrackConnectedObject();
      stStrEn.setTypeIndex(TrackConnectedObject.TYPE_INDEX_SOUNDTRIGGER_2);
      tnSwitchStraightEnd.setTrackConnectedObject(stStrEn);
      
      TrackConnectedObject spvTrackNodeStartTCO =  new TrackConnectedObject();
      spvTrackNodeStartTCO.setTypeIndex(TrackConnectedObject.TYPE_INDEX_OPPKJORT_SPV_DETECT);
      spvTrackNodeStartTCO.setTrackNode(tnStart);
      tnStart.setTrackConnectedObject(spvTrackNodeStartTCO);
    }
    // sound triggers
    //soundTriggerList.add(initSoundTrigger(startVertex));
    //soundTriggerList.add(initSoundTrigger(p4_lt));
    //soundTriggerList.add(initSoundTrigger(p2_lt));
  }
  
//  private TrackConnectedObject initSoundTrigger(Vector3f v, int dir, int type)
//  {
//    TrackConnectedObject tst = new TrackConnectedObject();
//    tst.setTrackPoint(new Vector3f(v.x,0,v.z));
//    tst.getTrackNode().getTempPoint().x = v.x;
//    tst.getTrackNode().getTempPoint().y = 0;
//    tst.getTrackNode().getTempPoint().z = v.z;
//    tst.setTypeIndex(TrackConnectedObject.TYPE_INDEX_SOUNDTRIGGER_1);
//    
//    return tst;
//  }

  public List<TrackConnectedObject> getSoundTriggerList()
  {
    return soundTriggerList;
  }

  public SwitchPositioner getSwitchPositioner() 
  {
    return switchPositioner;
  }
  
  public Transform getPivot()
    {
      return pivotAll;
    }

  public Transform getPivotMovables()
  {
    return pivotMovables;
  }
  
//  public TrackNode getFirst()
//  {
//    return first;
//  }
    
  public List<TrackNode> getAllTrackNodes() 
  {
    return allTrackNodes;
  }

  public List<TrackNode> getStartTrackNodes() 
  {
    return startTrackNodes;
  }

  public List<TrackNode> getEndTrackNodes() 
  {
    return endTrackNodes;
  }
  
  protected void buildTrackNode(TrackNode forrige)
  {
    
  }
  
  protected float sporbredde = 1.435f;
  protected float sporavstand = 5.0f;
  protected float omkr;// = (radius * 2) * FastMath.PI;
  protected Vector3 p0 = new Vector3();
  protected Vector3 p1 = new Vector3();
  protected Vector3 p2 = new Vector3();
  protected Vector3 p3 = new Vector3();
  protected Vector3 p4 = new Vector3();
  protected Vector3 p5 = new Vector3();
  protected float lengthAlfaMovingParts = 0.95f;
  protected Vector3 p6 = new Vector3();
  protected Vector3 p7 = new Vector3();
  protected Vector3 p8 = new Vector3();
  protected Vector3 p9 = new Vector3();
  protected Vector3 p10 = new Vector3();
  protected Vector3 p11 = new Vector3();
  protected Vector3 p12 = new Vector3();
  protected Vector3 p13 = new Vector3();
  protected Vector3 p14 = new Vector3();
  protected Vector3 p14o = new Vector3();  
  protected Vector3 p15 = new Vector3();
  protected float startAlfaOuterLeadTracks = 0.85f;
 
  protected Vector3 p7w = new Vector3();
  protected Vector3 p8w = new Vector3();
  protected float angP1;
  
  Vector3 p2_lt = new Vector3();
  Vector3 p3_lt = new Vector3();
  Vector3 p4_lt = new Vector3();
  Vector3 p5_lt = new Vector3();
  
  protected void findImportantPoints()
  {
    omkr = omkr = (radius * 2) * Mathf.PI;;
    float hyp_p1 = radius + (sporbredde/2);
    float hos_p1 = radius - (sporbredde / 2);
    float ang_p1 = Mathf.Acos(hos_p1 / hyp_p1);
    angP1 = ang_p1;
    float mot_p1 = Mathf.Sin(ang_p1) * hyp_p1;
    p1.x = radius - hos_p1;
    p1.z = mot_p1;
    float hyp_p2 = radius;
    float hos_p2 = Mathf.Cos(ang_p1) * hyp_p2;
    float mot_p2 = Mathf.Sin(ang_p1) * hyp_p2;
    p2.x = radius - hos_p2;
    p2.z = mot_p2;
    float mot_p3 = (sporavstand / 2)- p2.x;
    float hos_p3 = mot_p3 / Mathf.Tan(ang_p1);
    p3.x = mot_p3 + p2.x;
    p3.z = hos_p3 + p2.z;
    p4.x = 0.0f;
    p4.z = p1.z;
    length = p4.z;
    float hyp_p3 = mot_p3 / Mathf.Sin(ang_p1);
    p5.z = p4.z + hyp_p3;
    p5.x = 0.0f;
    p6.z = p4.z * lengthAlfaMovingParts;
    p7.x = (sporbredde/2);
    p7.z = p6.z;
    p9.x = -(sporbredde/2);
    p9.z = p6.z;
    curveTranslation(p9, p8); // finding p8
    float p10_length = (p4.z - p6.z) * 2;
    float hos_p10 = Mathf.Cos(ang_p1) * p10_length;
    float mot_p10 = Mathf.Sin(ang_p1) * p10_length;
    p10.x = p7.x + mot_p10;
    p10.z = p7.z + hos_p10;
    p11.x = p8.x;
    p11.z = p8.z + p10_length;
    p14.x = -(sporbredde/2) + 0.15f;
    p14.z = p4.z * startAlfaOuterLeadTracks;
    p14o.x = (sporbredde/2) - 0.15f;
    p14o.z = p14.z;
    float p15_length = (p4.z - p14.z) * 1.5f;
    p15.x = p14.x;
    p15.z = p14.z + p15_length;
    curveTranslation(p14o, p12); // finding p12
    float hos_p13 = Mathf.Cos(ang_p1) * p15_length;
    float mot_p13 = Mathf.Sin(ang_p1) * p15_length;
    p13.x = p12.x + mot_p13;
    p13.z = p12.z + hos_p13;
    
    
    
    
//    float p12_hyp = sporbredde - 0.1f;

//    p12.x = p8.x + hos_p12;
//    p12.z = p8.z - mot_p12;
//    //float p13_hyp = 
    
    
//    System.out.println("important points:");
//    System.out.println("p1: " + p1);
//    System.out.println("p2: " + p2);
//    System.out.println("p3: " + p3);
//    System.out.println("p4: " + p4);
//    System.out.println("p5: " + p5);
//    System.out.println("p6: " + p6);
//    System.out.println("p7: " + p7);
//    System.out.println("p8: " + p8);
//    System.out.println("p9: " + p9);
  }
  
  protected void curveTranslation(Vector3 vecIn, Vector3 vecOut)
  {
    float angl_deg = (vecIn.z / omkr) * 360;
    float angl_rad = angl_deg * Mathf.Deg2Rad; 
    float korr_radius = radius - vecIn.x;
    float z = Mathf.Sin(angl_rad) * korr_radius;
    float x = radius - (Mathf.Cos(angl_rad) * korr_radius);
    
    vecOut.x = x;
    vecOut.z = z;
  }
  
  protected void curveTranslationAll(Vector3[] vecArray)
  {
    foreach (Vector3 vec in vecArray)
      curveTranslation(vec, vec);
  }
  
  protected void switchRotation(Vector3 vecIn, Vector3 vecOut)
  {
    vecOut = rotYTransform.transform.position + vecIn;
   // pivot.setLocalRotation(rotY);
    //pivot.setLocalTranslation(-p8.x, 0, -p8.z);
  }
  
  protected void switchRotationAll(Vector3[] vecArray)
  {
    foreach (Vector3 vec in vecArray)
      switchRotation(vec, vec);
  }
  
  protected void switchTranslation(Vector3 vecIn, Vector3 vecOut)
  {
    vecOut = startVertex+vecIn;
  }
  
  protected void switchTranslationAll(Vector3[] vecArray)
  {
    foreach (Vector3 vec in vecArray)
      switchTranslation(vec, vec);
  }
    
  protected void switchTranslateVector(Vector3 vector, Vector3 vecIn, Vector3 vecOut)
  {
    vecOut = vecIn+vector;
  }
  
  protected void switchTranslateVectorAll(Vector3 vector, Vector3[] vecArray)
  {
    foreach (Vector3 vec in vecArray)
      switchTranslateVector(vector, vec, vec);
  }
  
  protected void switchWorldTranslateVector(Vector3 vecIn, Vector3 vecOut, KmlSplineTrase trase)
  {
    trase.finnVertexITraseVertex(vecIn, vecOut);
  }
  
  protected void switchWorldTranslateVectorAll(Vector3[] vecArray, KmlSplineTrase trase)
  {
    foreach (Vector3 vec in vecArray)
      switchWorldTranslateVector(vec, vec, trase);
  }
  
  
  protected void movingPartNarrow(float narrowFactor, Vector3 vecIn, Vector3 vecOut)
  {
    vecOut.x = vecIn.x * narrowFactor;
  }
  
  protected void movingPartNarrowAll(Vector3[] vecArray)
  {
    float narrowPartPercentage = 0.3f;
    float zMax = p6.z;
    float zStartNarrowing = zMax * narrowPartPercentage;
    float narrowFactor = 1.0f;
    float minNarrowFactor = 0.2f;
    
    foreach (Vector3 vec in vecArray)
    {
      if (vec.z <= zStartNarrowing)
      {
        narrowFactor = vec.z / zStartNarrowing;
        if (narrowFactor < minNarrowFactor)
        {
          narrowFactor = minNarrowFactor;
        }
        
        movingPartNarrow(narrowFactor, vec, vec);
      }
    }
  }
  
  protected void mirrorX(Vector3 vecIn, Vector3 vecOut)
  {
    vecOut.x = -vecIn.x;
  }
  
  protected void mirrorXAll(Vector3[] vecArray)
  {
    foreach (Vector3 vec in vecArray)
      mirrorX(vec, vec);
  }
  
  protected void mirrorAllIndexArray(int[] indexArray)
  {
    int temp = 0;
    for (int i = 0; i < indexArray.Length; i += 3)
    {
      temp = indexArray[i + 1];
      indexArray[i + 1] = indexArray[i + 2];
      indexArray[i + 2] = temp;
    }
  }
  
  protected void switchConvertToWorldPoint(Vector3 vecIn, Vector3 vecOut, KmlSplineTrase kmlSplineTrase)
  {
    switchRotation(vecIn, vecOut);
    switchTranslation(vecOut, vecOut);
    kmlSplineTrase.finnVertexITraseVertex(vecOut, vecOut);
  }
  
  protected Transform divergingStraightPart(KmlSplineTrase kmlSplineTrase)
  {
    Track rettAv = new Track();
    
    if (directionForward)
    {
      rettAv.setStartVertex(p2_lt);
      rettAv.setEndVertex(p3_lt);
    }
    else
    {
      rettAv.setStartVertex(p3_lt);
      rettAv.setEndVertex(p2_lt); 
    }
    
    //rettAv.setStopWorldPos(true);
    rettAv.init(kmlSplineTrase);
    
    tnDivergingStraightStart = rettAv.getStartTrackNodes()[0];
    tnDivergingStraightEnd = rettAv.getEndTrackNodes()[0];
    
    //170120
    allTrackNodes.AddRange(rettAv.getAllTrackNodes());
    //
    
    return rettAv.getPivot();
  }
  
    protected Transform StraightStraightPart(KmlSplineTrase kmlSplineTrase)
  {
    Track rettRe = new Track();
    if (directionForward)
    {
      rettRe.setStartVertex(p4_lt);
      rettRe.setEndVertex(p5_lt);
    }
    else
    {
      rettRe.setStartVertex(p5_lt);
      rettRe.setEndVertex(p4_lt); 
    }

    //rettRe.setStopWorldPos(true);
    rettRe.init(kmlSplineTrase);
    
    tnStraightStraightStart = rettRe.getStartTrackNodes()[0];
    tnStraightStraightEnd = rettRe.getEndTrackNodes()[0];
    
    //170120
    allTrackNodes.AddRange(rettRe.getAllTrackNodes());   
    //    
    
    return rettRe.getPivot();
  }
  
  protected GameObject outerLeftCurve(KmlSplineTrase kmlSplineTrase)
  {
    GameObject pivot = new GameObject("Pivot");

    TrackPart track_top_v = new TrackPart(p0, p4, 3, TrackPart.to_top, TrackPart.txo_top, 
        TrackPart.indexSequenze_top, TrackPart.texFac_top, MaterialManager.Instance.GetMaterial("TrackTop"));
    track_top_v.localPositioning((sporbredde/2),0,0);  
    curveTranslationAll(track_top_v.getVertexArray());  
    if (!divergingLeft)
    {
      mirrorXAll(track_top_v.getVertexArray());
      mirrorAllIndexArray(track_top_v.getIndexArray());
    }
    switchRotationAll(track_top_v.getVertexArray());
    switchTranslationAll(track_top_v.getVertexArray());
    track_top_v.worldPositioning(kmlSplineTrase);
    GameObject spatial0 = track_top_v.meshMaker();
    spatial0.transform.parent = pivot.transform;

    TrackPart track_side_v = new TrackPart(p0, p4, 4, TrackPart.to_sides, TrackPart.txo_sides, 
        TrackPart.indexSequenze_sides, TrackPart.texFac_sides, MaterialManager.Instance.GetMaterial("Track"));
    track_side_v.localPositioning((sporbredde/2),0,0);
    curveTranslationAll(track_side_v.getVertexArray());
    if (!divergingLeft)
    {
      mirrorXAll(track_side_v.getVertexArray());
      mirrorAllIndexArray(track_side_v.getIndexArray());
    }
    switchRotationAll(track_side_v.getVertexArray());
    switchTranslationAll(track_side_v.getVertexArray());
    track_side_v.worldPositioning(kmlSplineTrase);
    GameObject spatial1 = track_side_v.meshMaker();
    spatial1.transform.parent = pivot.transform;
      
    TrackPart track_foot_side_v = new TrackPart(p0, p4, 5, TrackPart.to_foot_sides, TrackPart.txo_foot_sides, 
        TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, MaterialManager.Instance.GetMaterial("Track"));
    track_foot_side_v.localPositioning((sporbredde/2),0,0);
    curveTranslationAll(track_foot_side_v.getVertexArray());
    if (!divergingLeft)
    {
      mirrorXAll(track_foot_side_v.getVertexArray());
      mirrorAllIndexArray(track_foot_side_v.getIndexArray());
    }
    switchRotationAll(track_foot_side_v.getVertexArray());
    switchTranslationAll(track_foot_side_v.getVertexArray());
    track_foot_side_v.worldPositioning(kmlSplineTrase);
    GameObject spatial_foot_side = track_foot_side_v.meshMaker();
    spatial_foot_side.transform.parent = pivot.transform;
    
    return pivot;
  }

  protected GameObject outerRightStraight(KmlSplineTrase kmlSplineTrase)
  {
        GameObject pivot2 = new GameObject();
        TrackPart track_top_r = new TrackPart(p0, p4, 3, TrackPart.to_top, TrackPart.txo_top, 
            TrackPart.indexSequenze_top, TrackPart.texFac_top, MaterialManager.Instance.GetMaterial("TrackTop"));
        track_top_r.localPositioning(-0.7175f,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_top_r.getVertexArray());
            mirrorAllIndexArray(track_top_r.getIndexArray());
        }
        switchRotationAll(track_top_r.getVertexArray());
        switchTranslationAll(track_top_r.getVertexArray());
        track_top_r.worldPositioning(kmlSplineTrase);
        GameObject spatial2 = track_top_r.meshMaker();
        spatial2.transform.parent = pivot2.transform;

        TrackPart track_side_r = new TrackPart(p0, p4, 4, TrackPart.to_sides, TrackPart.txo_sides, 
            TrackPart.indexSequenze_sides, TrackPart.texFac_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_side_r.localPositioning(-0.7175f,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_side_r.getVertexArray());
            mirrorAllIndexArray(track_side_r.getIndexArray());
        }
        switchRotationAll(track_side_r.getVertexArray());
        switchTranslationAll(track_side_r.getVertexArray());
        track_side_r.worldPositioning(kmlSplineTrase);
        GameObject spatial3 = track_side_r.meshMaker();
            spatial3.transform.parent = pivot2.transform;
        
        TrackPart track_foot_side_r = new TrackPart(p0, p4, 5, TrackPart.to_foot_sides, 
            TrackPart.txo_foot_sides, TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_foot_side_r.localPositioning(-0.7175f,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_foot_side_r.getVertexArray());
            mirrorAllIndexArray(track_foot_side_r.getIndexArray());
        }
        switchRotationAll(track_foot_side_r.getVertexArray());
        switchTranslationAll(track_foot_side_r.getVertexArray());
        track_foot_side_r.worldPositioning(kmlSplineTrase);
        GameObject spatial_foot_side_right = track_foot_side_r.meshMaker();
        spatial_foot_side_right.transform.parent = pivot2.transform;
      
    return pivot2;
  }
  
  protected GameObject divergingCrossLeadTrack(KmlSplineTrase kmlSplineTrase)
  {
        GameObject pivot2 = new GameObject();
        TrackPart track_top_r = new TrackPart(p7, p10, 3, TrackPart.to_top, TrackPart.txo_top, 
            TrackPart.indexSequenze_top, TrackPart.texFac_top, MaterialManager.Instance.GetMaterial("TrackTop"));
        track_top_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_top_r.getVertexArray());
            mirrorAllIndexArray(track_top_r.getIndexArray());
        }
        switchRotationAll(track_top_r.getVertexArray());
        switchTranslationAll(track_top_r.getVertexArray());
        track_top_r.worldPositioning(kmlSplineTrase);
        GameObject spatial2 = track_top_r.meshMaker();
        spatial2.transform.parent = pivot2.transform;


      
        TrackPart track_side_r = new TrackPart(p7, p10, 4, TrackPart.to_sides, TrackPart.txo_sides, 
            TrackPart.indexSequenze_sides, TrackPart.texFac_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_side_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_side_r.getVertexArray());
            mirrorAllIndexArray(track_side_r.getIndexArray());
        }
        switchRotationAll(track_side_r.getVertexArray());
        switchTranslationAll(track_side_r.getVertexArray());
        track_side_r.worldPositioning(kmlSplineTrase);
        GameObject spatial3 = track_side_r.meshMaker();
        spatial3.transform.parent = pivot2.transform;
      
        TrackPart track_foot_side_r = new TrackPart(p7, p10, 5, TrackPart.to_foot_sides, 
            TrackPart.txo_foot_sides, TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_foot_side_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_foot_side_r.getVertexArray());
            mirrorAllIndexArray(track_foot_side_r.getIndexArray());
        }
        
    
        
        switchRotationAll(track_foot_side_r.getVertexArray());
        switchTranslationAll(track_foot_side_r.getVertexArray());
        track_foot_side_r.worldPositioning(kmlSplineTrase);
                
        
        GameObject spatial_foot_side_right = track_foot_side_r.meshMaker();
        spatial_foot_side_right.transform.parent = pivot2.transform;
      
       
      
    return pivot2;
  }
  
  protected GameObject straightCrossLeadTrack(KmlSplineTrase kmlSplineTrase)
  {
      GameObject pivot2 = new GameObject();
      
     
      TrackPart track_top_r = new TrackPart(p8, p11, 3, TrackPart.to_top, TrackPart.txo_top, 
        TrackPart.indexSequenze_top, TrackPart.texFac_top, MaterialManager.Instance.GetMaterial("TrackTop"));
      
          
      
      track_top_r.localPositioning(0,0,0);
      
       
      
      if (!divergingLeft)
      {
        mirrorXAll(track_top_r.getVertexArray());
        mirrorAllIndexArray(track_top_r.getIndexArray());
      }
      switchRotationAll(track_top_r.getVertexArray());
      switchTranslationAll(track_top_r.getVertexArray());
      track_top_r.worldPositioning(kmlSplineTrase);
      GameObject spatial2 = track_top_r.meshMaker();
      spatial2.transform.parent = pivot2.transform;

      
      
        TrackPart track_side_r = new TrackPart(p8, p11, 4, TrackPart.to_sides, TrackPart.txo_sides, 
            TrackPart.indexSequenze_sides, TrackPart.texFac_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_side_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_side_r.getVertexArray());
            mirrorAllIndexArray(track_side_r.getIndexArray());
        }
        switchRotationAll(track_side_r.getVertexArray());
        switchTranslationAll(track_side_r.getVertexArray());
        track_side_r.worldPositioning(kmlSplineTrase);
        GameObject spatial3 = track_side_r.meshMaker();
        spatial3.transform.parent = pivot2.transform;
        
        TrackPart track_foot_side_r = new TrackPart(p8, p11, 5, TrackPart.to_foot_sides, 
            TrackPart.txo_foot_sides, TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_foot_side_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_foot_side_r.getVertexArray());
            mirrorAllIndexArray(track_foot_side_r.getIndexArray());
        }
        
    
        
        switchRotationAll(track_foot_side_r.getVertexArray());
        switchTranslationAll(track_foot_side_r.getVertexArray());
        track_foot_side_r.worldPositioning(kmlSplineTrase);
        GameObject spatial_foot_side_right = track_foot_side_r.meshMaker();
        spatial_foot_side_right.transform.parent = pivot2.transform;
      
      return pivot2;
  }
  
    protected GameObject straightOuterLeadTrack(KmlSplineTrase kmlSplineTrase)
  {
        GameObject pivot2 = new GameObject();
        TrackPart track_top_r = new TrackPart(p14, p15, 3, TrackPart.to_top, TrackPart.txo_top, 
            TrackPart.indexSequenze_top, TrackPart.texFac_top, MaterialManager.Instance.GetMaterial("TrackTop"));
        track_top_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_top_r.getVertexArray());
            mirrorAllIndexArray(track_top_r.getIndexArray());
        }
        switchRotationAll(track_top_r.getVertexArray());
        switchTranslationAll(track_top_r.getVertexArray());
        track_top_r.worldPositioning(kmlSplineTrase);
        GameObject spatial2 = track_top_r.meshMaker();
        spatial2.transform.parent = pivot2.transform;

        TrackPart track_side_r = new TrackPart(p14, p15, 4, TrackPart.to_sides, TrackPart.txo_sides, 
            TrackPart.indexSequenze_sides, TrackPart.texFac_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_side_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_side_r.getVertexArray());
            mirrorAllIndexArray(track_side_r.getIndexArray());
        }
        switchRotationAll(track_side_r.getVertexArray());
        switchTranslationAll(track_side_r.getVertexArray());
        track_side_r.worldPositioning(kmlSplineTrase);
        GameObject spatial3 = track_side_r.meshMaker();
        spatial3.transform.parent = pivot2.transform;
        
        TrackPart track_foot_side_r = new TrackPart(p14, p15, 5, TrackPart.to_foot_sides, 
            TrackPart.txo_foot_sides, TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_foot_side_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_foot_side_r.getVertexArray());
            mirrorAllIndexArray(track_foot_side_r.getIndexArray());
        }
        switchRotationAll(track_foot_side_r.getVertexArray());
        switchTranslationAll(track_foot_side_r.getVertexArray());
        track_foot_side_r.worldPositioning(kmlSplineTrase);
        GameObject spatial_foot_side_right = track_foot_side_r.meshMaker();
        spatial_foot_side_right.transform.parent = pivot2.transform;
      
      return pivot2;
  }

  protected GameObject curveOuterLeadTrack(KmlSplineTrase kmlSplineTrase)
  {
        GameObject pivot2 = new GameObject();
        TrackPart track_top_r = new TrackPart(p12, p13, 3, TrackPart.to_top, TrackPart.txo_top, 
            TrackPart.indexSequenze_top, TrackPart.texFac_top, MaterialManager.Instance.GetMaterial("TrackTop"));
        track_top_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_top_r.getVertexArray());
            mirrorAllIndexArray(track_top_r.getIndexArray());
        }
        switchRotationAll(track_top_r.getVertexArray());
        switchTranslationAll(track_top_r.getVertexArray());
        track_top_r.worldPositioning(kmlSplineTrase);
        GameObject spatial2 = track_top_r.meshMaker();
        spatial2.transform.parent = pivot2.transform;

        TrackPart track_side_r = new TrackPart(p12, p13, 4, TrackPart.to_sides, TrackPart.txo_sides, 
            TrackPart.indexSequenze_sides, TrackPart.texFac_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_side_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_side_r.getVertexArray());
            mirrorAllIndexArray(track_side_r.getIndexArray());
        }
        switchRotationAll(track_side_r.getVertexArray());
        switchTranslationAll(track_side_r.getVertexArray());
        track_side_r.worldPositioning(kmlSplineTrase);
        GameObject spatial3 = track_side_r.meshMaker();
        spatial3.transform.parent = pivot2.transform;
        
        TrackPart track_foot_side_r = new TrackPart(p12, p13, 5, TrackPart.to_foot_sides, 
            TrackPart.txo_foot_sides, TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, MaterialManager.Instance.GetMaterial("Track"));
        track_foot_side_r.localPositioning(0,0,0);
        if (!divergingLeft)
        {
            mirrorXAll(track_foot_side_r.getVertexArray());
            mirrorAllIndexArray(track_foot_side_r.getIndexArray());
        }
        switchRotationAll(track_foot_side_r.getVertexArray());
        switchTranslationAll(track_foot_side_r.getVertexArray());
        track_foot_side_r.worldPositioning(kmlSplineTrase);
        GameObject spatial_foot_side_right = track_foot_side_r.meshMaker();
        spatial_foot_side_right.transform.parent = pivot2.transform;
      
      return pivot2;
  }
  
  protected GameObject gravel(KmlSplineTrase kmlSplineTrase)
  {
      GameObject pivot_gravel = new GameObject();
      TrackPart track_gravel_1 = new TrackPart(p0, p4, 6, TrackPart.to_gravel, TrackPart.txo_gravel, 
        TrackPart.indexSequenze_gravel, TrackPart.texFac_gravel, MaterialManager.Instance.GetMaterial("Ground"));
      track_gravel_1.localPositioning(0,0,0);
      
      foreach (Vector3 vec in track_gravel_1.getVertexArray())
      {
        if (vec.x > 0)
        {
          curveTranslation(vec, vec);
        }
      }
      if (!divergingLeft)
      {
        mirrorXAll(track_gravel_1.getVertexArray());
        mirrorAllIndexArray(track_gravel_1.getIndexArray());
      }
      switchRotationAll(track_gravel_1.getVertexArray());
      switchTranslationAll(track_gravel_1.getVertexArray());
      track_gravel_1.worldPositioning(kmlSplineTrase);
      GameObject spatial_gravel_1  = track_gravel_1.meshMaker();
      spatial_gravel_1.transform.parent = pivot_gravel.transform;
      
      return pivot_gravel;
  }

  protected GameObject movingPartCurve(KmlSplineTrase kmlSplineTrase)
  {
    GameObject pivot = new GameObject();

    // for moving parts only
    switchConvertToWorldPoint(p8, p8w, kmlSplineTrase);
    Vector3 vecToZero = -p8w;
    //
    
    TrackPart track_top_v = new TrackPart(p0, p6, 3, TrackPart.to_top, TrackPart.txo_top, 
        TrackPart.indexSequenze_top, TrackPart.texFac_top, MaterialManager.Instance.GetMaterial("TrackTop"));
    movingPartNarrowAll(track_top_v.getVertexArray()); // only moving parts
    
    track_top_v.localPositioning(-(sporbredde/2),0,0);  
    curveTranslationAll(track_top_v.getVertexArray());
      if (!divergingLeft)
      {
        mirrorXAll(track_top_v.getVertexArray());
        mirrorAllIndexArray(track_top_v.getIndexArray());
      }
    switchRotationAll(track_top_v.getVertexArray());
    switchTranslationAll(track_top_v.getVertexArray());
    track_top_v.worldPositioning(kmlSplineTrase);
    switchTranslateVectorAll(vecToZero, track_top_v.getVertexArray()); // only moving parts
    
    GameObject spatial0 = track_top_v.meshMaker();
    spatial0.transform.parent = pivot.transform;

    TrackPart track_side_v = new TrackPart(p0, p6, 4, TrackPart.to_sides, TrackPart.txo_sides, 
        TrackPart.indexSequenze_sides, TrackPart.texFac_sides, MaterialManager.Instance.GetMaterial("Track"));
    
    track_side_v.localPositioning(-(sporbredde/2),0,0);
    curveTranslationAll(track_side_v.getVertexArray());
      if (!divergingLeft)
      {
        mirrorXAll(track_side_v.getVertexArray());
        mirrorAllIndexArray(track_side_v.getIndexArray());
      }
    switchRotationAll(track_side_v.getVertexArray());
    switchTranslationAll(track_side_v.getVertexArray());
    track_side_v.worldPositioning(kmlSplineTrase);
    switchTranslateVectorAll(vecToZero, track_side_v.getVertexArray());
    
    GameObject spatial1 = track_side_v.meshMaker();
    spatial1.transform.parent = pivot.transform;
      
    TrackPart track_foot_side_v = new TrackPart(p0, p6, 5, TrackPart.to_foot_sides, TrackPart.txo_foot_sides, 
        TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, MaterialManager.Instance.GetMaterial("Track"));
    
    track_foot_side_v.localPositioning(-(sporbredde/2),0,0);
    curveTranslationAll(track_foot_side_v.getVertexArray());
      if (!divergingLeft)
      {
        mirrorXAll(track_foot_side_v.getVertexArray());
        mirrorAllIndexArray(track_foot_side_v.getIndexArray());
      }
    switchRotationAll(track_foot_side_v.getVertexArray());
    switchTranslationAll(track_foot_side_v.getVertexArray());
    track_foot_side_v.worldPositioning(kmlSplineTrase);
    switchTranslateVectorAll(vecToZero, track_foot_side_v.getVertexArray());
    
    GameObject spatial_foot_side = track_foot_side_v.meshMaker();
    spatial_foot_side.transform.parent = pivot.transform;

    // for moving parts only
    pivot.transform.Translate(p8w);
    //pivot.rotate(0, FastMath.DEG_TO_RAD * -0.5f, 0);
    
    switchPositioner.setMovableCurvePivot(pivot);
    
    return pivot;
  }

  protected GameObject movingPartStraight(KmlSplineTrase kmlSplineTrase)
  {
      GameObject pivot2 = new GameObject();
      
      // for moving parts only
      switchConvertToWorldPoint(p7, p7w, kmlSplineTrase);
      Vector3 vecToZero = -p7w;
      //
      
      TrackPart track_top_r = new TrackPart(p0, p6, 3, TrackPart.to_top, TrackPart.txo_top, 
        TrackPart.indexSequenze_top, TrackPart.texFac_top, MaterialManager.Instance.GetMaterial("TrackTop"));
      movingPartNarrowAll(track_top_r.getVertexArray()); // only moving parts
      track_top_r.localPositioning((sporbredde/2),0,0);
      if (!divergingLeft)
      {
        mirrorXAll(track_top_r.getVertexArray());
        mirrorAllIndexArray(track_top_r.getIndexArray());
      }
      switchRotationAll(track_top_r.getVertexArray());
      switchTranslationAll(track_top_r.getVertexArray());
      track_top_r.worldPositioning(kmlSplineTrase);
      switchTranslateVectorAll(vecToZero, track_top_r.getVertexArray()); // only moving parts
      GameObject spatial2 = track_top_r.meshMaker();
      spatial2.transform.parent = pivot2.transform;

      TrackPart track_side_r = new TrackPart(p0, p6, 4, TrackPart.to_sides, TrackPart.txo_sides, 
        TrackPart.indexSequenze_sides, TrackPart.texFac_sides, MaterialManager.Instance.GetMaterial("Track"));
      track_side_r.localPositioning((sporbredde/2),0,0);
      if (!divergingLeft)
      {
        mirrorXAll(track_side_r.getVertexArray());
        mirrorAllIndexArray(track_side_r.getIndexArray());
      }
      switchRotationAll(track_side_r.getVertexArray());
      switchTranslationAll(track_side_r.getVertexArray());
      track_side_r.worldPositioning(kmlSplineTrase);
      switchTranslateVectorAll(vecToZero, track_side_r.getVertexArray()); // only moving parts
      GameObject spatial3 = track_side_r.meshMaker();
      spatial3.transform.parent = pivot2.transform;
      
      TrackPart track_foot_side_r = new TrackPart(p0, p6, 5, TrackPart.to_foot_sides, 
        TrackPart.txo_foot_sides, TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, MaterialManager.Instance.GetMaterial("Track"));
      track_foot_side_r.localPositioning((sporbredde/2),0,0);
      if (!divergingLeft)
      {
        mirrorXAll(track_foot_side_r.getVertexArray());
        mirrorAllIndexArray(track_foot_side_r.getIndexArray());
      }
      switchRotationAll(track_foot_side_r.getVertexArray());
      switchTranslationAll(track_foot_side_r.getVertexArray());
      track_foot_side_r.worldPositioning(kmlSplineTrase);
      switchTranslateVectorAll(vecToZero, track_foot_side_r.getVertexArray()); // only moving parts
      GameObject spatial_foot_side_right = track_foot_side_r.meshMaker();
      spatial_foot_side_right.transform.parent = pivot2.transform; 
      
      // for moving parts only
      pivot2.transform.Translate(p7w);
      //pivot2.rotate(0, FastMath.DEG_TO_RAD * 0.5f, 0);
      
      switchPositioner.setMovableStraightPivot(pivot2);
      
      return pivot2;
  }
  
    protected void trackMesh(KmlSplineTrase kmlSplineTrase)
    {
        findImportantPoints();

        endVertex.z = p4.z;
        
        pivotAll = new GameObject("SwitchTrack").transform;
        pivotMovables = new GameObject("SwitchTrackMovables").transform;
        //pivot.attachChild(pivot1);
      

        outerRightStraight(kmlSplineTrase).transform.parent = pivotAll.transform;
        gravel(kmlSplineTrase).transform.parent = pivotAll.transform;
        outerLeftCurve(kmlSplineTrase).transform.parent = pivotAll.transform;
        movingPartCurve(kmlSplineTrase).transform.parent = pivotMovables.transform;
        movingPartStraight(kmlSplineTrase).transform.parent = pivotMovables.transform;
        divergingCrossLeadTrack(kmlSplineTrase).transform.parent = pivotAll;

        straightCrossLeadTrack(kmlSplineTrase).transform.parent = pivotAll.transform;

        straightOuterLeadTrack(kmlSplineTrase).transform.parent = pivotAll.transform;
        curveOuterLeadTrack(kmlSplineTrase).transform.parent = pivotAll.transform;

    
            
        // local translations of endpoints for straight parts
        p2_lt += p2 + Vector3.zero;
        p3_lt += p3 + Vector3.zero;
        
        if (!divergingLeft)
        {
            mirrorX(p2_lt, p2_lt);
            mirrorX(p3_lt, p3_lt);
        }
        
        switchRotation(p2_lt, p2_lt);
        switchTranslation(p2_lt, p2_lt);
        
        switchRotation(p3_lt, p3_lt);
        switchTranslation(p3_lt, p3_lt);
        
        switchRotation(p4, p4_lt);
        switchTranslation(p4_lt, p4_lt);
        
        switchRotation(p5, p5_lt);
        switchTranslation(p5_lt, p5_lt);
      //
      
//      System.out.println("p5_lt: " + p5_lt);
//      System.out.println("p3_lt: " + p3_lt);
      
        divergingStraightPart(kmlSplineTrase).transform.parent = pivotAll.transform;
        StraightStraightPart(kmlSplineTrase).transform.parent = pivotAll.transform;
      
      //
          
      
    } 

  protected void establishTrackNodesStraight(KmlSplineTrase kmlSplineTrase)
  {
    float zDist = 5.0f;
    TrackNode forrige = null;
    TrackNode t;
    for (float f = 0; f < length; f += zDist)
    {
      t = new TrackNode(null, new Vector3(0,0,f));
      if (forrige != null)
      {
        forrige.neste = t;
        t.forrige = forrige;
      }
      else
      {
        first = t;
      }
      
      forrige = t;
    }
    
    // siste
    t = new TrackNode(null, new Vector3(0,0,length));
    forrige.neste = t;
    t.forrige = forrige;
    tnSwitchStraightEnd = t;
    tnSwitchStraightStart = first;
    
    // transformations
    List<Vector3> trackNodesTempVecList = new List<Vector3>();
    List<Vector3> trackNodesWorldVecList = new List<Vector3>();

    
    TrackNode runner = first;
    while(runner != null)
    {
      trackNodesTempVecList.Add(runner.getTempPoint());
      trackNodesWorldVecList.Add(runner.getPoint());
      runner = runner.neste;
    }
    
    Vector3[] tva = new Vector3[trackNodesTempVecList.Count];
    tva = trackNodesTempVecList.ToArray();
    Vector3[] wva = new Vector3[trackNodesWorldVecList.Count];
    wva = trackNodesWorldVecList.ToArray();
    
    switchRotationAll(tva);
    switchTranslationAll(tva);
    for (int i = 0; i < tva.Length; i++)
      switchWorldTranslateVector(tva[i], wva[i], kmlSplineTrase);
    
    // fyll tracknode-lister
    runner = first;
    while(runner != null)
    {
      allTrackNodes.Add(runner);
      straightTrackNodes.Add(runner);
      
      runner = runner.neste;
    }
    
    //startTrackNodes.add(allTrackNodes.get(0));
    //endTrackNodes.add(allTrackNodes.get(allTrackNodes.size() - 1)); 
  }
  
  protected void establishTrackNodesDiverging(KmlSplineTrase kmlSplineTrase)
  {
    float zDist = 5.0f;
    TrackNode forrige = null;
    TrackNode t;
    for (float f = 0; f < length; f += zDist)
    {
      t = new TrackNode(null, new Vector3(0,0,f));
      if (forrige != null)
      {
        forrige.neste = t;
        t.forrige = forrige;
      }
      else
      {
        first = t;
      }
      
      forrige = t;
    }
    
    // siste
    t = new TrackNode(null, new Vector3(0,0,length));
    forrige.neste = t;
    t.forrige = forrige;
    tnSwitchDivergingEnd = t;
    tnSwitchDivergingStart = first;
    
    // transformations
    List<Vector3> trackNodesTempVecList = new List<Vector3>();
    List<Vector3> trackNodesWorldVecList = new List<Vector3>();
    
    TrackNode runner = first;
    while(runner != null)
    {
      trackNodesTempVecList.Add(runner.getTempPoint());
      trackNodesWorldVecList.Add(runner.getPoint());
      runner = runner.neste;
    }
    
    Vector3[] tva = new Vector3[trackNodesTempVecList.Count];
    tva = trackNodesTempVecList.ToArray();
    Vector3[] wva = new Vector3[trackNodesWorldVecList.Count];
    wva = trackNodesWorldVecList.ToArray();
    
    curveTranslationAll(tva);
    if (!divergingLeft)
      mirrorXAll(tva);
    switchRotationAll(tva);
    switchTranslationAll(tva);
    for (int i = 0; i < tva.Length; i++)
      switchWorldTranslateVector(tva[i], wva[i], kmlSplineTrase);
    
    // fyll tracknode-lister
    runner = first;
    while(runner != null)
    {
      allTrackNodes.Add(runner);
      
      runner = runner.neste;
    }
    
    //startTrackNodes.add(allTrackNodes.get(0));
    //endTrackNodes.add(allTrackNodes.get(allTrackNodes.size() - 1)); 
  }

  protected void reverseTrackNodes(List<TrackNode> tnl)
  {
    TrackNode tnTemp = null;
    
    foreach (TrackNode tn in tnl)
    {
      tnTemp = tn.neste;
      tn.neste = tn.forrige;
      tn.forrige = tnTemp;
    }
  }

  protected void connectTrackNodesSwitchPartsStraightParts()
  {
    TrackNodeSwitch tns = new TrackNodeSwitch(null, new Vector3(0,0,0));
    tns.getTempPoint.Set(first.getTempPoint().x + Vector3.zero.x, first.getTempPoint().y + Vector3.zero.y, first.getTempPoint().z + Vector3.zero.z);
    tns.GetPoint.Set(first.getPoint().x + Vector3.zero.x, first.getPoint().y + Vector3.zero.y, first.getPoint().z + Vector3.zero.z);
    allTrackNodes.Add(tns);
    switchPositioner.setTrackNodeSwitch(tns);
    tnStart = tns;
    
    if (directionForward)
    {
      tnSwitchStraightEnd.neste = tnStraightStraightStart;
      tnStraightStraightStart.forrige = tnSwitchStraightEnd;
      
      tnSwitchDivergingEnd.neste = tnDivergingStraightStart;
      tnDivergingStraightStart.forrige = tnSwitchDivergingEnd;
      
      startTrackNodes.Add(tns);
      tns.nesteL = tnSwitchDivergingStart;
      tnSwitchDivergingStart.forrige = tns;
      tns.nesteR = tnSwitchStraightStart;
      tnSwitchStraightStart.forrige = tns;
      tns.setStateR();

      endTrackNodes.Add(tnDivergingStraightEnd);
      endTrackNodes.Add(tnStraightStraightEnd);
    }
    else
    {
      // OBS reverse internal tracknodes
        // straight
      List<TrackNode> tnl = new List<TrackNode>();
      TrackNode runner = tnSwitchStraightStart;
      while (runner != null)
      {
        tnl.Add(runner);
        runner = runner.neste;
      }
      reverseTrackNodes(tnl);
      tnSwitchStraightStart = tnl[tnl.Count-1];
      tnSwitchStraightEnd = tnl[0];
      tnl.Clear();
        // diverging
      runner = tnSwitchDivergingStart;
      while (runner != null)
      {
        tnl.Add(runner);
        runner = runner.neste;
      }
      reverseTrackNodes(tnl);
      tnSwitchDivergingStart = tnl[tnl.Count-1];
      tnSwitchDivergingEnd = tnl[0];
      
      //System.exit(0);
      
      // 
      tnSwitchStraightStart.forrige = tnStraightStraightEnd;
      tnStraightStraightEnd.neste = tnSwitchStraightStart;
      
      tnSwitchDivergingStart.forrige = tnDivergingStraightEnd;
      tnDivergingStraightEnd.neste = tnSwitchDivergingStart;
      
      endTrackNodes.Add(tns);
      tns.forrigeL = tnSwitchDivergingEnd;
      tnSwitchDivergingEnd.neste = tns;
      tns.forrigeR = tnSwitchStraightEnd;
      tnSwitchStraightEnd.neste = tns;
      tns.setStateR();

      startTrackNodes.Add(tnDivergingStraightStart);
      startTrackNodes.Add(tnStraightStraightStart);
      
//      // test
//      runner = tnSwitchStraightStart;
//      while (runner != null)
//      {
//        System.out.println("testtn: " + runner.getTempPoint() + " " + runner.getClass());
//        runner = runner.neste;
//      }
//      
//      System.exit(0);
    }
  }
  

*/
}
