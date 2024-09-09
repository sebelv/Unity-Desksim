using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongLat
{
	private static float OMKRETS_VED_EKVATOR = 40075020;
	private static float OMKRETS_VED_POLANE = 40008000;
	private static float M_PR_GRAD_EKVATOR = OMKRETS_VED_EKVATOR / 360;
	private static float M_PR_GRAD_LAT = OMKRETS_VED_POLANE / 360;
	
	//private Vector3f googleEarthNullPunkt;
  private Vector3 googleEarthNullPunkt;
  
	public LongLat(Vector3 googleEarthNullPunkt)
	{
		this.googleEarthNullPunkt = googleEarthNullPunkt;
	}
	
	public List<Vector3> finnKoordinatPunkter(List<Vector3> googleEarthPunkter)
	{
		List<Vector3> pL = new List<Vector3>();
		
		foreach (Vector3 p in googleEarthPunkter)
			pL.Add(finnKoordinatPunkt(p));
		
		return pL;
	}
	
	public Vector3 finnKoordinatPunkt(Vector3 googleEarthPunkt)
	{
		double meterPrGrad1 = meterPrGrad(googleEarthPunkt.y);
		
		double x = (googleEarthPunkt.x - googleEarthNullPunkt.x) * meterPrGrad1;
		double y = (googleEarthPunkt.y - googleEarthNullPunkt.y) * M_PR_GRAD_LAT;
		
		return new Vector3((float)x, (float)y, googleEarthPunkt.z);
	}

	private double meterPrGrad(float lengdeGrad)
	{
		return Mathf.Cos((Mathf.Deg2Rad * (lengdeGrad)) * M_PR_GRAD_EKVATOR);
	}
	
//	public static void main(String[] args)
//	{
//		new LongLat(new Vector3f(0,0,0));
//	}
}
