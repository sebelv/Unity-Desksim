using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackConnectedObject
{
  protected int tcoIndex = 0;
  public static int TYPE_INDEX_SOUNDTRIGGER_1 = 1001;
  public static int TYPE_INDEX_SOUNDTRIGGER_2 = 1002;
  public static int TYPE_INDEX_OPPKJORT_SPV_DETECT = 1003;  
  public static int TYPE_INDEX_DUMMY = 1004;   
  protected TrackNode trackNode;
  protected Vector3 trackPoint; // this is the pathPoint if allVerticesTransform
  protected Vector3 offsetVector;
  protected Vector3 worldPoint = new Vector3();
  protected string modelFile;
  protected string modelType = "ac3d";
  protected GameObject pivot = new GameObject();
  protected GameObject pivotChangeable = new GameObject();
  protected string id = "";
  protected string info = "";  
  protected int netId = 0;
  protected int direction;
  protected int state = 0;
  protected int state0 = 0;
  protected int state1 = 0;
  protected int state2 = 0;
  protected int typeIndex = 0;
  // 
  protected float opPos;
  protected float opOffset = 0;
  protected int opDirection = 1;
  //
  protected bool noTrackNode = false;
  protected bool allVerticesTransform = false;
  protected bool allVerticesTerrainConnected = false;

  // move
  protected Vector3 worldPointPrev = new Vector3();
  protected Vector3 moveVec = new Vector3();
  protected float anglePrev;
  protected float rotAngle;

//  protected JME3AC3DImporter ac3;

  // update me
  protected bool updateMe = false;
  protected bool stateObject = false;

  // update others list
  protected bool updateOthers = false;

  // rotation
//  private Quaternion newRot = new Quaternion().fromAngleAxis(0, new Vector3(0, 1, 0));
  private float rotY;
  
  // infotext
//  private MyTextureCreator mtc;
  private Font infoTextFont;
  
  // net
  protected bool lastSetStateFromNet = false;
  
  // trackSectionDivision
  protected bool trackBlockDivider = false;
  protected bool deltarIBlokkSystem = false;
  protected bool harNesteBlokk = false;
  protected bool harForrigeBlokk = false;
//  protected BlokkNy blokkForrige;
//  protected BlokkNy blokkNeste;
  
  // sporslutter
  protected bool sporSlutter = false;  
  
  // advanced signalsystem
  protected bool involvedInAdvancedSignalControl = false;    
  
  // ny ts
//  protected InfraObjektLokal infraObjektLokal;
  protected bool blokkTeller = false;

  public TrackConnectedObject()
  {
    initTrackNode();
  }

  public virtual void initAll()
  {
    create3D();

    // ved scenariebygging
    /*
    
    if (StartParameters.showObjectData)
    {
      initInfoText(assetManager);
    }

    */
  }

  public void detachChildrenAndRebuild()
  {    
    pivot.transform.DetachChildren();
    create3D();
  }

  protected void create3D()
  {
    if (modelFile == null || modelFile == "" || modelFile == "null" || modelFile == "no content")
    {
      return;
    }

    if (modelFile.Contains(".j3o"))
    {
      /*GameObject spatial = assetManager.loadModel("Models/Blender/" + modelFile);
      if (info.equals("phong"))
      {
        System.out.println("pdrTilPhong: " + modelFile);
        BlenderOperasjoner.pbrTilPhongSpatial(assetManager, spatial);
      }
      allVerticesTransform = false;
      pivot.attachChild(spatial);
      */
    }
    else
    {
//      ac3 = new JME3AC3DImporter("assets/Models/" + modelFile);
      /*ac3 = new JME3AC3DImporter("materiale/models/" + modelFile);      
      pivot.attachChild(ac3.buildObject(assetManager));*/
    }

  }

  public void moveToWorldPosition(Section section)
  {
    if (!allVerticesTransform) // if singlePoint default
    {
      //System.out.println("trac: " + trackPoint + " " + getClass());
      //System.out.println("this: " + trackPoint);
      moveUsingTrackPointSinglePointTransform(trackPoint, section.getAllTrackNodes());
    } else // allVerticesTransform
    {
      // denne bruker alltid x=0 som utgangs x posisjon
      // så brukes x verdien i offsetVertex til x posisjonering
      trackPoint.x = offsetVector.x;
      moveUsingPathPointAllVerticesTransform(trackPoint /*used as pathPoint*/,
              section.getTrase());
    }
  }

  public void moveUsingTrackPointSinglePointTransform(
          Vector3 trackPoint, List<TrackNode> allTrackNodes)
  {
    // fjern evt tidligere forbindelser 
    if (!noTrackNode)
    {
      if (trackNode.neste != null && trackNode.forrige != null)
      {
        trackNode.forrige.neste = trackNode.neste;
        trackNode.neste.forrige = trackNode.forrige;
        trackNode.neste = null;
        trackNode.forrige = null;
      }
    }

    //System.out.println("trackpoint: " + trackPoint);
    // finn punkt i "verden"
    AnglesVectors.findWorldPointAlongTrack(trackPoint, offsetVector, worldPoint,
            allTrackNodes);

    rotAngle = AnglesVectors.getLatestCalculatedAngle() - anglePrev;
    worldPoint -= worldPointPrev - moveVec;

    rotY = AnglesVectors.getLatestCalculatedAngle();

    if (direction == -1)
    {
      rotY += 180 * Mathf.Deg2Rad;
//       pivot.rotate(0, 180 * FastMath.DEG_TO_RAD, 0);
//       pivotChangeable.rotate(0, 180 * FastMath.DEG_TO_RAD, 0);
    }
    /*
    newRot.fromAngles(0, rotY, 0);

    pivot.setLocalRotation(newRot);
    pivotChangeable.setLocalRotation(newRot);
    //pivot.rotate(0, rotAngle, 0);
    //pivotChangeable.rotate(0, rotAngle, 0);

    pivot.move(moveVec);
    pivotChangeable.move(moveVec);

    anglePrev = AnglesVectors.getLatestCalculatedAngle();
    worldPoint.add(Vector3.ZERO, worldPointPrev);
    */
    if (!noTrackNode)
    {
      AnglesVectors.connectTrackNodeUsingLatestFoundInsertTrackNodes(trackNode);
    }
  }

  public void moveUsingPathPointAllVerticesTransform(
          Vector3 pathPoint, GenerellTrase trase)
  {
    /*
    ac3.transformAllVerticesToWorld((KmlSplineTrase) trase, pathPoint);
    ac3.rebuildObject(assetManager);*/
  }
  
  /*
  public void applyTerrainHeightAllVerticesTransform(TerrainHeightFinder thf)
  {
    ac3.applyTerrainHeightToAllShapeVertices(thf);
    ac3.rebuildObject(assetManager);
  }*/

  //
  public void oppdaterOpOffset(float opOffset)
  {
    this.opOffset = opOffset;
    opPos = (trackNode.getPos() + opOffset) * opDirection;
  }

  public void oppdaterOpDirection(int opRetn)
  {
    this.opDirection = opRetn;
    opPos = (trackNode.getPos() + opOffset) * opRetn;
  }
  //

  public void detach()
  {
    if (trackNode.neste != null && trackNode.forrige != null)
    {
      trackNode.forrige.neste = trackNode.neste;
      trackNode.neste.forrige = trackNode.forrige;
    }
  }
  
  public void setState(int value)
  {
    state0 = value;
    state = value;

    updateVisuals();

    if (updateOthers && !lastSetStateFromNet)
    {
      performUpdateOthers();
    }
    
    // ny ts
    /*
    if (StartParameters.traffikkstyring_ny)
    {
      nyTsRapporterStatus();
    } 
    */   
  }
  
  // må overrides dersom status ikke skal rapporteres direkte som følge av setstate - rapporttering skjer da fra annen metode og denne gjør ingenting
  protected void nyTsRapporterStatus()
  {
    /*
    if (infraObjektLokal != null)
    {
      infraObjektLokal.setStatusRapportert(state);
    }*/
  }

  public int getState()
  {
    return state;
  }

  public void setState(int index, int value)
  {
    if (index == 0)
    {
      state0 = value;
      state = value;
    } else if (index == 1)
    {
      state1 = value;
    } else
    {
      state2 = value;
    }

    if (updateOthers)
    {
      performUpdateOthers();
    }
  }

  public void passAlert(TrackConnectedObject t)
  {
    //t.passAlert(this);
  }

  public bool erPassert(float posTog)
  {
    if (posTog > opPos)
    {
      return true;
    }

    return false;
  }

  public TrackNode getTrackNode()
  {
    return trackNode;
  }

  public string getId()
  {
    return id;
  }

  public void setId(string id)
  {
    this.id = id;
  }

  public string getInfo()
  {
    return info;
  }

  public void setInfo(string info)
  {
    this.info = info;
  }  
  
  public float getOperationPos()
  {
    return opPos;
  }

  public float getPos()
  {
    return trackNode.getPos();
  }

  public int getDirection()
  {
    return direction;
  }

  public void setDirection(int direction)
  {
    this.direction = direction;
  }

  public int getNetId()
  {
    return netId;
  }

  public void setNetId(int netId)
  {
    this.netId = netId;
  }

  public bool isNoTrackNode()
  {
    return noTrackNode;
  }

  public void setNoTrackNode(bool noTrackNode)
  {
    this.noTrackNode = noTrackNode;
  }

  public bool isAllVerticesTransform()
  {
    return allVerticesTransform;
  }

  public void setAllVerticesTransform(bool allVerticesTransform)
  {
    this.allVerticesTransform = allVerticesTransform;
  }

  public bool isAllVerticesTerrainConnected()
  {
    return allVerticesTerrainConnected;
  }

  public void setAllVerticesTerrainConnected(bool allVerticesTerrainConnected)
  {
    this.allVerticesTerrainConnected = allVerticesTerrainConnected;
  }

  public void axle(int retn)
  {
    // to be overridden
  }

  private void initTrackNode()
  {
    trackNode = new TrackNode(this, new Vector3());
  }

  public void setOffsetPoint(Vector3 vec)
  {
    offsetVector = vec;
  }

  public void setTrackNode(TrackNode trackNode)
  {
    this.trackNode = trackNode;
  }

  public Vector3 getTrackPoint()
  {
    return trackPoint;
  }

  public Vector3 getOffsetVector()
  {
    return offsetVector;
  }

  public void setTrackPoint(Vector3 trackPoint)
  {
    this.trackPoint = trackPoint;
  }

  public string getModelFile()
  {
    return modelFile;
  }

  public void setModelFile(string modelFile)
  {
    this.modelFile = modelFile;
  }

  public GameObject getPivot()
  {
    return pivot;
  }

  public GameObject getPivotChangeable()
  {
    return pivotChangeable;
  }

  public int getTypeIndex()
  {
    return typeIndex;
  }

  public void setTypeIndex(int typeIndex)
  {
    this.typeIndex = typeIndex;
  }

  public void toggleState()
  {

  }

  protected void updateVisuals()
  {
    // to be overridden
  }

  public bool isUpdateMe()
  {
    return updateMe;
  }

  public void setUpdateMe(bool updateMe)
  {
    this.updateMe = updateMe;
  }

  public bool isUpdateOthers()
  {
    return updateOthers;
  }

  public void setUpdateOthers(bool updateOthers)
  {
    this.updateOthers = updateOthers;
  }

  public bool isStateObject()
  {
    return stateObject;
  }

  public void setStateObject(bool stateObject)
  {
    this.stateObject = stateObject;
  }

  public void createUpdateOthersList()
  {
    // to be overridden
  }

  public void controllingObjectChanged()
  {
    // to be overridden
  }

  public void performUpdateOthers()
  {
    searchForAndUpdateDependentObjects();
  }

  public void searchForAndUpdateDependentObjects()
  {
    TrackNode runner = trackNode;
    // don't add itself
    if (direction == 1)
    {
      runner = runner.forrige;
    } else
    {
      runner = runner.neste;
    }
    //

    int tnCount = 0;
    int tnMax = 2000 / 5;

    while (runner != null && tnCount < tnMax)
    {
      if (runner.getTrackSideObject() != null && runner.getTrackSideObject().getDirection() == direction)
      {
        performUpdateOfDependentObject(runner.getTrackSideObject());
      }

      if (direction == 1)
      {
        runner = runner.forrige;
      } else
      {
        runner = runner.neste;
      }

      tnCount++;
    }
  }

  protected void performUpdateOfDependentObject(TrackConnectedObject tco)
  {
    // to be overridden
  }
    /*
  private void addInfoText()
  {
    mtc.addText(/*getModelFile()getClass().getSimpleName() + "", infoTextFont, Color.orange, 1, 20);
    mtc.addText(getPos() + "", infoTextFont, Color.orange, 1, 50);
  }
  
  private void initInfoText()
  {
    Box box = new Box(0.5f, 0.5f, 0.01f);
    Geometry textureTextGeometry = new Geometry("Box", box);
    Material mat = new Material(assetManager, "Common/MatDefs/Misc/Unshaded.j3md");

    //
    infoTextFont = new Font("Verdana", Font.BOLD, 14);
    mtc = new MyTextureCreator(64, 64, Color.gray);

    addInfoText();

    Texture2D tex2 = mtc.createTexture2D();
    mat.setTexture("ColorMap", tex2);

    mat.getAdditionalRenderState().setBlendMode(RenderState.BlendMode.Alpha);
    textureTextGeometry.setQueueBucket(RenderQueue.Bucket.Transparent);
    textureTextGeometry.setMaterial(mat);
    textureTextGeometry.move(-1.5f, 1f, 0);
    pivot.attachChild(textureTextGeometry);
  }
  
  public void updateInfoText2D()
  {
    if (!StartParameters.showObjectData)
    {
      return;
    }

    if (mtc == null)
    {
      return;
    }

    mtc.clearImage();
    addInfoText();
    mtc.updateTexture();
  }

    */
  public int getTcoIndex()
  {
    return tcoIndex;
  }

  public void setTcoIndex(int tcoIndex)
  {
    this.tcoIndex = tcoIndex;
  }  

  public bool isSporSlutter()
  {
    return sporSlutter;
  } 

  public bool isTrackSectionDivider()
  {
    return trackBlockDivider;
  }
  
  public void setAsNotTrackBlockDivider()
  {
    trackBlockDivider = false;
  } 

  public bool isBlokkTeller()
  {
    return blokkTeller;
  }

  public void setBlokkTeller(bool blokkTeller)
  {
    this.blokkTeller = blokkTeller;
  }
  
  public void blokkTellerPluss()
  {
    // to be overridden
  }
  
  public void blokkTellerMinus()
  {
    // to be overridden
  }  

  public bool isDeltarIBlokkSystem()
  {
    return deltarIBlokkSystem;
  }

    /*
  public void setDeltarINyttBlokkSystem(bool deltarIBlokkSystem)
  {
    this.deltarIBlokkSystem = deltarIBlokkSystem;
    if (deltarIBlokkSystem)
    {
      Program.nyttBlokkSystemAktivert = true;
    }
  }  

  public BlokkNy getBlokkForrige()
  {
    return blokkForrige;
  }

  public void setBlokkForrige(BlokkNy blokkForrige)
  {
    this.blokkForrige = blokkForrige;
    harForrigeBlokk = (blokkForrige != null);
  }

  public BlokkNy getBlokkNeste()
  {
    return blokkNeste;
  }

  public void setBlokkNeste(BlokkNy blokkNeste)
  {
    this.blokkNeste = blokkNeste;
    harNesteBlokk = (blokkNeste != null);
  }
  
  public void passerBlokkskille(bool posHast)
  {
    if (posHast)
    {
      if (harNesteBlokk)
      {
        blokkNeste.pluss();
      }
      if (harForrigeBlokk)
      {
        blokkForrige.minus();
      }
    }
    else
    {
      if (harNesteBlokk)
      {
        blokkNeste.minus();
      }
      if (harForrigeBlokk)
      {
        blokkForrige.pluss();
      }      
    }
  }
  
  public void passerTellepunktBlokkTeller(bool medKm)
  {
    if (infraObjektLokal != null)
    {
      if (medKm)
      {
        infraObjektLokal.nesteTellBlokkTeller();
      }
      else
      {
        infraObjektLokal.forrigeTellBlokkTeller();    
      }      
    }

  }  

  public bool isInvolvedInAdvancedSignalControl()
  {
    return involvedInAdvancedSignalControl;
  }

  public void setInvolvedInAdvancedSignalControl()
  {
    this.involvedInAdvancedSignalControl = true;
  }

  public bool isLastSetStateFromNet()
  {
    return lastSetStateFromNet;
  }  
  
  protected string findConnectedIdObjectString(string s)
  {
    int i1 = s.indexOf("'");
    string sSub = s.substring(i1+1);
    int i2 = sSub.indexOf("'");
    string sSub2 = sSub.substring(0, i2);
    
    return sSub2;
  }

  public InfraObjektLokal getInfraObjekt()
  {
    return infraObjektLokal;
  }

  public void setInfraObjekt(InfraObjektLokal iol)
  {
    this.infraObjektLokal = iol;
  }
  */
  
  protected string xmlDataString()
  {
    if (offsetVector == null)
    {
      offsetVector = new Vector3();
    }
    
    //System.out.println("class: " + getClass().getName());
    string sb = "";
    sb +=("    <IdXML>" + id + "</IdXML>\r\n");
    sb +=("    <InfoXML>" + info + "</InfoXML>\r\n");
    sb +=("    <StartVertexXML>" + trackPoint.x + ", " + trackPoint.y + ", " + trackPoint.z + "</StartVertexXML>\r\n");
    sb +=("    <OffsetVertexXML>" + offsetVector.x + ", " + offsetVector.y + ", " + offsetVector.z + "</OffsetVertexXML>\r\n");
    sb +=("    <DirectionXML>" + direction + "</DirectionXML>\r\n");
    sb +=("    <FileNameXML>" + modelFile + "</FileNameXML>\r\n");
    if  (allVerticesTransform)
    {
      sb+=("    <AllVerticesTransformXML>" + allVerticesTransform + "</AllVerticesTransformXML>\r\n");
    } 
    if (allVerticesTerrainConnected)
    {
      sb+=("    <AllVerticesTerrainXML>" + allVerticesTransform + "</AllVerticesTerrainXML>\r\n");
    } 

    return sb;
  }

  public string xmlSaveString()
  {
    string sb = "";
    sb +=("  <TrackConnectedObjectXML>\r\n");
    sb +=(xmlDataString());
    sb +=("  </TrackConnectedObjectXML>\r\n");
    return sb;
  }

  
  public void tcoChangeLoop()
  {
    // to be overriddem
  } 

}
