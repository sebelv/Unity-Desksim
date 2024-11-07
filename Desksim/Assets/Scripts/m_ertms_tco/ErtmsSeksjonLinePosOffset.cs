using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErtmsSeksjonLinePosOffset : TrackConnectedObject
{
  private float linePosOffset = -1;
  private List<Vector3> liste = new List<Vector3>();

  /*public void initAll(AssetManager assetManager)
  {
    modelFile = null;
    super.initAll(assetManager);
   
    if (Program.TRACK_BUILD_MODE)
    {
      init3DFigur(assetManager);
    }
  }  */
  
  protected void init3DFigur() 
  {
    /*
    
    //Dome(Vector3f center, int planes, int radialSamples, float radius, boolean insideView)
    Box box = new Box(0.1f, 1f, 0.1f);
    Spatial boxFigur = new Geometry("Box", box );
    Material mat = new Material( 
            assetManager, "Common/MatDefs/Misc/Unshaded.j3md");
    mat.setColor("Color", ColorRGBA.Yellow);
    boxFigur.setMaterial(mat);
    Quaternion q1 = new Quaternion().fromAngleAxis(FastMath.DEG_TO_RAD * 45,   new Vector3f(1,0,0));
    boxFigur.setLocalRotation(q1);
    //boxFigur.setLocalTranslation(0, 2, 0);
    //boxFigur.rotate(FastMath.DEG_TO_RAD * 90,0,0);
    pivot.attachChild(boxFigur);

    */
  }  
  
  public float getLinePosOffset(float z)
  {
    if (liste.Count > 0)
    {
      return linePosOffset;
    }
    
    float linePos = linePosOffset;
    
    foreach (Vector3 vec in liste)
    {
      if (z < vec.x)
      {
        return linePos;
      }
      linePos = vec.z;
    }
    
    return linePos;
  }

  public void setLinePosOffset(float linePosOffset)
  {
    this.linePosOffset = linePosOffset;
  }
  
  public void leggTilIListe(Vector3 vec)
  {
    if (vec == null)
    {
      return;
    }
    
    liste.Add(vec);
  }
  
  public void tomListe()
  {
    liste.Clear();
  }
  
  /*
    @Override
  public String xmlSaveString()
  {
    StringBuilder sb = new StringBuilder();
    sb.append("  <ErtmsSeksjonLinePosOffsetXML>\r\n");
    sb.append(xmlDataString());
    sb.append("    <LinePosOffsetXML>" + linePosOffset + "</LinePosOffsetXML>\r\n");
    for (Vector3f vec: liste)
    {
      sb.append("    <LinePosOffsetListeXML>" + vec.x + ", " + vec.y + ", " + vec.z + "</LinePosOffsetListeXML>\r\n");
    }
    sb.append("  </ErtmsSeksjonLinePosOffsetXML>\r\n");
    return sb.toString();
  }
  
  @Override
  public String toString()
  {
    StringBuilder sb = new StringBuilder();
    sb.append("lineposoffset=" + linePosOffset + "\n");
    
    for (Vector3f vec: liste)
    {
      sb.append("liste:" + vec.x + "," + vec.z + "\n");
    }

    return sb.toString();
  }
 */ 
}
