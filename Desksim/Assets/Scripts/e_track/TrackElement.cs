using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TrackElement
{
  public void init(KmlSplineTrase kmlSplineTrase);
  public List<TrackNode> getStartTrackNodes(); 
  public List<TrackNode> getEndTrackNodes();   
  public List<TrackNode> getAllTrackNodes();
  public Transform getPivot();
}
