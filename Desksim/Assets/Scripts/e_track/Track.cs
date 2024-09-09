using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour, TrackElement
{
  [SerializeField] private Material matBase;

  private Transform pivot;
  private TrackNode first;
  private float length = 10000;
[SerializeField]  private Vector3 startVertex = new Vector3(0,0,0);
[SerializeField]  private Vector3 endVertex = new Vector3(0,0,20);
  [SerializeField] private List<TrackNode> allTrackNodes = new List<TrackNode>();
  [SerializeField] private List<TrackNode> startTrackNodes = new List<TrackNode>();
  [SerializeField] private List<TrackNode> endTrackNodes = new List<TrackNode>();
  
  private bool stopWorldPos = false;

  public Track()
  {

  }

  public void setStartVertex(Vector3 startVertex) 
  {
    print("This happens!");
    print(this.startVertex + " - Start Vertex");
    this.startVertex = startVertex;
    print(this.startVertex + " - Start Vertex after");
    length = Vector3.Distance(startVertex, endVertex);
  }

  public void setEndVertex(Vector3 endVertex) 
  {
    this.endVertex = endVertex;
    length = Vector3.Distance(startVertex, endVertex);
  }
  
  // temp for testing
  public void setStopWorldPos(bool stopWorldPos)
  {
    this.stopWorldPos = stopWorldPos;
  }
  
  public void init(KmlSplineTrase kmlSplineTrase)
  {
    trackMesh(kmlSplineTrase);
    establishTrackNodes(kmlSplineTrase);
  }

  public Transform getPivot()
    {
      return pivot;
    }

  public TrackNode getFirst()
  {
    return first;
  }
    
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

  public Vector3 getEndVertex()
  {
    return endVertex;
  }
  
  private void buildTrackNode(TrackNode forrige)
  {
    
  }
  
  private void TrackNodeVecRotationAll(Vector3[] vertexArray)
  {
    // rotasjon
    Vector3.Angle (startVertex, endVertex);
    float vi1 = AnglesVectors.finnVinkelOmY(startVertex, endVertex);

    //float vi1 = FastMath.DEG_TO_RAD * 6;
    Quaternion rotY = AnglesVectors.fromAngleAxis(new Quaternion(), vi1, Vector3.up);
    Transform t1 = new GameObject().transform;
    t1.rotation = rotY;
    
    foreach (Vector3 v in vertexArray)
      t1.position = v;
  }
  
  private void establishTrackNodes(KmlSplineTrase kmlSplineTrase)
  {
    float zDist = 5.0f;
    TrackNode forrige = null;
    for (float f = 0; f < length; f += zDist)
    {
      TrackNode tb = new TrackNode(null, new Vector3(0,0,f));
      if (forrige != null)
      {
        forrige.neste = tb;
        tb.forrige = forrige;
      }
      else
      {
        first = tb;
      }
      
      forrige = tb;
    }
    
    // rotation
    
    // siste
    TrackNode t = new TrackNode(null, new Vector3(0,0,length));
    forrige.neste = t;
    t.forrige = forrige;
    
    // rotation
    TrackNode runner = first;
    float vi1 = AnglesVectors.finnVinkelOmY(startVertex, endVertex);

    Quaternion rotY = AnglesVectors.fromAngleAxis(new Quaternion(), vi1, new Vector3(0,1,0));
    Transform t1 = transform;
    t1.rotation = rotY;
    
    while(runner != null)
    {
      t1.position = runner.getTempPoint();
      runner = runner.neste;
    }
    
    // local transform
    runner = first;
    while(runner != null)
    {
      runner.getTempPoint().Set(runner.getTempPoint().x + startVertex.x, runner.getTempPoint().y + startVertex.y, runner.getTempPoint().z + startVertex.z);
      runner = runner.neste;
    }
    
    // world transform
    runner = first;
    while(runner != null)
    {
      kmlSplineTrase.finnVertexITraseVertex(runner.getTempPoint(), runner.getPoint());
      runner = runner.neste;
    }
    
    // fyll tracknode-lister
    runner = first;
    while(runner != null)
    {
      allTrackNodes.Add(runner);
      
      runner = runner.neste;
    }
    
    startTrackNodes.Add(allTrackNodes[0]);
    endTrackNodes.Add(allTrackNodes[allTrackNodes.Count - 1]);
  }

  private void trackMesh(KmlSplineTrase kmlSplineTrase)
  {
      GameObject pivot1 = new GameObject();
      pivot1.name = "Left Track";
      
      
      TrackPart track_top_v = new TrackPart(startVertex, endVertex, 3, TrackPart.to_top, TrackPart.txo_top, 
          TrackPart.indexSequenze_top, TrackPart.texFac_top, matBase);
      if (!stopWorldPos)
          track_top_v.worldPositioning(kmlSplineTrase);
      GameObject spatial0 = track_top_v.meshMaker();
      spatial0.name = "Left Top Piece";
      spatial0.transform.parent = pivot1.transform;

      TrackPart track_side_v = new TrackPart(startVertex, endVertex, 4, TrackPart.to_sides, TrackPart.txo_sides, 
          TrackPart.indexSequenze_sides, TrackPart.texFac_sides, matBase);
      if (!stopWorldPos)
          track_side_v.worldPositioning(kmlSplineTrase);
      GameObject spatial1 = track_side_v.meshMaker();
      spatial1.name = "Left Side Piece";
      spatial1.transform.parent = pivot1.transform;
      
      TrackPart track_foot_side_v = new TrackPart(startVertex, endVertex, 5, TrackPart.to_foot_sides, TrackPart.txo_foot_sides, 
          TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, matBase);
      if (!stopWorldPos)
          track_foot_side_v.worldPositioning(kmlSplineTrase);
      GameObject spatial_foot_side = track_foot_side_v.meshMaker();
      spatial_foot_side.name = "Left Side Foot Piece";
      spatial_foot_side.transform.parent = pivot1.transform;
      
      pivot1.transform.position += new Vector3(0.7175f,0,0);
      
      GameObject pivot2 = new GameObject();
      pivot2.name = "Right Track";
      TrackPart track_top_r = new TrackPart(startVertex, endVertex, 3, TrackPart.to_top, TrackPart.txo_top, 
          TrackPart.indexSequenze_top, TrackPart.texFac_top, matBase);
      if (!stopWorldPos)
          track_top_r.worldPositioning(kmlSplineTrase);
      GameObject spatial2 = track_top_r.meshMaker();
      spatial2.name = "Right Top Piece";
      spatial2.transform.parent = pivot2.transform;

      TrackPart track_side_r = new TrackPart(startVertex, endVertex, 4, TrackPart.to_sides, TrackPart.txo_sides, 
          TrackPart.indexSequenze_sides, TrackPart.texFac_sides, matBase);
      if (!stopWorldPos)
          track_side_r.worldPositioning(kmlSplineTrase);
      GameObject spatial3 = track_side_r.meshMaker();
      spatial0.name = "Right Side Piece";
      spatial3.transform.parent = pivot2.transform;
      
      TrackPart track_foot_side_r = new TrackPart(startVertex, endVertex, 5, TrackPart.to_foot_sides, 
          TrackPart.txo_foot_sides, TrackPart.indexSequenze_foot_sides, TrackPart.texFac_foot_sides, matBase);
      if (!stopWorldPos)
          track_foot_side_r.worldPositioning(kmlSplineTrase);
      GameObject spatial_foot_side_right = track_foot_side_r.meshMaker();
      spatial_foot_side_right.name = "Right Side Foot Piece";
      spatial_foot_side_right.transform.parent = pivot2.transform;
      
      pivot2.transform.position += new Vector3(-0.7175f,0,0);
      
      GameObject pivot_gravel = new GameObject();
      pivot_gravel.name = "Track Terrain";
      TrackPart track_gravel_1 = new TrackPart(startVertex, endVertex, 6, TrackPart.to_gravel, TrackPart.txo_gravel, 
          TrackPart.indexSequenze_gravel, TrackPart.texFac_gravel, matBase);
      track_gravel_1.localPositioning(0,0,0);
      if (!stopWorldPos)
          track_gravel_1.worldPositioning(kmlSplineTrase);
      GameObject spatial_gravel_1  = track_gravel_1.meshMaker();
      spatial_gravel_1
      .transform.parent = pivot_gravel.transform;
      pivot = new GameObject().transform;
      pivot.gameObject.name = "Track Object";
      pivot.parent = GameObject.Find("Track").transform;
      pivot1.transform.parent = pivot;
      pivot2.transform.parent = pivot;
      pivot_gravel.transform.parent = pivot;
      pivot.position = startVertex;
  } 


}
