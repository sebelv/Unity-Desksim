using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MyXmlCreatorTRACK : MyXmlCreatorABSTRACT
{
  public int trackCount = 0;
  private List<TrackElement> trackList = new List<TrackElement>();
  [SerializeField] private List<Vector3> trackStartVertices = new List<Vector3>();
  [SerializeField] private List<Vector3> trackEndVertices = new List<Vector3>(); 

  public List<TrackElement> getTrackList()
  {
    return trackList;
  }
  
public Vector3 getStartVertex(int i)
{
  if(trackStartVertices.Count > i)
  {
  return trackStartVertices[i];
  }
  else
  {
    return new Vector3(-1000, -1000, -1000);
  }
}

public Vector3 getEndVertex(int i)
{
  if(trackEndVertices.Count > i)
  {
  return trackEndVertices[i];
  }
  else
  {
    return new Vector3(-1000, -1000, -1000);
  }
}

  public void c_StraightTrackXML(BaseObjectXML b)
  {
    trackStartVertices.Add(c_vector3f(b.getParameter("StartVertexXML")));
    trackEndVertices.Add(c_vector3f(b.getParameter("EndVertexXML")));
    trackCount++;
  }
  /*
  private void c_SwitchTrackXML(BaseObjectXML b)
  {
    SwitchTrack t = new SwitchTrack(false);
    t.setRadius(c_float(b.getParameter("RadiusXML")));
    t.setId(b.getParameter("IdXML"));
    t.setInfo(b.getParameter("InfoXML"));    
    t.setStartVertex(c_vector3f(b.getParameter("StartVertexXML")));
    t.setRotation(c_float(b.getParameter("RotasjonXML")), new GameObject().transform);
    t.getSwitchPositioner().setLokalStiller(c_boolean(b.getParameter("SwitchLocalButtonXML")));
    t.getSwitchPositioner().setHandOperation(c_boolean(b.getParameter("SwitchHandOperationXML")));    
    t.setDivergingLeft(c_boolean(b.getParameter("AvvikVenstreXML")));
    float sExOffset = c_float(b.getParameter("SwitchEngineXOffsetXML"));   
    
    if (sExOffset != 0)
    {
     t.setSwitchEngineXOffset(sExOffset);
    }
    
    trackList.Add(t);
  }
  
  private void c_CurveTrackXML(BaseObjectXML b)
  {
    CurveTrack t = new CurveTrack(true);
    t.setRadius(c_float(b.getParameter("RadiusXML")));
    t.setId(b.getParameter("IdXML"));
    t.setStartVertex(c_vector3f(b.getParameter("StartVertexXML")));
    t.setRotation(c_float(b.getParameter("RotasjonXML")));
    t.setDivergingLeft(c_boolean(b.getParameter("AvvikVenstreXML")));
    
     trackList.Add(t);
  }
  
  private void c_BezierTrackXML(BaseObjectXML b)
  {
		BezierTrack t = new BezierTrack();
    t.setStartVertex(c_vector3f(b.getParameter("StartVertexXML")));
    t.setStartKontrollVertex(c_vector3f(b.getParameter("StartKontrollVertexXML")));    
    t.setEndKontrollVertex(c_vector3f(b.getParameter("EndKontrollVertexXML")));    
    t.setEndVertex(c_vector3f(b.getParameter("EndVertexXML")));
   
    trackList.Add(t);
  }  
  
  private void c_KryssTrackXML(BaseObjectXML b)
  {
    KryssTrack t = new KryssTrack(false);
    t.setRadius(c_float(b.getParameter("RadiusXML")));
    t.setId(b.getParameter("IdXML"));
    t.setInfo(b.getParameter("InfoXML"));    
    t.setStartVertex(c_vector3f(b.getParameter("StartVertexXML")));
    t.setRotation(c_float(b.getParameter("RotasjonXML")));
    t.getSwitchPositioner().setLokalStiller(c_boolean(b.getParameter("SwitchLocalButtonXML")));
    t.getSwitchPositioner().setHandOperation(c_boolean(b.getParameter("SwitchHandOperationXML"))); 
    t.getSwitchPositioner2().setLokalStiller(c_boolean(b.getParameter("SwitchLocalButtonXML")));
    t.getSwitchPositioner2().setHandOperation(c_boolean(b.getParameter("SwitchHandOperationXML")));     
    t.setDivergingLeft(c_boolean(b.getParameter("AvvikVenstreXML")));
    float sExOffset = c_float(b.getParameter("SwitchEngineXOffsetXML"));   
    
    if (sExOffset != 0)
    {
     t.setSwitchEngineXOffset(sExOffset);
    }
    
    trackList.Add(t);
  }  
  */
  protected override void methodInvoker(BaseObjectXML bo)
  {
    string type = "c_StraightTrackXML";
    if("c_" + bo.getTagName() == type)
    {
        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod("c_" + bo.getTagName());
        object[] obj = new object[1];
        obj[0] = bo;
        Debug.Log(theMethod.Name);
        theMethod.Invoke(this, obj);
    }
  }
}
