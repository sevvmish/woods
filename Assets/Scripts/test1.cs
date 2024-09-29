using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    private MeshFilter mf;

    [SerializeField] private GameObject pointer;

    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();

        Mesh mesh = mf.mesh;
        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < mesh.normals.Length; i++)
        {
            //print(i  + ": " + mesh.normals[i] + " - " + verts[i]);
            GameObject g = Instantiate(pointer, GameObject.Find("ttt").transform);
            g.transform.position = mf.transform.position + verts[i];
            //g.transform.LookAt(mf.transform.position + verts[i] + mesh.normals[i]);
            g.SetActive(true);

        }

    }

}
