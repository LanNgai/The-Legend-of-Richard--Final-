using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    //this script stores a array of models and on start replaces the connected gameobjects models with one from the array
    public List<GameObject> trashModels;


    // Start is called before the first frame update
    void Start()
    {
        if (trashModels.Count > 0)
        {
            GameObject trash = trashModels[Random.Range(0, trashModels.Count)];
            transform.GetComponent<MeshFilter>().mesh = trash.GetComponent<MeshFilter>().mesh;
            transform.GetComponent<MeshRenderer>().material = trash.GetComponent<MeshRenderer>().material;
        }
    }
}
