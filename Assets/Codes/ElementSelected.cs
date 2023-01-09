using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSelected : MonoBehaviour
{

    private Material temporal_material;

    private MeshRenderer renderer;

    private Material[] mesh_original_materials;

    public bool marked_for_delete = false;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();

        //Material[] mesh_original_materials = new Material[renderer.materials.Length];
        //renderer.materials.CopyTo(mesh_original_materials, 0);
        mesh_original_materials = renderer.materials;

        temporal_material = Resources.Load<Material>("Materials/Hologram");

        Material[] new_materials = new Material[mesh_original_materials.Length];
        for (int i = 0; i < renderer.materials.Length; ++i)
        {
            new_materials[i] = temporal_material;
        }

        renderer.materials = new_materials;
    }

    /*public void setTemporalMaterial(Material mat)
    {
        print(mat.name);
        //temporal_material = Instantiate(mat);
        temporal_material = mat;

        renderer.materials = new Material[] {temporal_material};
    }*/

    void Update()
    {
        
    }

    void OnDestroy()
    {
        //print(renderer.materials == null);
        renderer.materials = mesh_original_materials;
    }

}
