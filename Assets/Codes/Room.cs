using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Furniture
{
    public Furniture father = null;
    public Transform furniture = null;

    public string name = "";
    public string family = null;

    public bool disabled_by_mutex = false;
    public bool has_mutex = false;
}

public class Room : MonoBehaviour
{

    bool hasControl = false;

    public List<Vector3> pov_list = new List<Vector3>();
    private int current_pov = 0;
    public Vector3 spawn_point;

    private List<Furniture> allFurniture = new List<Furniture>();
    private List<Furniture> addedFurniture = new List<Furniture>();

    private bool change_ocurred = false;

    //private 

    void addChildern(Transform child, Furniture father)
    {

            child.gameObject.SetActive(false);

            string split_dot = child.name.Split('.').ToList()[0];
            List<string> split_underscore = split_dot.Split('_').ToList();
            string name = split_underscore[0];

            if (name == "POV")
            {
                pov_list.Add(child.position);
                return;
            }

            Furniture child_furtniture = new Furniture();
            child_furtniture.father = father;
            child_furtniture.furniture = child;
            child_furtniture.name = name;

            if (split_underscore.Count > 1)
            {
                child_furtniture.has_mutex = true;
                child_furtniture.family = split_underscore[1];
            }

            allFurniture.Add(child_furtniture);

            foreach (Transform grandchild in child)
            {
                addChildern(grandchild, child_furtniture);
            }
    }

    void Start()
    {
        foreach (Transform child in this.transform)
        {
            addChildern(child, null);
        }
        availableAddFurniture();
    }

    public Vector3 getPOV()
    {
        return pov_list[current_pov];
    }

    public void changePOV()
    {
        current_pov = (current_pov + 1) % pov_list.Count;
    }

    void Update()
    {
        
    }

    List<Furniture> availableAddFurniture()
    {
        //List<Furniture> difference_furniture = allFurniture.Except(addedFurniture).ToList();

        List<Furniture> matching_furniture = new List<Furniture>();

        foreach (Furniture current_furniture in allFurniture)
        {
            Furniture match = addedFurniture.Find(x => x == current_furniture);

            if (match != null)
                continue;

            Furniture brother_match = addedFurniture.Find(x => x.family != null && x.family == current_furniture.family && x.father == current_furniture.father);

            if (brother_match != null)
            {
                continue;
            }

            Furniture father = current_furniture.father;

            if (father == null)
            {
                matching_furniture.Add(current_furniture);
                continue;
            }

            Furniture father_match = addedFurniture.Find(x => x == father);

            if (father_match == null)
                continue;

            matching_furniture.Add(current_furniture);
            
        }

        return matching_furniture;
    }

    public void addFurniture(Furniture furniture_to_add)
    {
        addedFurniture.Add(furniture_to_add);
        furniture_to_add.furniture.gameObject.SetActive(true);
        Destroy(furniture_to_add.furniture.GetComponent<ElementSelected>());
        index_addable_furniture = 0;
    }

    private int index_addable_furniture = 0;
    public Furniture getCurrentAddableFurniture()
    {
        List<Furniture> available_add_furniture = availableAddFurniture();

        if (available_add_furniture.Count == 0)
        return null;

        if (index_addable_furniture < 0)
            index_addable_furniture = 0;

        else if (index_addable_furniture >= available_add_furniture.Count)
            index_addable_furniture = available_add_furniture.Count - 1;

        return available_add_furniture[index_addable_furniture];
    }

    public Furniture nextAddableFurniture()
    {

        List<Furniture> available_add_furniture = availableAddFurniture();

        if (available_add_furniture.Count == 0)
            return null;

        available_add_furniture[index_addable_furniture].furniture.gameObject.SetActive(false);
        Destroy(available_add_furniture[index_addable_furniture].furniture.GetComponent<ElementSelected>());

        index_addable_furniture++;

        if (index_addable_furniture >= available_add_furniture.Count)
        {
            index_addable_furniture = 0;
        }

        available_add_furniture[index_addable_furniture].furniture.gameObject.SetActive(true);
        ElementSelected element_selected = available_add_furniture[index_addable_furniture].furniture.gameObject.AddComponent<ElementSelected>();

        return available_add_furniture[index_addable_furniture];

    }

    public Furniture prevAddableFurniture()
    {
        List<Furniture> available_add_furniture = availableAddFurniture();

        if (available_add_furniture.Count == 0)
            return null;

        available_add_furniture[index_addable_furniture].furniture.gameObject.SetActive(false);
        Destroy(available_add_furniture[index_addable_furniture].furniture.GetComponent<ElementSelected>());
        

        index_addable_furniture--;

        if (index_addable_furniture < 0)
        {
            index_addable_furniture = available_add_furniture.Count - 1;
        }

        available_add_furniture[index_addable_furniture].furniture.gameObject.SetActive(true);
        ElementSelected element_selected = available_add_furniture[index_addable_furniture].furniture.gameObject.AddComponent<ElementSelected>();

        return available_add_furniture[index_addable_furniture];
    }

