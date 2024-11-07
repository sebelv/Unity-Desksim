using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    public static MeshCombiner Instance;
    [SerializeField] private bool activeCombiner = false;

//    [SerializeField] private Transform combineThis;
//    [SerializeField] private bool combineObject;

    void Awake ()
    {
        Instance = this;
    }

    void Update()
    {
        // This is for testing the combining of any object
        /*
        if(combineObject)
        {
            combineObject = false;
            CombineChildren(combineThis.gameObject);
        }*/
    }

    public void CombineChildren(GameObject parentObject)
    {
        if(activeCombiner)
        {
            List<MeshFilter> meshFilters = new List<MeshFilter>();
            meshFilters.AddRange(parentObject.GetComponentsInChildren<MeshFilter>());
            for(int i = 0; i < meshFilters.Count;)
            {
                List<MeshFilter> meshFiltersPerMaterial = new List<MeshFilter>();
                meshFiltersPerMaterial.Add(meshFilters[i]);
                meshFilters.Remove(meshFilters[i]);
                Material mat = meshFiltersPerMaterial[i].transform.GetComponent<MeshRenderer>().sharedMaterial;

                for(int o = 0; o < meshFilters.Count; o++)
                {
                    if(meshFilters[o].GetComponent<MeshRenderer>().sharedMaterial == mat)
                    {
                        meshFiltersPerMaterial.Add(meshFilters[o]);
                        meshFilters.Remove(meshFilters[o]);
                        o--;
                    }
                }
                int k = 0;
                CombineInstance[] combine = new CombineInstance[meshFiltersPerMaterial.Count];
                while (k < meshFiltersPerMaterial.Count)
                {
                    combine[k].mesh = meshFiltersPerMaterial[k].sharedMesh;
                    combine[k].transform = meshFiltersPerMaterial[k].transform.localToWorldMatrix;
                    meshFiltersPerMaterial[k].gameObject.SetActive(false);

                    k++;
                }
                GameObject newChild = new GameObject("Material Mesh");
                Mesh mesh = new Mesh();
                mesh.CombineMeshes(combine);
                if(newChild.transform.GetComponent<MeshFilter>() == null)
                {
                    newChild.AddComponent<MeshFilter>();
                    newChild.AddComponent<MeshRenderer>().material = mat;
                }
                newChild.transform.GetComponent<MeshFilter>().sharedMesh = mesh;
                newChild.transform.parent = parentObject.transform;
            }
            parentObject.SetActive(true);
            GameObject.Destroy(parentObject.transform.Find("Track Object").gameObject);
        }
    }
}
