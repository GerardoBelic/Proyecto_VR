using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Menus
{
    Menu_Principal,
    Editar_Habitaciones
}

public class EditCanvas : MonoBehaviour
{

    public Transform principal_menu;
    public Transform boton_editar_habitaciones;
    public Transform boton_explorar_casa;
    public Transform boton_cambiar_materiales;
    public Transform boton_alternar_dia_noche;

    public Transform edit_rooms;
    public Transform boton_cambiar_habitacion;
    public Transform boton_cambiar_camara;
    public Transform boton_agregar_muebles;
    public Transform boton_eliminar_muebles;
    public TextMeshProUGUI texto_seleccion;

    private Material select_material;
    private Material over_material;

    private Menus current_menu = Menus.Menu_Principal;

    void Start()
    {
        select_material = Resources.Load<Material>("Materials/Hologram 3");
        over_material = Resources.Load<Material>("Materials/Hologram 4");
    }

    void Update()
    {
        
    }

    public void change_text_selection(string text)
    {

        texto_seleccion.text = text;

    }

    public void change_menu(Menus new_menu)
    {
        current_menu = new_menu;

        if (new_menu == Menus.Menu_Principal)
        {
            principal_menu.gameObject.SetActive(true);
            edit_rooms.gameObject.SetActive(false);
        }
        else if (new_menu == Menus.Editar_Habitaciones)
        {
            principal_menu.gameObject.SetActive(false);
            edit_rooms.gameObject.SetActive(true);
        }
    }

    private Transform last_selected_button = null;

    public void select_button(Menu_Principal menu_principal_button, int mode)
    {

        if (last_selected_button != null)
        {
            last_selected_button.gameObject.GetComponent<Image>().material = null;
            //last_selected_button = null;
        }

        switch (menu_principal_button)
        {
        case Menu_Principal.Elegir_Habitacion:

            last_selected_button = boton_editar_habitaciones;

            break;

        case Menu_Principal.Explorar_Casa:

            last_selected_button = boton_explorar_casa;

            break;

        case Menu_Principal.Materiales_Casa:

            last_selected_button = boton_cambiar_materiales;

            break;

        case Menu_Principal.Alternar_Dia_Noche:

            last_selected_button = boton_alternar_dia_noche;

            break;

        }

        if (mode == 1)
        {
            last_selected_button.gameObject.GetComponent<Image>().material = over_material;
        }
        else if (mode == 2)
        {
            last_selected_button.gameObject.GetComponent<Image>().material = select_material;
        }

    }

    public void select_button(Editar_Habitacion editar_habitacion_button, int mode)
    {
        if (last_selected_button != null)
        {
            last_selected_button.gameObject.GetComponent<Image>().material = null;
            //last_selected_button = null;
        }

        switch (editar_habitacion_button)
        {
        case Editar_Habitacion.Cambiar_Habitacion:

            last_selected_button = boton_cambiar_habitacion;

            break;

        case Editar_Habitacion.Cambiar_Camara:

            last_selected_button = boton_cambiar_camara;

            break;

        case Editar_Habitacion.Agregar_Muebles:

            last_selected_button = boton_agregar_muebles;

            break;

        case Editar_Habitacion.Eliminar_Muebles:

            last_selected_button = boton_eliminar_muebles;

            break;
        }

        if (mode == 1)
        {
            last_selected_button.gameObject.GetComponent<Image>().material = over_material;
        }
        else if (mode == 2)
        {
            last_selected_button.gameObject.GetComponent<Image>().material = select_material;
        }

    }

}
