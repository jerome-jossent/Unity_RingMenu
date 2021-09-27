using RingMenuJJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu_Interactions : MonoBehaviour
{
    public RingMenu_Manager ringMenu_Manager;
    public bool debug;
    public string hitname;

    public float period = 0.1f;
    public float nexttime;

    private void Start()
    {
        //ringMenu_Manager = GetComponentInChildren<RingMenu_Manager>();
        //GameObject menu = ringMenu_Manager.gameObject;
        //ringMenu_Manager._ListAllButtons();
    }

    bool btnclick;

    void Update()
    {
        if (ringMenu_Manager == null)
        {
            //GameObject menu = gameObject.transform.Find("menu").gameObject;
            //ringMenu_Manager = menu.GetComponent<RingMenu_Manager>();
            //ringMenu_Manager._ListAllButtons();

            ringMenu_Manager = GetComponentInChildren<RingMenu_Manager>();
            GameObject menu = ringMenu_Manager.gameObject;
            ringMenu_Manager._ListAllButtons();

            RingMenu_Manager[] rs = GetComponentsInChildren<RingMenu_Manager>();
            foreach (RingMenu_Manager r in rs)
            {
                if (r == ringMenu_Manager) continue;

                r._ListAllButtons();
                foreach (var item in r._buttons)
                    ringMenu_Manager._buttons.Add(item.Key, item.Value);
            }
        }

        if (Input.GetMouseButtonDown(0))
            btnclick = true;

        if (!btnclick && nexttime > Time.time)
            return;
        nexttime += period;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        

        ringMenu_Manager._InteractionManager(ray, btnclick, out hitname, debug);
        btnclick = false;
    }
}
