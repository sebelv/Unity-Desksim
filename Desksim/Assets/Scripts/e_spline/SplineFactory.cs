using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineFactory
{
  private double[] c;  
  private double sizeExpand = 1;

  
  public SplineFactory()
  {
    
    c = new double[18];
    c[0]  =   0.0;  // x0
    c[1]  =   0.0;  // y0
    c[2]  =   0.0;  // z0      
    
    c[3]  =   50.0;  // x1
    c[4]  =   60.0;  // y1
    c[5]  =   0.0;  // z1      

    c[6]  =   100.0;  // x2
    c[7]  =  	50.0;  // y2
    c[8]  =   0.0;  // z2      

    c[9]  =  	150.0;  // x3
    c[10] =   60.0;  // y3
    c[11] =   0.0;  // z3      
    
    c[12]  =  200.0;  // x4
    c[13] =   100.0;  // y4
    c[14] =   0.0;  // z4    
    
    c[15]  =  250.0;  // x5
    c[16] =   90.0;  // y5
    c[17] =   0.0;  // z5    
    
    for (int i = 0; i < c.Length; i++)
    {
    	c[i] *= sizeExpand;
    }
    
    double[] spline1 = SplineFactory.createBezier (c,     10);
    double[] spline2 = SplineFactory.createCubic (c,      10);
    double[] spline3 = SplineFactory.createCatmullRom (c, 10);        

    Debug.Log("-- Bezier");
    for (int i = 0; i < spline1.Length; i+=3)
      Debug.Log(spline1[i] + "," + spline1[i+1] + "," + spline1[i+2]);
    
    Debug.Log("-- Cubic");
    for (int i = 0; i < spline2.Length; i+=3)
      Debug.Log(spline2[i] + "," + spline2[i+1] + "," + spline2[i+2]);

    Debug.Log("-- Catmull-Rom");
    for (int i = 0; i < spline3.Length; i+=3)
      Debug.Log(spline3[i] + "," + spline3[i+1] + "," + spline3[i+2]);

	}

  public static double[] createBezier (double[] controlPoints, int nParts)
  {
    Spline spline = new BezierSpline(controlPoints, nParts);
    return spline.generate();
  }


  
  /**
   * Create a cubic spline based on the given control points.
   * The generated curve starts in the first control point and ends
   * in the last control point.
   * 
   * @param controlPoints  Control points of spline (x0,y0,z0,x1,y1,z1,...).
   * @param nParts         Number of parts to divide each leg into.
   * @return               Spline (x0,y0,z0,x1,y1,z1,...).
   */
  public static double[] createCubic (double[] controlPoints, int nParts)
  {
    Spline spline = new CubicSpline (controlPoints, nParts);
    return spline.generate();
  }


  
  /**
   * Create a Catmull-Rom spline based on the given control points.
   * The generated curve starts in the first control point and ends
   * in the last control point.
   * Im addition, the curve intersects all the control points.
   * 
   * @param controlPoints  Control points of spline (x0,y0,z0,x1,y1,z1,...).
   * @param nParts         Number of parts to divide each leg into.
   * @return               Spline (x0,y0,z0,x1,y1,z1,...).
   */
  public static double[] createCatmullRom (double[] controlPoints, int nParts)
  {
    Spline spline = new CatmullRomSpline(controlPoints, nParts);
    return spline.generate();
  }
}
