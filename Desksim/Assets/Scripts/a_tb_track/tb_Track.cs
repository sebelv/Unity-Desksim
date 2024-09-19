using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tb_Track : MonoBehaviour
{
    [SerializeField] private GameObject geomPrefab;
    protected GameObject geom1;
    protected GameObject b1;
    protected Mesh mesh;
    protected Vector3 sv = new Vector3();
    protected Vector3 ev = new Vector3(0,0,50);
    protected Vector3 v1 = new Vector3();
    protected Vector3 vCenter = new Vector3();
    protected Quaternion q1 = new Quaternion();
    [SerializeField] protected Material mat1;
    [SerializeField] protected Material mat2;
    [SerializeField] protected Material mat3;
    protected Vector3 v2 = new Vector3(-2, 0, 5);
    protected Vector3 v3 = new Vector3(2, 0, 5);
    protected static int MARKED_STATE_UNMARKED = 0;  
    protected static int MARKED_STATE_REDMARKED = 1;
    protected static int MARKED_STATE_ORANGEMARKED = 2;
    protected int markedState = MARKED_STATE_UNMARKED;
    protected Vector3 markedVector;
    protected float trackWidth = 1.435f;
    protected float trackWidthHalf; 
  
    protected float markedAngle = 0;
    protected float koblingsvinkelRad = 0;  


    void Awake()
    {
        trackWidthHalf = trackWidth * 0.5f;
        setStartVector(0,0,-0.1f);
        setEndVector(0,0, 300f);
        createMesh();
    }

  protected void createMesh()
  {    
    mesh = new Mesh();
    
    Vector3[] vertices = {new Vector3(sv.x + trackWidthHalf, 0, sv.z), 
                           new Vector3(sv.x - trackWidthHalf, 0 ,sv.z),
                           new Vector3(ev.x - trackWidthHalf, 0 ,ev.z),
                           new Vector3(ev.x + trackWidthHalf, 0 ,ev.z)};

    int[] indexes = {0,1,3,3,1,2};

    // create normals
    mesh.SetVertices(vertices);
    mesh.SetIndices(indexes, MeshTopology.Triangles, 0);
    mesh.RecalculateBounds();
    mesh.RecalculateNormals();

        // JMonkeyEngine code
    //mesh.setBuffer(VertexBuffer.Type.Position, 3, BufferUtils.createFloatBuffer(vertices));
    //mesh.setBuffer(VertexBuffer.Type.Index,    3, BufferUtils.createIntBuffer(indexes));
    //mesh.updateBound();

    geom1 = GameObject.Instantiate(geomPrefab, transform.root);
    geom1.GetComponent<MeshFilter>().mesh = mesh;
    MeshRenderer meshRenderer = geom1.GetComponent<MeshRenderer>();
    if (markedState == MARKED_STATE_UNMARKED)
      meshRenderer.material = mat1;
    else if (markedState == MARKED_STATE_REDMARKED)
      meshRenderer.material = mat2;
    else if (markedState == MARKED_STATE_ORANGEMARKED)
      meshRenderer.material = mat3;
    
    geom1.transform.parent = transform.root; 
  }
  
  public void detach()
  {
    if (geom1 != null)
      geom1.transform.parent = null; 
  }
  
  public void performChange()
  {
    if (geom1 != null)
      geom1.transform.parent = null;
    
    createMesh();
  }
  
  
  
  public void setStartVector(float x, float y, float z)
  {
    sv.x = x;
    sv.y = y;
    sv.z = z;

  }
  
  public void setEndVector(float x, float y, float z)
  {
    ev.x = x;
    ev.y = y;
    ev.z = z;
//    VertexBuffer vb = mesh.getBuffer(Type.Position);
//    v2.z = z;
//    v3.z = z;
//    BufferUtils.setInBuffer(v2, (FloatBuffer)vb.getData(), 2);
//    BufferUtils.setInBuffer(v3, (FloatBuffer)vb.getData(), 3);
//    
//    vb.setUpdateNeeded();
//    mesh.updateBound();
//    geom1.updateModelBound();
//    
    
    
    //geom1.updateGeometricState();
    
//      Vector3f[] vertices = {new Vector3f(2, 0, 0), 
//                             new Vector3f(-2, 0 ,0),
//                             new Vector3f(-2, 0, z),
//                             new Vector3f(2, 0, z)};
//      
//            mesh.setBuffer(VertexBuffer.Type.Position, 3, BufferUtils.createFloatBuffer(vertices));
//            mesh.updateBound();
//            geom1.updateGeometricState();
//    ev.x = x;
//    ev.y = y;
//    ev.z = z;
//    calcV1();
//    calcCenter();
//    b1.zExtent = v1.z;
//    b1.updateGeometry();
    //geom1.setLocalTranslation(vCenter);
    //geom1.setLocalRotation(q1.fromAngleAxis(findAngleAroundY(), Vector3f.UNIT_Y));
  }
  
//  public void setEndVector_old(float x, float y, float z)
//  {
//    ev.x = x;
//    ev.y = y;
//    ev.z = z;
//    calcV1();
//    calcCenter();
//    b1.zExtent = v1.z;
//    b1.updateGeometry();
//    geom1.setLocalTranslation(vCenter);
//    geom1.setLocalRotation(q1.fromAngleAxis(findAngleAroundY(), Vector3f.UNIT_Y));
//  }

  public Vector3 getSv()
  {
    return sv;
  }

  public Vector3 getEv()
  {
    return ev;
  }
  
  public void mark(Vector3 markPoint)
  {
    geom1.GetComponent<MeshRenderer>().material = mat2;
    markedState = MARKED_STATE_REDMARKED;
    findMarkedVector(markPoint);
  }
  
  public void markOr(Vector3 markPoint)
  {
    geom1.GetComponent<MeshRenderer>().material = mat3;    
    markedState = MARKED_STATE_ORANGEMARKED;
    findMarkedVector(markPoint);
  }
 
  protected void findMarkedVector(Vector3 markPoint)
  {
    // angle
    float mot = ev.x - sv.x;
    float hos = ev.z - sv.z;
    markedAngle = Mathf.Atan(mot/hos);
    
    if (Vector3.Distance(markPoint, sv) < Vector3.Distance(markPoint, ev))
    {
      markedVector = sv;
      markedAngle += (Mathf.Deg2Rad * 180);
    }
    else
    {
      markedVector = ev;
    }
    

    
    print("markedvector: " + markedVector + " marked angle: " + markedAngle);
  }
  
  // kjøres ved koble, da er markedvecor samme objekt som enten sv eller ev
  public void setMarkedVectorPoint(float x, float y, float z)
  {
    markedVector.x = x;
    markedVector.y = y;
    markedVector.z = z;
  }
  
  public void setKoblingsvinkel(float a)
  {
    koblingsvinkelRad = a;
    
    float mot;
    float hos = ev.z - sv.z;
    
    if (markedVector == sv)
    {
      mot = Mathf.Tan(a) * hos;
      ev.x = sv.x + mot;
    }
    else if (markedVector == ev)
    {
      mot = Mathf.Tan(a) * hos;
      sv.x = ev.x - mot;
    }
    
  }
    
  
  public void unMark()
  {
    geom1.GetComponent<MeshRenderer>().material = mat1;    
    markedState = MARKED_STATE_UNMARKED;
    markedVector = new Vector3();
  }

  public Vector3 getMarkedVector()
  {
    return markedVector;
  }

  public float getMarkedAngle()
  {
    return markedAngle;
  }
  
  public bool hasThisGeometry(GameObject g)
  {
    return g == geom1;
  }
  
  private void calcV1()
  {
    v1.x = (ev.x - sv.x) / 2;
    v1.y = (ev.y - sv.y) / 2;
    v1.z = (ev.z - sv.z) / 2;
  }
  
  private void calcCenter()
  {
    vCenter.x = (ev.x + sv.x) / 2.0f;
    vCenter.y = (ev.y + sv.y) / 2.0f;
    vCenter.z = (ev.z + sv.z) / 2.0f;
    
  }
  
  private float findAngleAroundY()
  {
    float v;
    calcV1();

    if (v1.z < 0)
      v = (Mathf.Atan(v1.x / v1.z)) + (180 * Mathf.Deg2Rad);
    else
      v = (Mathf.Atan(v1.x / v1.z));
    
    return v;
  }

  public string xmlSaveString()
  {
    string sb = "";
    
    sb +=("  <StraightTrackXML>\r\n");
    sb +=("    <StartVertexXML>" + sv.x + ", " + sv.y + ", " + sv.z + "</StartVertexXML>\r\n");
    sb +=("    <EndVertexXML>" + ev.x + ", " + ev.y + ", " + ev.z + "</EndVertexXML>\r\n");
    sb +=("  </StraightTrackXML>\r\n");
    sb +=("\r\n");
    
    
    return sb;
  }
 
}
