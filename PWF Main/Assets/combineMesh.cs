using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class combineMesh : MonoBehaviour
{
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

     
        for (int i = 0; i < meshFilters.Length; ++i)
        {
        	if (meshFilters [i].transform == transform)
        		continue;
        	

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);

		AssetDatabase.CreateAsset(transform.GetComponent<MeshFilter>().sharedMesh, "Assets/netmesh.asset");
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
    }

}