    public void loadAddableFurniture(Furniture furniture_to_unload)
    {
        furniture_to_unload.furniture.gameObject.SetActive(true);
        furniture_to_unload.furniture.gameObject.AddComponent<ElementSelected>();
    }

    public void unloadAddableFurniture(Furniture furniture_to_unload)
    {
        furniture_to_unload.furniture.gameObject.SetActive(false);
        furniture_to_unload.furniture.GetComponent<ElementSelected>();
    }

    List<Furniture> availableDeleteFurniture()
    {
        return addedFurniture;
    }

    void recursiveDeleteFurniture(Furniture furniture_to_delete)
    {
        List<Furniture> children_furniture = new List<Furniture>();

        foreach (Furniture furniture in addedFurniture)
        {
            if (furniture.father == furniture_to_delete)
            {
                children_furniture.Add(furniture);
            }
        }

        foreach (Furniture child_to_delete in children_furniture)
        {
            recursiveDeleteFurniture(child_to_delete);
        }

        int furniture_delete_index = addedFurniture.FindIndex(x => x == furniture_to_delete);
        addedFurniture.RemoveAt(furniture_delete_index);
        furniture_to_delete.furniture.gameObject.SetActive(false);

    }

    private int index_removable_furniture = 0;

    public void deleteFurniture(Furniture furniture_to_delete)
    {
        recursiveDeleteFurniture(furniture_to_delete);
        Destroy(furniture_to_delete.furniture.GetComponent<ElementSelectedDelete>());
        index_removable_furniture = 0;
    }
    
    public Furniture getCurrentDeletableFurniture()
    {
        List<Furniture> available_delete_furniture = availableDeleteFurniture();

        if (available_delete_furniture.Count == 0)
            return null;

        if (index_removable_furniture < 0)
            index_removable_furniture = 0;

        else if (index_removable_furniture >= available_delete_furniture.Count)
            index_removable_furniture = available_delete_furniture.Count - 1;

        return available_delete_furniture[index_removable_furniture];
    }

    public Furniture nextDeletableFurniture()
    {

        List<Furniture> available_delete_furniture = availableDeleteFurniture();

        if (available_delete_furniture.Count == 0)
            return null;

        //available_delete_furniture[index_removable_furniture].furniture.gameObject.SetActive(false);
        Destroy(available_delete_furniture[index_removable_furniture].furniture.GetComponent<ElementSelectedDelete>());

        index_removable_furniture++;

        if (index_removable_furniture >= available_delete_furniture.Count)
        {
            index_removable_furniture = 0;
        }

        //available_delete_furniture[index_removable_furniture].furniture.gameObject.SetActive(true);
        ElementSelectedDelete element_selected = available_delete_furniture[index_removable_furniture].furniture.gameObject.AddComponent<ElementSelectedDelete>();

        return available_delete_furniture[index_removable_furniture];

    }

    public Furniture prevDeletableFurniture()
    {
        List<Furniture> available_delete_furniture = availableDeleteFurniture();

        if (available_delete_furniture.Count == 0)
            return null;

        //available_delete_furniture[index_removable_furniture].furniture.gameObject.SetActive(false);
        Destroy(available_delete_furniture[index_removable_furniture].furniture.GetComponent<ElementSelectedDelete>());
        

        index_removable_furniture--;

        if (index_removable_furniture < 0)
        {
            index_removable_furniture = available_delete_furniture.Count - 1;
        }

        //available_delete_furniture[index_removable_furniture].furniture.gameObject.SetActive(true);
        ElementSelectedDelete element_selected = available_delete_furniture[index_removable_furniture].furniture.gameObject.AddComponent<ElementSelectedDelete>();

        return available_delete_furniture[index_removable_furniture];
    }

    public void loadDeletableFurniture(Furniture furniture_to_unload)
    {
        //furniture_to_unload.furniture.gameObject.SetActive(true);
        furniture_to_unload.furniture.gameObject.AddComponent<ElementSelectedDelete>();
    }

    public void unloadDeletableFurniture(Furniture furniture_to_unload)
    {
        //furniture_to_unload.furniture.gameObject.SetActive(false);
        ElementSelectedDelete component_to_delete = furniture_to_unload.furniture.GetComponent<ElementSelectedDelete>();
        if (component_to_delete != null)
            Destroy(component_to_delete);
    }

}
