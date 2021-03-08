using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu_Manager : MonoBehaviour
{
    public Dictionary<string, RingButton_Manager> _buttons;
    RingButton_Manager rb_previsous = null;
    public string _selected_RingButton_name;
    public RingButton_Manager _selected_RingButton_Manager;
    internal void _ListAllButtons()
    {
        _buttons = new Dictionary<string, RingButton_Manager>();
        RingButton_Manager[] rbms = gameObject.GetComponentsInChildren<RingButton_Manager>();
        for (int i = 0; i < rbms.Length; i++)
            _buttons.Add(rbms[i]._name, rbms[i]);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Debug.Log(hit.transform.name);
            _selected_RingButton_name = hit.transform.name;
            RingButton_Manager rb = _buttons[hit.transform.name];
            _selected_RingButton_Manager = rb;
            if (rb != rb_previsous)
            {
                if (rb_previsous != null)
                    rb_previsous._SetNormalColor();

                if (rb != null)
                    rb._SetHighlightColor();
                rb_previsous = rb;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (rb != null)
                    rb._SetSelectedColor();
            }
        }
        else
        {
            _selected_RingButton_name = "";
            if (rb_previsous != null)
                rb_previsous._SetNormalColor();
            rb_previsous = null;
        }
    }
}
