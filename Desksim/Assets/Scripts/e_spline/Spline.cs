using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An abstract class defining a general spline object.
 * 
 */   
abstract class Spline
{
  protected double[] controlPoints_;
  protected int    nParts_;

  public abstract double[] generate();
}
