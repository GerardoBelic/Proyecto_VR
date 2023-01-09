using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeMaterial : MonoBehaviour
{

    //private Material current_mesh_material;
    private MeshRenderer renderer;
    public int renderer_material_index = -1;

    public Material initial_material;
    public List<Material> materials;
    private int current_material_index = 0;

    void Start()
    {
        
        materials.Insert(0, initial_material);

        renderer = GetComponent<MeshRenderer>();

        Material[] mesh_materials = renderer.materials;

        renderer_material_index = Array.FindIndex(mesh_materials,
                                         element => element.name.Contains(initial_material.name));

    }

    public void prev()
    {

        current_material_index = (current_material_index - 1) % materials.Count;
        if (current_material_index < 0)
            current_material_index = materials.Count - 1;

        //renderer.materials[renderer_material_index] = materials[current_material_index];

        Material[] mesh_materials = renderer.materials;
        mesh_materials[renderer_material_index] = materials[current_material_index];
        renderer.materials = mesh_materials;

    }

    public void next()
    {

        current_material_index = (current_material_index + 1) % materials.Count;

        //renderer.materials[renderer_material_index] = materials[current_material_index];
        Material[] mesh_materials = renderer.materials;
        mesh_materials[renderer_material_index] = materials[current_material_index];
        renderer.materials = mesh_materials;

    }

    void Update()
    {
        
        /*if (Input.GetKeyDown("space"))
            next();*/

    }
}
