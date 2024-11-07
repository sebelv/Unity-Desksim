using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class KmlSpline
{
	private int numParts = 25;
	private LongLat longLat;
	private List<Vector3> splineKontrollPunkt = new List<Vector3>();
	private double totLengde;
	private string textureFile = "";
	private double textureFactor = 1;
	private int numXVertices = 5;
	private bool shapeUsesXSpline = true;
	private List<Vector3> googleEarthCoords = new List<Vector3>();
	private List<Vector3> distanceHeightList;
	private SomeSpline2D crs2DXZ;
	private SomeSpline2D crs2DY;
	
	//
	//private ArrayList<Shape3DSplineControlPoint> s3dScpxzList = new ArrayList<Shape3DSplineControlPoint>();
//	private BranchGroup bg = new BranchGroup();
	private double DIST_BET_Y = 100.0;
	
	//private ArrayList<Shape3DSplineControlPoint> s3dScpyList = new ArrayList<Shape3DSplineControlPoint>();
	
	
	public KmlSpline(List<Vector3> googleEarthCoords, Vector3 googleEarthNullpunkt,
			List<Vector3> distanceHeightList, Vector3 startKorr, Vector3 sluttKorr)
	{
		this.googleEarthCoords.AddRange(googleEarthCoords);
		this.distanceHeightList = distanceHeightList;

		longLat = new LongLat(googleEarthNullpunkt);
		finnSplineKontrollPunkt(googleEarthCoords);
		
    //
    int k = 0;
    for (int i = 0; i < splineKontrollPunkt.Count; i++)
    {
      if (k > 0)
      {
        splineKontrollPunkt[i].Set(splineKontrollPunkt[i].x -startKorr.x, splineKontrollPunkt[i].y - startKorr.z, splineKontrollPunkt[i].z);
      }
      
      if (k == (splineKontrollPunkt.Count-1))
      {
        splineKontrollPunkt[i].Set(splineKontrollPunkt[i].x -sluttKorr.x, splineKontrollPunkt[i].y - sluttKorr.z, splineKontrollPunkt[i].z);
      }
      k++;
    }
    
    
		//
//		for (Vector3f p: splineKontrollPunkt)
//		{
//			Shape3DSplineControlPoint s = new Shape3DSplineControlPoint(this, 0);
//			s.drag(new Vector3f(-p.x, p.z, p.y)); // bytte av y og z
//			s3dScpxzList.add(s);
//			bg.addChild(s.getBranchGroup());
//		}
		crs2DXZ = new SomeSpline2D(splineKontrollPunkt, numParts);
		totLengde = crs2DXZ.getLengde();
    
		if (this.distanceHeightList != null)
		{
			Debug.Log("DETTE GJØRES HAHA");
			crs2DY = new SomeSpline2D(distanceHeightList, numParts);
		}
		
//		distanceHeightList = new ArrayList<Vector3f>();
//		distanceHeightList.add(new Vector3f(0,0,0));
//		distanceHeightList.add(new Vector3f(0,0,totLengde));
		

//		for (Vector3f p: distanceHeightList)
//		{
//			Shape3DSplineControlPoint s = new Shape3DSplineControlPoint(this, 1);
//			s.setZ(p.z);
//			Vector3f np = crs2DXZ.finn3DPunkt(p.z);
//			
//			s.drag(new Vector3f(-np.x, p.y, np.y)); // bytte av y og z
//			s3dScpxzList.add(s);
//			bg.addChild(s.getBranchGroup());
//		}
		
		
		//crs2DY = new CatmullRomSpline2D(distanceHeightList, numParts);
		
		//genererSpline();
		//genererVertexListe();
		//genererDistanser();
	}
	
	public void etablerElevationProfile(List<Vector3> distanceHeightList)
	{
		this.distanceHeightList = distanceHeightList;
		crs2DY = new SomeSpline2D(distanceHeightList, numParts);
	}
	
	public void updateY()
	{
		distanceHeightList = new List<Vector3>();
		
//		for (Shape3DSplineControlPoint s: s3dScpxzList)
//		{
//			if (s.getType() == 1)
//				distanceHeightList.add(new Vector3f(0, s.getVertex().y, s.getZ()));
//		}
		
		crs2DY.setSplineKontrollPunkt(distanceHeightList);
		crs2DY.reGenerate();
		
		// update splinecontrolpoints
		int k = 0;
//		for (Shape3DSplineControlPoint s: s3dScpxzList)
//		{
//			if (s.getType() == 0)
//			{
//				double distance = getDistanceAtKontrollPunkt(k);
//				s.drag(new Vector3f(s.getVertex().x, finn3DPunkt(distance).y, s.getVertex().z ));
//				k++;
//			}
//		}
	}
	
//	public void update()
//	{
//		distanceHeightList.clear();
//		for (Shape3DSplineControlPoint s: s3dScpyList)
//		{
//			distanceHeightList.add(new Vector3f(0.0, s.getVertex().y, s.getZ()));
//		}
//		
//	}
	
//	public BranchGroup getBranchGroup()
//	{
//		return bg;
//	}
	
	public float finnVinkel(float distanseMeter)
	{
		return crs2DXZ.finnVinkel(distanseMeter);
	}
	
	public Vector3 finn3DPunkt(double distanseMeter)
	{
		Vector3 p = crs2DXZ.finn3DPunkt(distanseMeter);
		// Debug.Log(p + " - finn3DPunkt");
		// tilpass
		//double temp = p.z;
    float temp = p.z;
		p.z = p.y;
		p.y = temp;
		p.x = -p.x;
		
		// dersom eigen høgdeprofil
		if (crs2DY != null)
    {
			p.y = crs2DY.finn3DPunkt(distanseMeter).y;
      // 230424 - sikre at etterspurt z verdi er innanfor max definert z verdi i høydeprofil 
      if (distanseMeter >= distanceHeightList[distanceHeightList.Count-1].z) 
      {
        float maxDist = distanceHeightList[distanceHeightList.Count -1].z;
        p.y = crs2DY.finn3DPunkt(maxDist-0.1f).y;
      }
    }
		// Debug.Log(p + " - return3DPunkt");
		return p;
	}
	
	public double getDistanceAtKontrollPunkt(int kontrollPunktIndex)
	{
		return crs2DXZ.getDistanceAtKontrollPunkt(kontrollPunktIndex);
	}
	
	public int getAntallKontrollPunkt()
	{
		return crs2DXZ.getAntallKontrollPunkt();
	}
	
	public double getLengde()
	{
		return totLengde;
	}
	
	public double getSluttPos()
	{
		return totLengde;
	}
	
	public string getTextureFile()
	{
		return textureFile;
	}

	public void setTextureFile(string textureFile)
	{
		this.textureFile = textureFile;
	}

	public double getTextureFactor()
	{
		return textureFactor;
	}

	public void setTextureFactor(double textureFactor)
	{
		this.textureFactor = textureFactor;
	}

	public int getNumXVertices()
	{
		return numXVertices;
	}

	public void setNumXVertices(int numXVertices)
	{
		this.numXVertices = numXVertices;
	}
	
	public bool isShapeUsesXSpline()
	{
		return shapeUsesXSpline;
	}

	public void setShapeUsesXSpline(bool shapeUsesXSpline)
	{
		this.shapeUsesXSpline = shapeUsesXSpline;
	}

	public List<Vector3> getGoogleEarthCoords()
	{		
		return googleEarthCoords;
	}
	
  public List<Vector3> getSplineKontrollPunktListe()
  {
    return splineKontrollPunkt;
  }
  
//	public ArrayList<Shape3DSplineControlPoint> getS3dScpxzList()
//	{
//		return s3dScpxzList;
//	}

	private void finnSplineKontrollPunkt(List<Vector3> googleEarthCoords)
	{
		for (int i = 0; i < googleEarthCoords.Count; i++)
		{
			Debug.Log("google coord " + i + " - " + googleEarthCoords[i]);
			Debug.Log("google point " + i + " - " + longLat.finnKoordinatPunkt(googleEarthCoords[i]));
			splineKontrollPunkt.Add(longLat.finnKoordinatPunkt(googleEarthCoords[i]));
		}
	}
}
