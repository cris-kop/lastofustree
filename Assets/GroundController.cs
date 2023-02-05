using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Material[] groundMaterials;
    
    public void ChangeMaterial(int seasonIndex)
    {
        GetComponent<MeshRenderer>().material = groundMaterials[seasonIndex];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
