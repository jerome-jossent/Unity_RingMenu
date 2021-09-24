using RingMenuJJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu_Interactions : MonoBehaviour
{
    public RingMenu_Manager ringMenu_Manager;
    public bool debug;
    public string hitname;

    private void Start()
    {
        GameObject menu = gameObject.transform.Find("menu").gameObject;
        ringMenu_Manager = menu.GetComponent<RingMenu_Manager>();
        ringMenu_Manager._ListAllButtons();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (ringMenu_Manager == null)
        {
            GameObject menu = gameObject.transform.Find("menu").gameObject;
            ringMenu_Manager = menu.GetComponent<RingMenu_Manager>();
            ringMenu_Manager._ListAllButtons();
        }

        ringMenu_Manager._InteractionManager(ray, Input.GetMouseButtonDown(0), out hitname, debug);
    }
}
