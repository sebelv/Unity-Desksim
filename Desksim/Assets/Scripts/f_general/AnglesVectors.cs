using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglesVectors
{
  private static Vector3 vec = new Vector3();
  private static float latestCalculatedAngle;
  private static TrackNode latestFoundRefTrackNode;
  private static Vector3 temporaryTrackBasedVertex = new Vector3();
  private static Vector3 refVecTrase = new Vector3();
  private static Vector3 refVecWorld = new Vector3();
  private static Vector3 xVecTrase = new Vector3();
  private static Vector3 offsVecWorld = new Vector3();
  private static Transform offsVecTrans;
  private static Quaternion rotYQuat = new Quaternion();
  private static Vector3 latestFoundPathPoint = new Vector3();
  private static Vector3 latestFoundTrackWorldPoint = new Vector3();
  private static TrackNode latestFoundTrackNode1;
  private static TrackNode latestFoundTrackNode2;
  public static Quaternion resetTilNullGrader = fromAngleAxis(new Quaternion(), 0, Vector3.up);

  public static float finnVinkelOmYForVector(Vector3 vect)
  {
    vec.x = vect.x;
		vec.z = vect.z;

    return finnVinkelOmYForVec();
  }
  
  private static float finnVinkelOmYForVec()
  {
//    if (vec.z == 0)
//      vec.z = 0.0001f;
    if (vec.z < 0)
      return Mathf.PI + Mathf.Atan(vec.x / vec.z);
    
		return Mathf.Atan(vec.x / vec.z);
  }
  
  public static float finnVinkelOmY(Vector3 p1, Vector3 p2)
	{
    vec.x = p2.x - p1.x;
    vec.y = p2.y - p1.y;
    vec.z = p2.z - p1.z;    
		
		float mot = vec.x;
		float hos = vec.z;
    
    float vinkel = Mathf.Atan(mot / hos);

    if (vec.z < 0)
      vinkel = Mathf.PI + vinkel;
    
    
		return vinkel;
	}
  
  
  public static float finnVinkelOmX(Vector3 p1, Vector3 p2)
	{
		float dist = Vector3.Distance(p1, p2);
		float sina = (p2.y - p1.y) / dist;
		float a = Mathf.Asin(sina);
		
		return a;
	}
  
  public static float getLatestCalculatedAngle()
  {
    return latestCalculatedAngle;
  }

  public static Vector3 getLatestFoundPathPoint()
  {
    return latestFoundPathPoint;
  }
  
  /**
   * 
   * @param trackPoint - x: to find correct track if parallell tracks, y: not used, z: position along path
   * @param allTrackNodes
   * @return 
   */
  public static TrackNode findReferenceTrackNode(Vector3 trackPoint, 
    List<TrackNode> allTrackNodes)
  {
    latestFoundRefTrackNode = findClosestTrackNodeTraseVertex(trackPoint, allTrackNodes);
    
    return latestFoundRefTrackNode;
  }
  
  /**
   * 
   * @param trackBasedVertex - x: offset away from track y: offset away from track z: position along path
   * @param resultworldPoint
   * @return 
   */
  public static Vector3 findWorldPointAlongTrackUsingLatestFoundReferenceTrackNode(Vector3 trackBasedVertex, 
    Vector3 resultworldPoint )
  {
    if (trackBasedVertex.z > latestFoundRefTrackNode.getTempPoint().z)
      findPairForwards(trackBasedVertex, resultworldPoint, latestFoundRefTrackNode);
    else
      findPairBackwards(trackBasedVertex, resultworldPoint, latestFoundRefTrackNode);
    
    return new Vector3();
  }
  
  public static void findWorldPointAlongTrack(Vector3 trackPoint, Vector3 offsetVector,
    Vector3 resultworldPoint, List<TrackNode> allTrackNodes)
  { 
    findReferenceTrackNode(trackPoint, allTrackNodes);
    
    if (offsetVector != null)
    {
      temporaryTrackBasedVertex.x = offsetVector.x;
      temporaryTrackBasedVertex.y = offsetVector.y;
    }
    else
    {
      temporaryTrackBasedVertex.x = 0;
      temporaryTrackBasedVertex.y = 0;
    }
    
    temporaryTrackBasedVertex.z = trackPoint.z;
    findWorldPointAlongTrackUsingLatestFoundReferenceTrackNode(temporaryTrackBasedVertex, resultworldPoint);
  }
  

  private static void findPairForwards(Vector3 trackBasedVertex, Vector3 resultworldPoint, TrackNode refTn)
  {
//    System.out.println("trackBasedVertex: " + trackBasedVertex);
    TrackNode runner = refTn;
    while(runner != null && runner.getTempPoint().z < trackBasedVertex.z)
    {
      runner = runner.neste;
    }
    TrackNode setInnHerNode = runner.forrige;
    TrackNode t1 = runner.forrige;
    
    // sikre en viss avstand mellom kalkulasjonsnoder
    while(runner != null && (Vector3.Distance(runner.getTempPoint(), t1.getTempPoint()) < 1.0f) )
    {
      runner = runner.neste;
    }
    TrackNode t2 = runner;
    
    findExactPoint(trackBasedVertex, resultworldPoint,t1, t2, setInnHerNode);
  }
  
  private static void findPairBackwards(Vector3 trackBasedVertex,
    Vector3 resultworldPoint, TrackNode refTn)
  {
    TrackNode runner = refTn;
    while(runner != null && runner.getTempPoint().z > trackBasedVertex.z)
    {      
      runner = runner.forrige;
    }
    TrackNode setInnHerNode = runner;
    TrackNode t2 = runner.neste;
    
    // sikre en viss avstand mellom kalkulasjonsnoder
    while(runner != null && (Vector3.Distance(runner.getTempPoint(), t2.getTempPoint()) < 1.0f) )
    {
      runner = runner.forrige;
    }
    TrackNode t1 = runner;
    
    findExactPoint(trackBasedVertex, resultworldPoint, t1, t2, setInnHerNode);
  }

  private static void findExactPoint(Vector3 trackBasedPointVertex, 
    Vector3 resultworldPoint, TrackNode t1, TrackNode t2, TrackNode tSettInn)
  {
    Vector3 pt1 = t1.getTempPoint();
    Vector3 pt2 = t2.getTempPoint();

    Vector3 pw1 = t1.getPoint();
    Vector3 pw2 = t2.getPoint();
   
    float len = pt2.z - pt1.z;
    float del = trackBasedPointVertex.z - pt1.z;
    float alpha = del / len;
    
    latestFoundPathPoint.x = pt1.x;
    latestFoundPathPoint.y = pt1.y;
    latestFoundPathPoint.z = pt1.z;
    refVecTrase.x = (pt2.x - pt1.x) * alpha;
    refVecTrase.y = (pt2.y - pt1.y) * alpha;
    refVecTrase.z = (pt2.z - pt1.z) * alpha;
    latestFoundPathPoint += refVecTrase;
    
    refVecWorld.x = (pw2.x - pw1.x) * alpha;
    refVecWorld.y = (pw2.y - pw1.y) * alpha;
    refVecWorld.z = (pw2.z - pw1.z) * alpha;
    resultworldPoint = pw1+refVecWorld;

    // copy worldpoint before offset is added to be used for TrackNode
    latestFoundTrackWorldPoint.x = resultworldPoint.x;
    latestFoundTrackWorldPoint.y = resultworldPoint.y;
    latestFoundTrackWorldPoint.z = resultworldPoint.z;
    //

    offsVecWorld.x = trackBasedPointVertex.x;
    offsVecWorld.y = trackBasedPointVertex.y;
    offsVecWorld.z = 0;
    
    float vi = finnVinkelOmY(pw1, pw2);
    latestCalculatedAngle = vi;
    
    fromAngleAxis(rotYQuat, vi, Vector3.up);
    offsVecTrans.rotation = rotYQuat;
    offsVecTrans.position = offsVecWorld;
    
    resultworldPoint.x += offsVecWorld.x;
    resultworldPoint.y += offsVecWorld.y;
    resultworldPoint.z += offsVecWorld.z;
    
    // tracknodes
//    latestFoundTrackNode1 = t1;
//    latestFoundTrackNode2 = t1.neste;
  
    // prøver endring her 130522 for å sikre at det alltid er stigande z-verdi frå node til node 
    latestFoundTrackNode1 = tSettInn;
    latestFoundTrackNode2 = tSettInn.neste;
  }
  
  public static void connectTrackNodeUsingLatestFoundInsertTrackNodes(TrackNode tn)
  {
    // koble tracknodes som denne tracknoden var mellom før
    if (tn.neste != null && tn.forrige != null)
    {
      tn.forrige.neste = tn.neste;
      tn.neste.forrige = tn.forrige;
    }
    
    latestFoundTrackNode1.neste = tn;
    tn.neste = latestFoundTrackNode2;
    latestFoundTrackNode2.forrige = tn;
    tn.forrige = latestFoundTrackNode1;

    tn.getTempPoint().Set(latestFoundTrackWorldPoint.x, latestFoundTrackWorldPoint.y ,latestFoundTrackWorldPoint.z);
    
//    System.out.println("lf1: " + latestFoundTrackNode1.getTempPoint());
//    System.out.println("lf2: " + latestFoundTrackNode2.getTempPoint());
  }

  private static TrackNode findClosestTrackNodeTraseVertex(Vector3 traseVertex, List<TrackNode> allTrackNodes)
  {
    TrackNode ctn = allTrackNodes[0];
    float cd = Vector3.Distance(traseVertex, ctn.getTempPoint());
    
    foreach (TrackNode tn in allTrackNodes)
    {
      float d = Vector3.Distance(tn.getTempPoint(), traseVertex);
      if (d < cd)
      {
        cd = d;
        ctn = tn;
      }
    }
    
    return ctn;
  }

  public static Quaternion fromAngleAxis(Quaternion quat, float angle, Vector3 axis) 
  {
    axis.Normalize();
    Quaternion qoot = fromAngleNormalAxis(quat, angle, axis);
    return qoot;
  }
  public static Quaternion fromAngleNormalAxis(Quaternion quat, float angle, Vector3 axis) 
  {
    if (axis.x == 0 && axis.y == 0 && axis.z == 0) 
    {
        quat.x=quat.y=quat.z=0;
        quat.w = 1;
    } 
    else 
    {
        float halfAngle = 0.5f * angle;
        float sin = Mathf.Sin(halfAngle);
        quat.w = Mathf.Cos(halfAngle);
        quat.x = sin * axis.x;
        quat.y = sin * axis.y;
        quat.z = sin * axis.z;
    }
    return quat;
  }
}
