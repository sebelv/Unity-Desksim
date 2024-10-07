using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TrackPart : MonoBehaviour
{
    private Vector3[] vertexArray;
    private Vector2[] texArray;
    private int[] indexArray;
    private Vector3[] normalArray;
    [SerializeField] private Material matBase;


    private static float Y_SKINNE_TOPP = 0.45f;
    private static float X_SKINNE_TOPP = 0.04f;
    private static float Y_SKINNE_LOWER_SIDE_TOP = 0.38f;
    
    public static float Y_GRAVEL_TOP_NEW_LIGHT = 0.25f;
    public static float Y_GRAVEL_TOP_NEW_TERRAIN = 0.1f;
    public static float Y_GRAVEL_TOP = 0.05f;
    
    private static float X_SKINNE_LOWER_SIDE = 0.02f;
    
    // top
    public static Vector3[]to_top = {new Vector3(-0.04f,0.2f,0.0f), 
                                        new Vector3(0.04f,0.2f,0.0f)};
    
    public static Vector3[]to_top_new_light = {new Vector3(-X_SKINNE_TOPP,Y_SKINNE_TOPP ,0.0f), 
                                        new Vector3(X_SKINNE_TOPP,Y_SKINNE_TOPP,0.0f)};
    
    public static Vector2[]txo_top = {new Vector2(0,0), 
                                        new Vector2(1,0)};
    
    public static int[] indexSequenze_top = {0,3,1,0,2,3};
    
    public static float texFac_top = 1f;
  
  // upper sides
    public static Vector3[]to_sides = {new Vector3(-0.04f,0.15f,0.0f), 
                                      new Vector3(-0.04f,0.2f,0.0f),
                                      new Vector3(0.04f,0.15f,0.0f),
                                      new Vector3(0.04f,0.2f,0.0f)};
  
  public static Vector3[]to_sides_new_light = {new Vector3(-X_SKINNE_TOPP,Y_SKINNE_LOWER_SIDE_TOP,0.0f), 
                                      new Vector3(-X_SKINNE_TOPP,Y_SKINNE_TOPP,0.0f),
                                      new Vector3(X_SKINNE_TOPP,Y_SKINNE_LOWER_SIDE_TOP,0.0f),
                                      new Vector3(X_SKINNE_TOPP,Y_SKINNE_TOPP,0.0f)};
  
  public static Vector2[]txo_sides = {new Vector2(0,0), 
                          new Vector2(1,0),
                          new Vector2(0,0),
                          new Vector2(1,0)};
  
  public static int[] indexSequenze_sides = {0,4,5,0,5,1,6,2,3,6,3,7};
  
  public static float texFac_sides = 1f;
  
  // lower sides
    public static Vector3[]to_foot_sides = {new Vector3(-0.02f,0.02f,0.0f), 
                                      new Vector3(-0.02f,0.15f,0.0f),
                                      new Vector3(0.02f,0.02f,0.0f),
                                      new Vector3(0.02f,0.15f,0.0f)};
  
  public static Vector3[]to_foot_sides_new_light = {new Vector3(-X_SKINNE_LOWER_SIDE,Y_GRAVEL_TOP_NEW_LIGHT,0.0f), 
                                      new Vector3(-X_SKINNE_LOWER_SIDE,Y_SKINNE_LOWER_SIDE_TOP,0.0f),
                                      new Vector3(X_SKINNE_LOWER_SIDE,Y_GRAVEL_TOP_NEW_LIGHT,0.0f),
                                      new Vector3(X_SKINNE_LOWER_SIDE,Y_SKINNE_LOWER_SIDE_TOP,0.0f)};
  
  public static Vector2[]txo_foot_sides = {new Vector2(0,0), 
                          new Vector2(1,0),
                          new Vector2(0,0),
                          new Vector2(1,0)};
  
  public static int[] indexSequenze_foot_sides = {0,4,5,0,5,1,6,2,3,6,3,7};
  
  public void SetMaterialBase(Material material)
  {
    matBase = material;
  }

  public static float texFac_foot_sides = 1f;
  
  // gravel
//  public static Vector3f[]to_gravel_old = {new Vector3f(-1.5f,0.02f,0.0f), 
//                                    new Vector3f(1.5f,0.02f,0.0f)};
  
    public static Vector3[]to_gravel = {new Vector3(-1.5f,0.02f,0.0f), 
                                         new Vector3(-0.75f,0.05f,0.0f),
                                         new Vector3(0.75f,0.05f,0.0f),
                                         new Vector3(1.5f,0.02f,0.0f)};
    
//        public static Vector3f[]to_gravel_new = {new Vector3f(-2.5f,0.01f,0.0f), 
//                                         new Vector3f(-X_GRAVEL_INNER,Y_GRAVEL_TOP,0.0f),
//                                         new Vector3f(X_GRAVEL_INNER,Y_GRAVEL_TOP,0.0f),
//                                         new Vector3f(2.5f,0.01f,0.0f)};
    
    public static Vector3[]to_gravel_new_terrain = {new Vector3(-2.5f,0.01f,0.0f), 
                                         new Vector3(-0.75f,0.1f,0.0f),
                                         new Vector3(0.75f,0.1f,0.0f),
                                         new Vector3(2.5f,0.01f,0.0f)};
        
    public static Vector3[]to_gravel_new_light = {new Vector3(-2.5f,0.01f,0.0f), 
                                         new Vector3(-1.5f,0.23f,0.0f),
                                         new Vector3(-0.75f,Y_GRAVEL_TOP_NEW_LIGHT,0.0f),
                                         new Vector3(0.75f,Y_GRAVEL_TOP_NEW_LIGHT,0.0f),
                                         new Vector3(1.5f,0.23f,0.0f),
                                         new Vector3(2.5f,0.01f,0.0f)};        
  
//  public static Vector2f[]txo_gravel_old = {new Vector2f(0,0), 
//                                     new Vector2f(1,0)};
  
    public static Vector2[]txo_gravel = {new Vector2(0,0), 
                                          new Vector2(0.25f,0),
                                          new Vector2(0.75f,0),
                                          new Vector2(1,0)};
//      public static Vector2f[]txo_gravel_new = {new Vector2f(0,0), 
//                                          new Vector2f(0.2f,0), // 0.31f
//                                          new Vector2f(0.8f,0), // 0.69f
//                                          new Vector2f(1,0)};
    
    public static Vector2[]txo_gravel_new_terrain = {new Vector2(0,0), 
                                          new Vector2(0.31f,0),
                                          new Vector2(0.69f,0),
                                          new Vector2(1,0)};
      
      public static Vector2[]txo_gravel_new_light = {new Vector2(0,0), 
                                          new Vector2(0.15f,0), // 0.31f
                                          new Vector2(0.31f,0),
                                          new Vector2(0.69f,0),
                                          new Vector2(0.85f,0), // 0.69f
                                          new Vector2(1,0)};      
      

    
  public static string gravel_file = "Textures/sand_sviller.png"; 
    
//  public static int[] indexSequenze_gravel = {0,3,1,0,2,3};
  public static int[] indexSequenze_gravel = {0,5,1,0,4,5, 1,6,2,1,5,6, 2,7,3,2,6,7};
  public static int[] indexSequenze_gravel_new_lighting = {0,7,1,0,6,7, 1,8,2,1,7,8, 2,9,3,2,8,9, 3,10,4,3,9,10, 4,11,5,4,10,11};
  
  public static float texFac_gravel = 3f;
  
  private Vector3[] to;
  private Vector2[] txo;
  private int[] indexSequenze;
  private float zMax;
  private int materialChoice = 0;
  private float texFac = 1f;
  private float length;
  private Vector3 startVertex, endVertex;
  
  public TrackPart(Vector3 startVertex, Vector3 endVertex, int materialChoice, Vector3[] trackOutline, 
    Vector2[] texOutline, int[] indexSeq, float texFac, Material material)
  {
    matBase = material;
    length = Vector3.Distance(startVertex, endVertex);
    zMax = length;
    to = trackOutline;
    txo = texOutline;
    indexSequenze = indexSeq;
    this.materialChoice = materialChoice;
    this.texFac = texFac;
    this.startVertex = startVertex;
    this.endVertex = endVertex;
    
    // initStaticTrackValues();

    createVertexes();
  }
  
  /*
  public static void initStaticTrackValues()
  {
    // new terrain
    if (Program.newTerrain)
    {
      to_gravel = to_gravel_new_terrain;
      txo_gravel = txo_gravel_new_terrain;
      gravel_file = "Textures/sand_sviller_3.png";
    }
    // new lighting
    if (StartParameters.newLighting)
    {
      to_top = to_top_new_light;
      to_sides = to_sides_new_light;
      to_foot_sides = to_foot_sides_new_light;
      to_gravel = to_gravel_new_light;
      txo_gravel = txo_gravel_new_light;
      indexSequenze_gravel = indexSequenze_gravel_new_lighting;
      gravel_file = "Textures/sand_sviller_3.png";
    }
  } */

  public Vector3[] getVertexArray()
  {
    return vertexArray;
  }
  
  public int[] getIndexArray()
  {
    return indexArray;
  }
  
  private void createVertexes()
  {
    vertexMaker();
    vertexIndexMaker();
  }
  
  public void tilStartVertex()
  {
    for(int i = 0; i < vertexArray.Length; i++)
    {
      vertexArray[i].Set(vertexArray[i].x+startVertex.x, vertexArray[i].y+startVertex.y, vertexArray[i].z+startVertex.z);    
    }
  }
  
  public void tilVertex(float x, float y, float z)
  {
    for(int i = 0; i < vertexArray.Length; i++)
    {
        vertexArray[i].Set(vertexArray[i].x+x, vertexArray[i].y+y, vertexArray[i].z+z);
    }    
  }
  
  public void localPositioning(float x, float y, float z)
  {
    // flytt til posisjon internt i skinneobjektet
    if(x != 0 || y != 0 || z != 0)
    {
      for(int i = 0; i < vertexArray.Length; i++)
      {
        //print(v + " - Before");
        vertexArray[i].Set(vertexArray[i].x+x, vertexArray[i].y+y, vertexArray[i].z+z);
        //print(v + " - After");
      }
    }
    
    // rotasjon
    float vi1 = AnglesVectors.finnVinkelOmY(startVertex, endVertex);
    Debug.Log(vi1 + " - Angles");

    Quaternion rotY = AnglesVectors.fromAngleAxis(new Quaternion(), vi1,   new Vector3(0,1,0));
    Transform t1 = new GameObject().transform;
    t1.gameObject.name = "Rotate Object";
    t1.rotation = rotY;
    
    for(int i = 0; i < vertexArray.Length; i++)
      t1.position = vertexArray[i];
    
    // flytt til posisjon langs traseèn 
    for(int i = 0; i < vertexArray.Length; i++)
    {
      vertexArray[i].Set(vertexArray[i].x+startVertex.x, vertexArray[i].y+startVertex.y, vertexArray[i].z+startVertex.z);
    }
   
    // System.out.println("startv: " + startVertex);
    GameObject.Destroy(t1.gameObject);
  }
  
  public void worldPositioning(KmlSplineTrase kmlSplineTrase)
  {
    for (int i = 0; i < vertexArray.Length; i++)
    {
      //print(vertexArray[i] + " - Before asd sad asd ");
      vertexArray[i] = kmlSplineTrase.finnVertexITraseVertex(vertexArray[i]);
      //print(vertexArray[i] + " - Results asd asd asd ");
    }
  }
  
  public void create3D()
  {
    
  }
  
  public GameObject meshMaker()
  {
    Mesh mesh = new Mesh();
    mesh.vertices = vertexArray;
    mesh.triangles = indexArray;
      
    GameObject geo = new GameObject("Track");
    MeshFilter filter = geo.AddComponent<MeshFilter>();
    MeshRenderer renderer = geo.AddComponent<MeshRenderer>();
    filter.mesh = mesh; // using our custom mesh object

//    Material mat = new Material(assetManager, 
//           "Common/MatDefs/Light/Lighting.j3md");
    Material mat = materialGravelTextureColor();
    
    if (materialChoice == 0)
    {
      mat = materialTopTextureColor();
    }
    else if (materialChoice == 1)
    {

    }
    else if (materialChoice == 2)
    {
      mat = materialTopMaterialLight();
    }
    else if (materialChoice == 3)
    {
      mat = materialTopTextureColor2();
    }
    else if (materialChoice == 4)
    {
      mat = materialSideTextureColor();
    }
    else if (materialChoice == 5)
    {
      mat = materialFootSideTextureColor();
    }
    else if (materialChoice == 6)
    {
      mat = materialGravelTextureColor();
    }

    
//    mat.setTexture("DiffuseMap", 
//            assetManager.loadTexture("Textures/Terrain/BrickWall/BrickWall.jpg"));
    renderer.material = mat;
    
    return geo;
  }
  
  
    private void createNormals_new(Vector3[] normals, Vector3[] vertices, int[] indexes)
  {
    Vector3[] accumulatedNormalValues = new Vector3[vertices.Length];

    // new
    for (int i = 0; i < normals.Length; i++)
    {
      normals[i] = new Vector3(0, 0, 0);
      accumulatedNormalValues[i] = new Vector3(0, 0, 0);
    }

    //
    for (int i = 0; i < indexes.Length; i += 3)
    {
      Vector3 v0 = vertices[indexes[i]];
      Vector3 v1 = vertices[indexes[i + 1]];
      Vector3 v2 = vertices[indexes[i + 2]];

      Vector3 edge0 = v1 - v0;
      Vector3 edge1 = v2 - v1;

      Vector3 norm = Vector3.Cross(edge0,edge1);

      // new
      accumulatedNormalValues[indexes[i]].x += norm.x;
      accumulatedNormalValues[indexes[i]].y += norm.y;
      accumulatedNormalValues[indexes[i]].z += norm.z;

      accumulatedNormalValues[indexes[i + 1]].x += norm.x;
      accumulatedNormalValues[indexes[i + 1]].y += norm.y;
      accumulatedNormalValues[indexes[i + 1]].z += norm.z;

      accumulatedNormalValues[indexes[i + 2]].x += norm.x;
      accumulatedNormalValues[indexes[i + 2]].y += norm.y;
      accumulatedNormalValues[indexes[i + 2]].z += norm.z;
    }

    int k = 0;
    foreach (Vector3 v in accumulatedNormalValues)
    {
      v.Normalize();
      normals[k].x = v.x;
      normals[k].y = v.y;
      normals[k].z = v.z;
      k++;
    }
  }
  
  private void initTrack()
  {
    vertexMaker();
    vertexIndexMaker();
  }
  
  private void vertexMaker_old()
  {
    List<Vector3> vl = new List<Vector3>();
    List<Vector2> tl = new List<Vector2>();
    
    int zDist = 5;
    for (float i = 0; i < zMax; i += zDist)
    {
      for (int k = 0; k < to.Length; k++)
      {
        Vector3 p = to[k];
        p.z = i;
        vl.Add(p);
        
        Vector2 t = txo[k];
        t.y = (i / zDist) * texFac;//i/5;
        tl.Add(t);
             
      }
       
    }
    
    vertexArray = new Vector3[vl.Count];
    for (int i = 0; i < vertexArray.Length; i++)
      vertexArray[i] = vl[i];
    
    texArray = new Vector2[tl.Count];
    for (int i = 0; i < texArray.Length; i++)
      texArray[i] = tl[i];

  }
  
  private void vertexMaker()
  {
    List<Vector3> vl = new List<Vector3>();
    List<Vector2> tl = new List<Vector2>();
    
    int zDist = 5;
    float f = 0;
    bool cont = true;
    bool last = false;
    
    while (cont)
    {
      for (int k = 0; k < to.Length; k++)
      {
        Vector3 p = to[k];
        p.z = f;
        vl.Add(p);
        
        Vector2 t = txo[k];
        t.y = (f / zDist) * texFac;//i/5;
        tl.Add(t);
             
      }
      
      // håndter siste
      if (last)
      {
        cont = false;
      }
       
      f += zDist;
      
      if (f >= zMax)
      {
        f = zMax;
        last = true;
      }
    }
    
    vertexArray = new Vector3[vl.Count];
    for (int i = 0; i < vertexArray.Length; i++)
      vertexArray[i] = vl[i];
    
    texArray = new Vector2[tl.Count];
    for (int i = 0; i < texArray.Length; i++)
      texArray[i] = tl[i];

  }
  
  private void vertexIndexMaker()
  {
    List<int> il = new List<int>();
    
    for (int i = 0; i < (vertexArray.Length - to.Length); i += to.Length)
    {
      for (int k = 0; k < indexSequenze.Length; k++)
      {
        il.Add(indexSequenze[k] + i);
      }
    }
    
    indexArray = new int[il.Count];
    for (int i = 0; i < indexArray.Length; i++)
      indexArray[i] = il[i];
  }

  private Material materialTopTextureColor()
  {
        return MaterialManager.Instance.GetMaterial("TrackTop");
  }
  
  private Material materialTopTextureColor2()
  {
        return MaterialManager.Instance.GetMaterial("TrackTop");
     /*Material mat = new Material(assetManager, 
        "Common/MatDefs/Misc/Unshaded.j3md");
     
      Texture tex =  assetManager.loadTexture("Textures/track_top.png");
      tex.setWrap(Texture.WrapMode.Repeat);
     
      mat.setTexture("ColorMap", tex);
      
      return mat;*/
  }
  
  private Material materialSideTextureColor()
  {
        return MaterialManager.Instance.GetMaterial("Track");
    /*if (StartParameters.newLighting)
    {
      Material mat = new Material(assetManager, 
        "Common/MatDefs/Light/Lighting.j3md");
      
      Texture tex = assetManager.loadTexture("Textures/track_side.png");
      tex.setWrap(Texture.WrapMode.Repeat);
      mat.setBoolean("UseMaterialColors", true);
      mat.setColor("Diffuse", ColorRGBA.White);
      mat.setColor("Ambient", ColorRGBA.White);
      mat.setTexture("DiffuseMap", tex);
      
      return mat;
    }
    
    Material mat = new Material(assetManager, 
        "Common/MatDefs/Misc/Unshaded.j3md");
     
    Texture tex =  assetManager.loadTexture("Textures/track_side.png");
    tex.setWrap(Texture.WrapMode.Repeat);

    mat.setTexture("ColorMap", tex);

    return mat;*/
  }

  private Material materialTopMaterialLight()
  {
    return MaterialManager.Instance.GetMaterial("TopMaterialLight");
  }
  
  private Material materialFootSideTextureColor()
  {
            return MaterialManager.Instance.GetMaterial("Track");
    /*if (StartParameters.newLighting)
    {
      Material mat = new Material(assetManager, 
        "Common/MatDefs/Light/Lighting.j3md");
      
      Texture tex = assetManager.loadTexture("Textures/track_foot_side.png");
      tex.setWrap(Texture.WrapMode.Repeat);
      mat.setBoolean("UseMaterialColors", true);
      mat.setColor("Diffuse", ColorRGBA.White);
      mat.setColor("Ambient", ColorRGBA.White);

      mat.setTexture("DiffuseMap", tex);
      return mat;
    }
    
    Material mat = new Material(assetManager, 
        "Common/MatDefs/Misc/Unshaded.j3md");
     
    Texture tex =  assetManager.loadTexture("Textures/track_foot_side.png");
    tex.setWrap(Texture.WrapMode.Repeat);

    mat.setTexture("ColorMap", tex);

    return mat;*/
  }
  
  private Material materialGravelTextureColor()
  {
            return MaterialManager.Instance.GetMaterial("Gravel");
    /*if (StartParameters.newLighting)
    {
      Material mat = new Material(assetManager, 
        "Common/MatDefs/Light/Lighting.j3md");
      
      Texture tex = assetManager.loadTexture(gravel_file);
      tex.setWrap(Texture.WrapMode.Repeat);

      mat.setTexture("DiffuseMap", tex);
      return mat;
    }
    
    Material mat = new Material(assetManager, 
      "Common/MatDefs/Misc/Unshaded.j3md");
     
    //Texture tex = assetManager.loadTexture("Textures/sand_sviller.png");
    //Texture tex = assetManager.loadTexture("Textures/sand_sviller_2.png");
    Texture tex = assetManager.loadTexture(gravel_file);
    tex.setWrap(Texture.WrapMode.Repeat);

    mat.setTexture("ColorMap", tex);

    return mat;
  }
  
    private Material materialTopMaterialLight(AssetManager assetManager)
  {
//     Material mat = new Material(assetManager, 
//        "Common/MatDefs/Light/Lighting.j3md");
     Material mat = new Material(assetManager, 
        "Common/MatDefs/Light/Lighting.j3md");
      //mat.setColor("Color", shape.getAc3dMaterial().getDiff());
//     mat.setTexture("DiffuseMap", 
//            assetManager.loadTexture("Textures/marker.jpg"));
//     mat.setTexture("ColorMap", 
//            assetManager.loadTexture("Textures/marker.jpg"));
     
//      Texture tex =  assetManager.loadTexture("Textures/marker.jpg");
//      tex.setWrap(Texture.WrapMode.Repeat);
     
//      mat.setTexture("ColorMap", tex);
      

        mat.setBoolean("UseMaterialColors",true);    
        mat.setColor("Ambient",ColorRGBA.Black);
        mat.setColor("Diffuse",ColorRGBA.White);
        mat.setColor("Specular",ColorRGBA.White);
        mat.setColor("GlowColor",ColorRGBA.Black);
        mat.setFloat("Shininess", 64f);
      
      
      return mat;*/
  }
    

}
