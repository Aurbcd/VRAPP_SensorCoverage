using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

    // Start is called before the first frame update
    Mesh mesh;
    Vector3[] vertices;
    Mesh sharedmesh;
    public float coverage;
    public bool Directions_6_or_4 = false;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        sharedmesh = GetComponent<MeshFilter>().sharedMesh;
        vertices = mesh.vertices;
        gameObject.transform.parent.gameObject.layer = 2;

    }
    public float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit_fwd;
        RaycastHit hit_bwd;

        for (var i = 0; i < vertices.Length; i++)
        {
            //Forward z
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit_fwd, 2) && vertices[i].z > hit_fwd.distance) { 
                vertices[i] -= Vector3.forward * 0.01f;
            }
            //Backward -z
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hit_bwd, 2) && vertices[i].z < -hit_bwd.distance)
            {
                vertices[i] -= Vector3.back * 0.01f;
            }
            //Right x
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit_fwd, 2) && vertices[i].x > hit_fwd.distance)
            {
                vertices[i] -= Vector3.right * 0.01f;
            }
            //Left -x
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit_bwd, 2) && vertices[i].x < -hit_bwd.distance)
            {
                vertices[i] -= Vector3.left * 0.01f;
            }
            //Up and down their noddin' y
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit_fwd, 2) && vertices[i].y > hit_fwd.distance)
            {
                vertices[i] -= Vector3.up * 0.01f;
            }
            //Up and down their noddin' -y
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit_bwd, 2) && vertices[i].y < -hit_bwd.distance)
            {
                vertices[i] -= Vector3.down* 0.01f;
            }

            if (Directions_6_or_4){
                //Up Right y,x
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up + Vector3.right), out hit_bwd, 2) && vertices[i].y + vertices[i].x > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.up + Vector3.right) * 0.01f;
                }

                //Up Left y,-x
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up + Vector3.left), out hit_bwd, 2) && vertices[i].y - vertices[i].x > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.up + Vector3.left) * 0.01f;
                }

                //Down Right -y,x
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down + Vector3.right), out hit_bwd, 2) && -vertices[i].y + vertices[i].x > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.down + Vector3.right) * 0.01f;
                }

                //Down Left -y,-x
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down + Vector3.left), out hit_bwd, 2) && -vertices[i].y - vertices[i].x > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.down + Vector3.left) * 0.01f;
                }

                //Forward Right z,x
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + Vector3.right), out hit_bwd, 2) && vertices[i].z + vertices[i].x > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.forward + Vector3.right) * 0.01f;
                }

                //Backward Right -z,x
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back + Vector3.right), out hit_bwd, 2) && -vertices[i].z + vertices[i].x > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.back + Vector3.right) * 0.01f;
                }

                //Backward Left -z,-x
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back + Vector3.left), out hit_bwd, 2) && -vertices[i].z - vertices[i].x > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.back + Vector3.left) * 0.01f;
                }

                //Forward Left z,-x
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + Vector3.left), out hit_bwd, 2) && vertices[i].z - vertices[i].x > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.forward + Vector3.left) * 0.01f;
                }

                //Up Forward y,z
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up + Vector3.forward), out hit_bwd, 2) && vertices[i].y + vertices[i].z > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.up + Vector3.forward) * 0.01f;
                }

                //Up Backward y,-z
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up + Vector3.back), out hit_bwd, 2) && vertices[i].y - vertices[i].z > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.up + Vector3.back) * 0.01f;
                }

                //down Forward -y,z
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down + Vector3.forward), out hit_bwd, 2) && -vertices[i].y + vertices[i].z > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.down + Vector3.forward) * 0.01f;
                }

                //down Backward -y,-z
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down + Vector3.back), out hit_bwd, 2) && -vertices[i].y - vertices[i].z > 1.5 * hit_bwd.distance)
                {
                    vertices[i] -= (Vector3.down + Vector3.back) * 0.01f;
                }
            }
        }

        // assign the local vertices array into the vertices array of the Mesh.
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        float volume = VolumeOfMesh(sharedmesh);
        volume *= transform.localScale.x * transform.localScale.y * transform.localScale.z;
        string msg = "The volume of the mesh is " + volume + " cube units.";
        coverage = volume;
    }



}
