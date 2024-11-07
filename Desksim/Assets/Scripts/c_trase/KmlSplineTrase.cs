using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class KmlSplineTrase : GenerellTrase
{

	private KmlSpline spline;
	private Vector3 startWorldPoint;
  private float sectionTrackLength;
  private float nextSectionStartAngle;
  private float sirkelTraseStartPos;
  private float sirkelTraseSluttPos;
  private SirkelTrase sirkelTrase; 
  private bool includeSirkelTrase = false;
  private List<Vector3> splineKontrollPunktListe;

    public KmlSplineTrase(Vector3 startPunkt, Vector3 sluttPunkt, double startPos) : base(startPunkt, sluttPunkt, startPos)
    {

    }

    public void InitSplineTrase(string kmlFilnavn, Vector3 startWorldPoint, 
      Vector3 startCorrOffs, Vector3 endCorrOffs, Vector3 startPunkt)
    {
        this.startWorldPoint = startWorldPoint;
		KmlReader kmlTr = new KmlReader(kmlFilnavn, new Vector3(startPunkt.x, startPunkt.y, startPunkt.z), 
            startCorrOffs, endCorrOffs);

		spline = kmlTr.getTraseSpline();
		
		lengde = spline.getLengde();
    }

    public void InitSplineTrase(double startPos, string kmlFilnavn, Vector3 startWorldPoint, 
          Vector3 startCorrOffs, Vector3 endCorrOffs)
    {
		this.startWorldPoint = startWorldPoint;

		KmlReader kmlTr = new KmlReader(kmlFilnavn, new Vector3(), startCorrOffs, endCorrOffs);
    
    spline = kmlTr.getTraseSpline();
    
    splineKontrollPunktListe = kmlTr.getSplineKontrollPunktListe();

		if (spline == null)
		{
			Debug.Log("Error during reading of path for section, program ends");
			Application.Quit();
		}
		
    }

//  public void setIncludeSirkelTrase(boolean includeSirkelTrase)
//  {
//    this.includeSirkelTrase = includeSirkelTrase;
//  }

  public float getSectionTrackLength()
  {
    return sectionTrackLength;
  }

//  public void setSectionAndNextSectionData(float sectionTrackLength, float nextSectionStartAngle)
//  {
//    this.sectionTrackLength = sectionTrackLength;
//    this.nextSectionStartAngle = nextSectionStartAngle;
//    
//    this.sirkelTraseStartPos = sectionTrackLength - SirkelTrase.SIRKELTRASE_LENGDE;
//    this.sirkelTraseSluttPos = sectionTrackLength;
//    
//    Vector3f vectorVedObergangTilSirkelTrase = finn3DPunktIPos(sirkelTraseStartPos);
//    float vVedOvergangTilSirkelTrase = finnVinkelIPos(sirkelTraseStartPos);
//    vVedOvergangTilSirkelTrase *= FastMath.RAD_TO_DEG;
//    
//    System.out.println("vink inn: " + vVedOvergangTilSirkelTrase);
//    System.out.println("vink ut: " + nextSectionStartAngle  ) ;
//    
//    sirkelTrase = new SirkelTrase(vVedOvergangTilSirkelTrase, nextSectionStartAngle, 
//            sectionTrackLength, vectorVedObergangTilSirkelTrase);
//  }
  
//	@Override
//	public Vector3f finn3DPunktIPos(double pos)
//	{
//		Vector3f p = spline.finn3DPunkt(pos);
//		
//    p.x += startWorldPoint.x;
//    p.y += startWorldPoint.y;
//    p.z += startWorldPoint.z;
//    
//		return p;
//	}
  
	public override Vector3 finn3DPunktIPos(float pos)
	{
		Vector3 p = spline.finn3DPunkt(pos);
    // Debug.Log(p + " - Find point");
		p.z -= spline.finn3DPunkt(0).z; 

    p.x += startWorldPoint.x;
    p.y += startWorldPoint.y;
    p.z += startWorldPoint.z;
    
    //Debug.Log(startWorldPoint + " - Start point point");
//    if (includeSirkelTrase && pos > sirkelTraseStartPos /*&& pos < sirkelTraseSluttPos*/)
//    {
//      Vector3f vec1 = sirkelTrase.finn3dPointISeksjonPos((float)pos);
//      p.x = vec1.x;
//      p.z = vec1.z;
//    }
    //Debug.Log(p + " - End point");
		return p;
	}

//	@Override
//	public float finnVinkelIPos(float pos)
//	{
//		return spline.finnVinkel(pos);
//	}
  
	public override float finnVinkelIPos(float pos)
	{
    float v1 = spline.finnVinkel(pos);
    
//    if (includeSirkelTrase && pos > sirkelTraseStartPos /*&& pos < sirkelTraseSluttPos*/)
//    {
//      v1 = sirkelTrase.finnVinkelRadISeksjonPos(pos);
//    }
    
		return v1;
	}

// DENNE MÅ SKRIVES HELT OM MED NY LOGIKK
  public Vector3 finnVertexITraseVertex(Vector3 vecIn)
  {
    Debug.Log(vecIn + " - VecIn");
    float ang = finnVinkelIPos(vecIn.z);
    Debug.Log(ang + " - Angle");
    Vector3 v = finn3DPunktIPos(vecIn.z);
    //Debug.Log(v + " - Punkt");

    Vector3 v2 = new Vector3(vecIn.x, vecIn.y, 0);
    //Debug.Log(v2);
    GameObject t1 = new GameObject();
    GameObject t2 = new GameObject(); 
    t1.transform.rotation = AnglesVectors.fromAngleAxis(t1.transform.rotation, ang, Vector3.up);
    Debug.Log(t1.transform.rotation.x + "," + t1.transform.rotation.y + "," + t1.transform.rotation.z + "," + t1.transform.rotation.w + " - Roty");
    t1.transform.position = v;
    t2.transform.name = "assdt";
    t2.transform.position = t1.transform.position;
    t2.transform.parent = t1.transform;
    t2.transform.position += v2;


    Vector3 v3 = t2.transform.position;

    v3.x += v.x;
    v3.y += v.y;
    v3.z += v.z;

    t2.transform.position = v3;
    Vector3 vecOut = new Vector3();
    vecOut.x = v3.x;
    vecOut.y = v3.y;
    vecOut.z = v3.z;
    GameObject.Destroy(t1);
    GameObject.Destroy(t2);
    Debug.Log("VecOut: " + vecOut);
    return vecOut;
  }

    public override void finnVertexITraseVertex(Vector3 vecIn, Vector3 vecOut)
  {
  }
	
	public KmlSpline getSpline()
	{
		return spline;
	}

  public List<Vector3> getSplineKontrollPunktListe()
  {
    return splineKontrollPunktListe;
  }
}
