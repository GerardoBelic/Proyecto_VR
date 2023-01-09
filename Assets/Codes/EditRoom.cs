using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Editar_Habitacion
{
    Cambiar_Habitacion,
    Cambiar_Camara,
    Agregar_Muebles,
    //Editar_Muebles,
    Eliminar_Muebles
}

public class EditRoom : MonoBehaviour
{

    public bool hasControl = false;

    private Transform camera;

    private Editar_Habitacion current_state_room_edit = Editar_Habitacion.Cambiar_Habitacion;
    private bool in_edition = false;

    public List<Transform> rooms;
    private int current_room = 0;

    public List<Transform> objects_disabled_explore;

    public List<Transform> house_material_parts;

    private DPAD dpad = new DPAD();

    public EditCanvas edit_canvas;

    public bool newEnter = false;

    void Start()
    {
        foreach (Transform room in rooms)
        {
            Room currenct_room = room.gameObject.AddComponent<Room>();
        }

        camera = GameObject.Find("Player").transform;
        edit_canvas = GameObject.Find("Canvas").gameObject.GetComponent<EditCanvas>();

    }

    void Update()
    {

        if (newEnter)
        {
            newEnter = false;
            return;
        }

        if (!hasControl)
        {
            return;
        }
        else if (hasControl && !in_edition && Input.GetButtonDown("B"))
        {
            hasControl = false;
            edit_canvas.change_menu(Menus.Menu_Principal);
            return;
        }

        dpad.update();

        edit_canvas.change_text_selection("");

        switch (current_state_room_edit)
        {
        case Editar_Habitacion.Cambiar_Habitacion:
            edit_canvas.select_button(current_state_room_edit, 1);
            if (in_edition || Input.GetButtonDown("A"))
            {
                edit_canvas.select_button(current_state_room_edit, 2);
                in_edition = change_room();
                return;
            }                
            else if (dpad.first_dpad_down)
                current_state_room_edit = Editar_Habitacion.Cambiar_Camara;
            else if (dpad.first_dpad_up)
                current_state_room_edit = Editar_Habitacion.Eliminar_Muebles;

            break;

        case Editar_Habitacion.Cambiar_Camara:
            edit_canvas.select_button(current_state_room_edit, 1);
            if (Input.GetButtonDown("A"))
            {
                change_camera();
            }                
            else if (dpad.first_dpad_down)
                current_state_room_edit = Editar_Habitacion.Agregar_Muebles;
            else if (dpad.first_dpad_up)
                current_state_room_edit = Editar_Habitacion.Cambiar_Habitacion;

            break;

        case Editar_Habitacion.Agregar_Muebles:
            edit_canvas.select_button(current_state_room_edit, 1);
            if (in_edition || Input.GetButtonDown("A"))
            {
                in_edition = add_furniture();
                edit_canvas.select_button(current_state_room_edit, 2);
            }
            else if (dpad.first_dpad_down)
                current_state_room_edit = Editar_Habitacion.Eliminar_Muebles;
            else if (dpad.first_dpad_up)
                current_state_room_edit = Editar_Habitacion.Cambiar_Camara;

            break;

        /*case Editar_Habitacion.Editar_Muebles:
            if (in_edition)
                in_edition = edit_furniture();
            else if (dpad.first_dpad_down)
                current_state_room_edit = Editar_Habitacion.Eliminar_Muebles;
            else if (dpad.first_dpad_up)
                current_state_room_edit = Editar_Habitacion.Agregar_Muebles;

            break;*/

        case Editar_Habitacion.Eliminar_Muebles:
            edit_canvas.select_button(current_state_room_edit, 1);
            if (in_edition || Input.GetButtonDown("A"))
            {
                in_edition = delete_furniture();
                edit_canvas.select_button(current_state_room_edit, 2);
            }
            else if (dpad.first_dpad_down)
                current_state_room_edit = Editar_Habitacion.Cambiar_Habitacion;
            else if (dpad.first_dpad_up)
                current_state_room_edit = Editar_Habitacion.Agregar_Muebles;

            break;

        }

        var enumDisp = (Editar_Habitacion)current_state_room_edit;
        if (!in_edition)
            print(enumDisp.ToString());
        
    }

    public float adjust_camera()
    {

        Vector3 target_position = rooms[current_room].gameObject.GetComponent<Room>().getPOV();

        float distance = Vector3.Distance(target_position, camera.position);

        if (distance > 0.1f)
        {
            Vector3 vector_destino = target_position - camera.position;
            camera.position += vector_destino * Time.deltaTime * 2.0f;
        }

        return distance;

    }

    bool change_room()
    {
        bool finished = false;

        if (dpad.first_dpad_right)
            current_room = (current_room + 1) % rooms.Count;
        else if (dpad.first_dpad_left)
        {
            current_room = (current_room - 1) % rooms.Count;
            current_room = (current_room < 0) ? current_room + rooms.Count : current_room;
        }
        else if (Input.GetButtonDown("B"))
            finished = true;

        edit_canvas.change_text_selection(rooms[current_room].gameObject.name);

        adjust_camera();

        return !finished;

    }

    void change_camera()
    {
        rooms[current_room].gameObject.GetComponent<Room>().changePOV();

        camera.position = rooms[current_room].gameObject.GetComponent<Room>().getPOV();
    }

    private Furniture current_furniture = null;

    bool add_furniture()
    {
        bool finished = false;

        Room current_room_component = rooms[current_room].gameObject.GetComponent<Room>();

        if (current_furniture == null)
        {
            current_furniture = current_room_component.getCurrentAddableFurniture();

            if (current_furniture != null)
            {
                current_room_component.loadAddableFurniture(current_furniture);
            }
        }
        else if (Input.GetButtonDown("A") && current_furniture != null)
        {
            current_room_component.addFurniture(current_furniture);
            current_furniture = null;
        }
        else if (dpad.first_dpad_right)
        {
            current_furniture = current_room_component.nextAddableFurniture();
        }
        else if (dpad.first_dpad_left)
        {
            current_furniture = current_room_component.prevAddableFurniture();
        }
        if (Input.GetButtonDown("B"))
        {
            finished = true;

            if (current_furniture != null)
                current_room_component.unloadAddableFurniture(current_furniture);

            current_furniture = null;            
        }

        if (current_furniture != null)
        {
            edit_canvas.change_text_selection(current_furniture.name);
        }

        return !finished;
    }

    bool delete_furniture()
    {
        bool finished = false;

        Room current_room_component = rooms[current_room].gameObject.GetComponent<Room>();

        if (current_furniture == null)
        {
            current_furniture = current_room_component.getCurrentDeletableFurniture();

            if (current_furniture != null)
            {
                current_room_component.loadDeletableFurniture(current_furniture);
            }
        }
        else if (Input.GetButtonDown("A") && current_furniture != null)
        {
            current_room_component.deleteFurniture(current_furniture);
            current_furniture = null;
        }
        else if (dpad.first_dpad_right)
        {
            current_furniture = current_room_component.nextDeletableFurniture();
        }
        else if (dpad.first_dpad_left)
        {
            current_furniture = current_room_component.prevDeletableFurniture();
        }
        if (Input.GetButtonDown("B"))
        {
            finished = true;

            if (current_furniture != null)
                current_room_component.unloadDeletableFurniture(current_furniture);

            current_furniture = null;            
        }
        
        if (current_furniture != null)
        {
            edit_canvas.change_text_selection(current_furniture.name);
        }

        return !finished;
    }

}
