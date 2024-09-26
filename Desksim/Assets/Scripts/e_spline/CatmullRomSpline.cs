using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

 class CatmullRomSpline : CubicSpline
{
  /**
   * Construct a Catmull-Rom spline. Package local; Use the SplineFactory
   * to create splines of this type. The control points are used according
   * to the definition of Catmull-Rom splines.
   * 
   * @param controlPoints  Control points of spline (x0,y0,z0,x1,y1,z1,...)
   * @param nParts         Number of parts in generated spline.
   */
  public CatmullRomSpline(double[] controlPoints, int nParts) : base(controlPoints, nParts)
  {
    initialize(controlPoints, nParts);
  }


  
  protected void initialize (double[] controlPoints, int nParts)
  {
    nParts_ = nParts;

    // Endpoints are added twice to force in the generated array
    controlPoints_ = new double[controlPoints.Length + 6];
    Array.Copy(controlPoints, 0, controlPoints_, 3,
                      controlPoints.Length);
    
    controlPoints_[0] = controlPoints_[3];
    controlPoints_[1] = controlPoints_[4];
    controlPoints_[2] = controlPoints_[5];
    
    controlPoints_[controlPoints_.Length - 3] = controlPoints_[controlPoints_.Length - 6];
    controlPoints_[controlPoints_.Length - 2] = controlPoints_[controlPoints_.Length - 5];
    controlPoints_[controlPoints_.Length - 1] = controlPoints_[controlPoints_.Length - 4];
  }
  

  
  protected double blend (int i, double t)
  {
    if      (i == -2) return ((-t + 2) * t - 1) * t / 2;
    else if (i == -1) return (((3 * t - 5) * t) * t + 2) / 2;
    else if (i ==  0) return ((-3 * t + 4) * t + 1) * t / 2;
    else              return ((t - 1) * t * t) / 2;
  }
}
