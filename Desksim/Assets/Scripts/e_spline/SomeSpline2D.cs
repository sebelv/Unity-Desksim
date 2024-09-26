using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class SomeSpline2D
{
	private int numParts = 25;
	private List<Vector3> splineKontrollPunkt = new List<Vector3>();
	private double[] someSpline;
	private List<Double> vertexDistanceFromStartList = new List<Double>();
	private double[] vertexDistanceFromStartArray;
	private List<Vector3> vertexList = new List<Vector3>();
	private double totLengde;
	//private boolean cubic = true;
	private int splineType = SPLINE_TYPE_CUBIC; // DEFAULT CUBIC
	
	public static int SPLINE_TYPE_CATMULL = 0;
	public static int SPLINE_TYPE_CUBIC = 1;
	public static int SPLINE_TYPE_BEZIER = 2;
  
  private float sisteVinkelXZ;
	
	
	public SomeSpline2D(List<Vector3> splineKontrollPunkt, int numParts)
	{
		init(splineKontrollPunkt, numParts);
	}
	
	public SomeSpline2D(List<Vector3> splineKontrollPunkt, int numParts, int splineType)
	{
		this.splineType = splineType;
		init(splineKontrollPunkt, numParts);
	}
	
  private void findNumParts()
  {
    float lengde = 0;
    Vector3 vf = splineKontrollPunkt[0];
    foreach (Vector3 v in splineKontrollPunkt)
    {
      lengde += Vector3.Distance(v,vf);
      vf = v;
    }
    
    float gjSnAvstMelKtrPkt = lengde / splineKontrollPunkt.Count;
    int numP = (int)(gjSnAvstMelKtrPkt / 5.0f);

    if (numP < 25)
      this.numParts = 25;
    else
      this.numParts = numP;
  }
  
	private void init(List<Vector3> splineKontrollPunkt, int numParts)
	{
		this.splineKontrollPunkt = splineKontrollPunkt;
		//this.numParts = numParts;
    findNumParts();
		genererSpline();
		genererVertexListe();
		genererDistanser();
	}
	
	public void setSplineKontrollPunkt(List<Vector3> splineKontrollPunkt)
	{
		this.splineKontrollPunkt = splineKontrollPunkt;
	}
	
//	public void setCubic(boolean cubic)
//	{
//		this.cubic = cubic;
//	}
	
//	public void setSplineType(int splineType)
//	{
//		this.splineType = splineType;
//	}

	public void reGenerate()
	{
		vertexList.Clear();
		vertexDistanceFromStartList.Clear();

		genererSpline();
		genererVertexListe();
		genererDistanser();
	}
	
	public float finnVinkel(float distanseMeter)
	{
		int index = finnIndex(distanseMeter);
    
		if (index == 0)
			index = 1;
		if (index >= vertexDistanceFromStartArray.Length)
			index = vertexDistanceFromStartArray.Length - 1;
		
		Vector3 p1 = vertexList[index - 1];
		Vector3 p2 = vertexList[index];
		
    sisteVinkelXZ = finnVinkelXZ(p1, p2);
    
		return finnVinkel(p1, p2);
	}
	
	public Vector3 finn3DPunkt(double distanseMeter)
	{
		int index = finnIndex(distanseMeter);
		
		if (index == 0)
			index = 1;
		if (index >= vertexDistanceFromStartArray.Length)
			index = vertexDistanceFromStartArray.Length - 1;
		
		double alpha = (distanseMeter - vertexDistanceFromStartArray[index - 1]) / 
			(vertexDistanceFromStartArray[index] - vertexDistanceFromStartArray[index - 1]);
		
		Vector3 p = vertexList[index - 1];
		p = Vector3.Lerp(p, vertexList[index], (float)alpha);
		
		// tilpass
//		double temp = p.z;
//		p.z = p.y;
//		p.y = temp;
//		p.x = -p.x;
		
		return p;
	}

  public float getSisteVinkelXZ()
  {
    return sisteVinkelXZ;
  }
	
	public double getDistanceAtKontrollPunkt(int kontrollPunktIndex)
	{
//		int index = kontrollPunktIndex * numParts;
//		
//		return vertexDistanceFromStartArray[index];
		
		double akkDist = 0.0;
		Vector3 forrigeP = splineKontrollPunkt[0];
		
		for (int i = 0; i < kontrollPunktIndex; i++)
		{
			akkDist += Vector3.Distance(splineKontrollPunkt[i+1], forrigeP);
			forrigeP = splineKontrollPunkt[i + 1];
		}
		
		return akkDist;
	}
	
	public int getAntallKontrollPunkt()
	{
		return splineKontrollPunkt.Count;
	}
	
	public double getLengde()
	{
		return totLengde;
	}
	
	public double getSluttPos()
	{
		return totLengde;
	}

	private float finnVinkelXZ(Vector3 p1, Vector3 p2)
	{
    Vector3 v = p2 - p1;
		
		float mot = v.x;
		float hos = v.z;
    
    float vinkel = Mathf.Atan(mot / hos);
    
		if (v.z < 0)
    {
			vinkel = Mathf.PI + vinkel;
    }
		else if (v.x < 0)
    {
			vinkel = (Mathf.PI * 2) + vinkel;
    }
		
		return vinkel;
	}  
  
	private float finnVinkel(Vector3 p1, Vector3 p2)
	{
    Vector3 v = p2 - p1;
		
		float mot = v.x;
		float hos = v.y;
    
    float vinkel = Mathf.Atan(mot / hos);
    
		if (v.y < 0)
    {
			vinkel = Mathf.PI + vinkel;
    }
		else if (v.x < 0)
    {
			vinkel = (Mathf.PI * 2) + vinkel;
    }
		
		return -vinkel;
	}
	
	private int finnIndex(double distance)
	{
		int index = Array.BinarySearch(vertexDistanceFromStartArray, distance);
		
		if (index >= 0)
			return index;
		
		return (-index) - 1;
	}
	
	private void genererSpline()
	{
		double[] c = new double[splineKontrollPunkt.Count * 3];
		
		int i = 0;
		
		foreach (Vector3 p in splineKontrollPunkt)
		{
			c[i] = p.x;
			c[i + 1] = p.y;
			c[i + 2] = p.z;
			
			i += 3;
		}
		
		if (splineType == SPLINE_TYPE_CUBIC)
			someSpline = SplineFactory.createCubic(c, numParts);
		else if (splineType == SPLINE_TYPE_CATMULL)
			someSpline = SplineFactory.createCatmullRom(c, numParts);
		else // bezier
			someSpline = SplineFactory.createBezier(c, numParts);
	}
	
	private void genererVertexListe()
	{
		for (int i = 0; i < someSpline.Length; i += 3)
		{
			vertexList.Add(new Vector3((float)someSpline[i], (float)someSpline[i+1], (float)someSpline[i+2]));	
		}
    
	}
	
	private void genererDistanser()
	{
		double[] vertices = someSpline;
		Vector3 forrigeP = new Vector3();
		double total = 0;
		
		for (int i = 0; i < vertices.Length; i += 3)
		{
			Vector3 p = new Vector3((float)vertices[i], (float)vertices[i + 1], (float)vertices[i + 2]);
			//vertexList.add(p);
			
			if (i == 0)
				vertexDistanceFromStartList.Add(0.0);
			else
			{
				total += Vector3.Distance(forrigeP,p);
				vertexDistanceFromStartList.Add(total);
			}
					
			forrigeP = p;	
		}
		
		totLengde = total;
		tilArray();
		
		int k  = 0;
		foreach (Vector3 p in vertexList)
		{
			//System.out.println("P: " + k + " " + p);
			k++;
		}
	}
	
	private double[] tilArray()
	{
		vertexDistanceFromStartArray = new double[vertexDistanceFromStartList.Count];
		
		for (int i = 0; i < vertexDistanceFromStartList.Count; i++)
			vertexDistanceFromStartArray[i] = vertexDistanceFromStartList[i];
		
		return vertexDistanceFromStartArray;
	}
  
  public List<Vector3> getVertexList()
  {
    return vertexList;
  }
}
