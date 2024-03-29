﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RingMenuJJ
{
    public class RingMenu_Manager : MonoBehaviour
    {
        public Dictionary<string, RingButton_Manager> _buttons;
        public RingButton_Manager _selected_RingButton_Manager;
        RingButton_Manager rb_previsous = null;
        public string _selected_RingButton_name;

        public event EventHandler<EventArgs> _OnSelected;
        public event EventHandler<EventArgs> _OnEnter;
        public event EventHandler<EventArgs> _OnExit;

        internal void _ListAllButtons()
        {
            _buttons = new Dictionary<string, RingButton_Manager>();
            RingButton_Manager[] rbms = gameObject.GetComponentsInChildren<RingButton_Manager>();
            for (int i = 0; i < rbms.Length; i++)
                _buttons.Add(rbms[i].gameObject.name, rbms[i]);
        }

        public void _InteractionManager(Ray ray, bool select, out string hitname, bool debug = false)
        {
            hitname = "";
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (debug)
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);

                hitname = hit.transform.name;
                if (!_buttons.ContainsKey(hitname))
                    return;

                _selected_RingButton_name = hitname;
                RingButton_Manager rbm = _buttons[_selected_RingButton_name];
                _selected_RingButton_Manager = rbm;
                if (rbm != rb_previsous)
                {
                    //reset couleur de l'ancien bouton
                    if (rb_previsous != null)
                    {
                        rb_previsous._SetNormalColor();
                        rbm._OnExit.Invoke();
                        //Debug.Log(rbm.gameObject.name + " [EXIT]");
                        //_OnExit?.Invoke(rb_previsous, new EventArgs());
                    }

                    //set couleur surligné du bouton visé
                    if (rbm != null)
                    {
                        rbm._SetHighlightColor();
                        rbm._OnEnter.Invoke();             
                        //Debug.Log(rbm.gameObject.name + " [ENTER]");
                        //_OnEnter?.Invoke(rbm, new EventArgs());
                    }

                    rb_previsous = rbm;
                }

                if (select)
                {
                    if (rbm != null)
                    {
                        //set couleur enfoncé du bouton visé
                        rbm._SetSelectedColor();
                        rbm._OnClick.Invoke();
                        //Debug.Log(rbm.gameObject.name + " [SELECTED]");
                        //_OnSelected?.Invoke(rbm, new EventArgs());
                    }
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
}