using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Menu_Principal
{
    Elegir_Habitacion,
    Explorar_Casa,
    Materiales_Casa,
    Alternar_Dia_Noche
}

public class UI : MonoBehaviour
{

    public EditRoom edit_room;
    //public Explore explore;
    public Character character;

    private Menu_Principal current_state_menu = Menu_Principal.Elegir_Habitacion;

    private DPAD dpad = new DPAD();

    private bool exploring = false;

    public Material skybox_day;
    public Material skybox_night;

    public GameObject directional_light;

    private List<Transform> house_material_parts;

    public EditCanvas edit_canvas;

    private Transform camera;

    void Start()
    {
        camera = GameObject.Find("Player").transform;
    }

    void loadHouseMaterials()
    {
        house_material_parts = edit_room.house_material_parts;
    }

    private int house_material_index = 0;
    private bool editing_material = false;
    private bool first_material_enter = false;

    bool editHouseMaterials()
    {
        bool finished = false;

        if (!editing_material && house_material_parts[house_material_index].GetComponent<ElementSelectedEdit>() == null)
        {
            house_material_parts[house_material_index].gameObject.AddComponent<ElementSelectedEdit>();
        }

        if (!first_material_enter && (Input.GetButtonDown("A") || editing_material))
        {
            editing_material = true;

            if (house_material_parts[house_material_index].GetComponent<ElementSelectedEdit>() != null)
            {
                Destroy(house_material_parts[house_material_index].GetComponent<ElementSelectedEdit>());
            }

            ChangeMaterial change_material = house_material_parts[house_material_index].GetComponent<ChangeMaterial>();

            if (change_material != null && dpad.first_dpad_right)
            {
                change_material.next();
            }
            else if (change_material != null && dpad.first_dpad_left)
            {
                change_material.prev();
            }
            else if (Input.GetButtonDown("B"))
            {
                editing_material = false;
            }

        }
        else if (dpad.first_dpad_left)
        {
            Destroy(house_material_parts[house_material_index].GetComponent<ElementSelectedEdit>());

            house_material_index--;
            if (house_material_index < 0)
                house_material_index = house_material_parts.Count - 1;

            house_material_parts[house_material_index].gameObject.AddComponent<ElementSelectedEdit>();
        }
        else if (dpad.first_dpad_right)
        {
            Destroy(house_material_parts[house_material_index].GetComponent<ElementSelectedEdit>());

            house_material_index++;
            if (house_material_index > house_material_parts.Count - 1)
                house_material_index = 0;

            house_material_parts[house_material_index].gameObject.AddComponent<ElementSelectedEdit>();
        }
        else if (Input.GetButtonDown("B"))
        {
            Destroy(house_material_parts[house_material_index].GetComponent<ElementSelectedEdit>());

            finished = true;
        }

        first_material_enter = false;

        return !finished;
    }

    private float distance_exploring_to_pov = 0.0f;

    private bool editing_materials_house = false;

    void Update()
    {

        if (edit_room == null && GameObject.Find("House") != null)
        {
            edit_room = GameObject.Find("House").gameObject.GetComponent<EditRoom>();

            if (edit_room != null)
            {
                GameObject.Find("Canvas/Black Screen").gameObject.SetActive(false);
                GameObject.Find("Canvas/Loading Screen").gameObject.SetActive(false);
                edit_canvas.change_menu(Menus.Menu_Principal);
            }

            return;
        }

        if (edit_room.hasControl)
            return;

        dpad.update();

        switch (current_state_menu)
        {
            case Menu_Principal.Elegir_Habitacion:
                edit_canvas.select_button(current_state_menu, 1);
                if (Input.GetButtonDown("A")){
                    edit_room.hasControl = true;
                    edit_room.newEnter = true;

                    if (edit_room.edit_canvas == null)
                        edit_room.edit_canvas = edit_canvas;

                    edit_canvas.change_menu(Menus.Editar_Habitaciones);
                }
                else if (dpad.first_dpad_down == true)
                    current_state_menu = Menu_Principal.Explorar_Casa;
                else if (dpad.first_dpad_up == true)
                    current_state_menu = Menu_Principal.Alternar_Dia_Noche;

            break;

            case Menu_Principal.Explorar_Casa:
                
                if (exploring && Input.GetButtonDown("B"))
                {
                    exploring = false;
                    character.canMove = false;
                    //distance_exploring_to_pov = edit_room.adjust_camera();

                    foreach(Transform object_to_disable in edit_room.objects_disabled_explore)
                    {
                        object_to_disable.gameObject.SetActive(true);
                    }
                }
                else if (!exploring && Input.GetButtonDown("A"))
                {
                    edit_canvas.select_button(current_state_menu, 2);
                    exploring = true;
                    character.canMove = true;

                    foreach(Transform object_to_disable in edit_room.objects_disabled_explore)
                    {
                        object_to_disable.gameObject.SetActive(false);
                    }
                }
                /*else if (!exploring && distance_exploring_to_pov > 0.1f)
                {
                    distance_exploring_to_pov = edit_room.adjust_camera();
                }*/
                else if (dpad.first_dpad_down == true)
                    current_state_menu = Menu_Principal.Materiales_Casa;
                else if (dpad.first_dpad_up == true)
                    current_state_menu = Menu_Principal.Elegir_Habitacion;
                else if (!exploring)
                    edit_canvas.select_button(current_state_menu, 1);

            break;

            case Menu_Principal.Materiales_Casa:
                edit_canvas.select_button(current_state_menu, 1);
                if (Input.GetButtonDown("A") || editing_materials_house)
                {   
                    edit_canvas.select_button(current_state_menu, 2);
                    if (house_material_parts == null)
                    {
                        loadHouseMaterials();
                    }

                    if (!editing_materials_house)
                        first_material_enter = true;

                    editing_materials_house = editHouseMaterials();
                    break;
                }
                else if (dpad.first_dpad_down == true)
                    current_state_menu = Menu_Principal.Alternar_Dia_Noche;
                else if (dpad.first_dpad_up == true)
                    current_state_menu = Menu_Principal.Explorar_Casa;

            break;

            case Menu_Principal.Alternar_Dia_Noche:
                edit_canvas.select_button(current_state_menu, 1);
                if (Input.GetButtonDown("A"))
                {
                    if (RenderSettings.skybox == skybox_day)
                    {
                        RenderSettings.skybox = skybox_night;
                        //directional_light.gameObject.SetActive(false);
                        directional_light.gameObject.GetComponent<Light>().intensity = 0.1f;
                        camera.gameObject.GetComponent<Light>().enabled = true;
                    }
                    else
                    {
                        RenderSettings.skybox = skybox_day;
                        //directional_light.gameObject.SetActive(true);
                        directional_light.gameObject.GetComponent<Light>().intensity = 1.0f;
                        camera.gameObject.GetComponent<Light>().enabled = false;
                    }
                }
                else if (dpad.first_dpad_down == true)
                    current_state_menu = Menu_Principal.Elegir_Habitacion;
                else if (dpad.first_dpad_up == true)
                    current_state_menu = Menu_Principal.Materiales_Casa;

            break;

            break;
        }

        var enumDisp = (Menu_Principal)current_state_menu;
        //print(enumDisp.ToString());
        
    }
}
