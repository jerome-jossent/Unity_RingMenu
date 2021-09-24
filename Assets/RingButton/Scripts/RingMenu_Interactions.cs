using RingMenuJJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu_Interactions : MonoBehaviour
{
    RingMenu_Manager ringMenu_Manager;

    private void Start()
    {
        ringMenu_Manager = gameObject.GetComponentInChildren<RingMenu_Manager>();
        ringMenu_Manager._ListAllButtons();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
        ringMenu_Manager._InteractionManager(ray, Input.GetMouseButtonDown(0));
    }
}
