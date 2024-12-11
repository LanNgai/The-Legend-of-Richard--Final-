using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    //this script stores a array of models and on start replaces the connected gameobjects models with one from the array
    public List<GameObject> trashModels;
    Mesh mesh; 
    Material material;


    // Start is called before the first frame update
    void Start()
    {
        if (trashModels.Count > 0)
        {
            GameObject trash = trashModels[Random.Range(0, trashModels.Count)];
            //search trash gameobject for mesh
            if (trash.GetComponent<MeshFilter>() != null)
            {
                mesh = trash.GetComponent<MeshFilter>().mesh;
            }
            else
            {
                //search for mesh in children of trash gameobject
                foreach (Transform child in trash.transform)
                {
                    if (child.GetComponent<MeshFilter>()!= null)
                    {
                        mesh = child.GetComponent<MeshFilter>().mesh;
                        break;
                    }
                }
            }

            //search trash gameobject for material
            if (trash.GetComponent<MeshRenderer>().sharedMaterial!= null)
            {
                material = trash.GetComponent<MeshRenderer>().sharedMaterial;
            }
            else
            {
                //search for material in children of trash gameobject
                foreach (Transform child in trash.transform)
                {
                    if (child.GetComponent<MeshRenderer>()!= null)
                    {
                        material = child.GetComponent<MeshRenderer>().sharedMaterial;
                        break;
                    }
                }
            }

            //set gameObject mesh & material to new Material 
            this.GetComponent<MeshFilter>().mesh = mesh;
            this.GetComponent<MeshRenderer>().sharedMaterial = material;

        }
    }
}
