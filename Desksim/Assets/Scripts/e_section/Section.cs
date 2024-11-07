using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Section
{
  private List<TrackNode> allTrackNodes = new List<TrackNode>();
  private List<TrackNode> allEndTrackNodes = new List<TrackNode>();
  private List<TrackNode> allStartTrackNodes = new List<TrackNode>();
  private string name;
  private GenerellTrase trase;
  private List<TrackConnectedObject> tcoList;
  private List<TrackConnectedObject> stateTcoList;
  private List<SwitchPositioner> switchPositionerList = new List<SwitchPositioner>();
  private List<TrackConnectedObject> soundTriggerList = new List<TrackConnectedObject>();
  private ErtmsSeksjonLinePosOffset ertmsSeksjonLinePosOffset;  

  public List<TrackNode> getAllTrackNodes() 
  {
    return allTrackNodes;
  }

  public void addTrackNodes(List<TrackNode> newTrackNodes)
  {
    allTrackNodes.AddRange(newTrackNodes);
  }

  public List<TrackNode> getAllEndTrackNodes()
  {
    return allEndTrackNodes;
  }
  
  public float findTrackLength()
  {
    float l = allEndTrackNodes[0].getPos();
    
    foreach (TrackNode tn in allEndTrackNodes)
    {
      if (tn.getPos() > l)
      {
        l = tn.getPos();
      }
    }
    
    return l;
  }

  public List<TrackNode> getAllStartTrackNodes()
  {
    return allStartTrackNodes;
  }
  
  public string getName()
  {
    return name;
  }

  public void setName(string name)
  {
    this.name = name;
  }

  public GenerellTrase getTrase()
  {
    return trase;
  }

  public void setTrase(GenerellTrase trase)
  {
    this.trase = trase;
  }

  public List<TrackConnectedObject> getTcoList()
  {
    return tcoList;
  }

  public void setTcoList(List<TrackConnectedObject> tcoList)
  {
    this.tcoList = tcoList;
  }

  public List<TrackConnectedObject> getStateTcoList()
  {
    return stateTcoList;
  }

  public void setStateTcoList(List<TrackConnectedObject> stateTcoList)
  {
    this.stateTcoList = stateTcoList;
  }

  public void addStateTCO(TrackConnectedObject tco)
  {
    stateTcoList.Add(tco);
  }
  
  public void addSwitchPositioner(SwitchPositioner sp)
  {
    switchPositionerList.Add(sp);
  }

  public void addSoundTriggers(List<TrackConnectedObject> tcoList)
  {
    soundTriggerList.AddRange(tcoList);
  }
  
  public List<SwitchPositioner> getSwitchPositionerList()
  {
    return switchPositionerList;
  }

  public List<TrackConnectedObject> getSoundTriggerList()
  {
    return soundTriggerList;
  }

  public float getErtmsLinePosOffset(float tcoPos)
  {
    if (ertmsSeksjonLinePosOffset == null)
    {
      return -1;
    }
    
    return ertmsSeksjonLinePosOffset.getLinePosOffset(tcoPos);
  }

  public void setErtmsLinePosOffset(ErtmsSeksjonLinePosOffset ertmsSeksjonLinePosOffset)
  {
    this.ertmsSeksjonLinePosOffset = ertmsSeksjonLinePosOffset;
  }

  public void generateAllStartTrackNodeList()
  {
    List<TrackNode> list = new List<TrackNode>();
    foreach (TrackNode tn in allTrackNodes)
    {
      if (tn.forrige == null)
      {
        list.Add(tn);
      }
    }
    
    allStartTrackNodes.AddRange(list);
  }
  
  public void generateAllEndTrackNodeList()
  {
    List<TrackNode> list = new List<TrackNode>();
    foreach (TrackNode tn in allTrackNodes)
    {
      if (tn.neste == null)
      {
        list.Add(tn);
      }
    }
    
    allEndTrackNodes.AddRange(list);
  }
  
  public void sort() 
  {
    TrackNode[] ta = new TrackNode[allTrackNodes.Count];
    int k = 0;
    foreach (TrackNode tn in allTrackNodes)
    {
      ta[k] = tn;
      k++;
    }
    /*
    Arrays.sort(ta, new TrackNodeComparator());
    
    allTrackNodes.clear();
    
    for (TrackNode tn: ta)
      allTrackNodes.add(tn);*/
  }
  
  public TrackNode findClosestTrackNodeTraseVertex(Vector3 traseVertex)
  {
    TrackNode ctn = allTrackNodes[0];
    float cd = Vector3.Distance(traseVertex, ctn.getTempPoint());
    
    foreach (TrackNode tn in allTrackNodes)
    {
      float d = Vector3.Distance(tn.getTempPoint(), traseVertex);
      if (d < cd)
      {
        cd = d;
        ctn = tn;
      }
    }
    
    return ctn;
  }
 
  public float finnSeksjonOffset()
  {
    foreach (TrackNode tn in getAllEndTrackNodes())
    {
      if (tn.neste != null)
      {
        return tn.forrige.getPos();
      }
    }

    return 0;
  }
  
}
