using System;
using UnityEngine;

class CubicSpline : Spline
{
  /**
   * Construct a cubic spline. Package local; Use the SplineFactory
   * to create splines of this type. The control points are used according
   * to the definition of cubic splines.
   * 
   * @param controlPoints  Control points of spline (x0,y0,z0,x1,y1,z1,...)
   * @param nParts         Number of parts in generated spline.
   */
  public CubicSpline (double[] controlPoints, int nParts)
  {
    initialize(controlPoints, nParts);
  }


  
  protected void initialize (double[] controlPoints, int nParts)
  {
    Debug.Log(controlPoints.Length + " - point length");
    nParts_ = nParts;
    Debug.Log(nParts_ + " - parts");
    // Endpoints are added three times to get them include in the
    // generated array    
    controlPoints_ = new double[controlPoints.Length + 12];
    Array.Copy(controlPoints, 0, controlPoints_, 6,
                      controlPoints.Length);
    
    controlPoints_[0] = controlPoints_[6];
    controlPoints_[1] = controlPoints_[7];
    controlPoints_[2] = controlPoints_[8];
    
    controlPoints_[3] = controlPoints_[6];
    controlPoints_[4] = controlPoints_[7];
    controlPoints_[5] = controlPoints_[8];

    controlPoints_[controlPoints_.Length - 3] = controlPoints_[controlPoints_.Length - 9];
    controlPoints_[controlPoints_.Length - 2] = controlPoints_[controlPoints_.Length - 8];
    controlPoints_[controlPoints_.Length - 1] = controlPoints_[controlPoints_.Length - 7];

    controlPoints_[controlPoints_.Length - 6] = controlPoints_[controlPoints_.Length - 9];
    controlPoints_[controlPoints_.Length - 5] = controlPoints_[controlPoints_.Length - 8];
    controlPoints_[controlPoints_.Length - 4] = controlPoints_[controlPoints_.Length - 7];
  }
  

    
  /**
   * Generate this spline.
   * 
   * @return  Coordinates of the spline (x0,y0,z0,x1,y1,z1,...)
   */

    override
  public double[] generate()
  {
    int n = controlPoints_.Length / 3;
    int length = (n - 3) * nParts_ + 1;
    double[] spline = new double[length * 3];

    p(2, 0, controlPoints_, spline, 0);
      
    int index = 3;
    for (int i = 2; i < n - 1; i++) {
      for (int j = 1; j <= nParts_; j++) {
        p (i, j / (double) nParts_, controlPoints_, spline, index);
        index += 3;
      }
    }
      
    return spline;      
  }

    

  private void p (int i, double t, double[] cp, double[] spline, int index)
  {
    double x = 0.0;
    double y = 0.0;
    double z = 0.0;
      
    int k = (i - 2) * 3;
    for (int j = -2; j <= 1; j++) {
      // TODO: Precompute blending matrix
      double b = blend (j, t);
      
      x += b * cp[k++];
      y += b * cp[k++];
      z += b * cp[k++];
    }
      
    spline[index + 0] = x;
    spline[index + 1] = y;
    spline[index + 2] = z;    
  }

    
    
  protected double blend (int i, double t)
  {
    if      (i == -2) return (((-t + 3) * t - 3) * t + 1) / 6;
    else if (i == -1) return (((3 * t - 6) * t) * t + 4) / 6;
    else if (i ==  0) return (((-3 * t + 3) * t + 3) * t + 1) / 6;
    else              return (t * t * t) / 6;
  }
}
