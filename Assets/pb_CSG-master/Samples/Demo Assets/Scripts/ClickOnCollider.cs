using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parabox.CSG.Demo
{
    public class ClickOnCollider : MonoBehaviour
    {
        ScriptsManager _sm;

        private void Awake()
        {
            _sm = GameObject.Find("ScriptsManager").GetComponent<ScriptsManager>();
        }

        void OnMouseDown()
        {
            Debug.Log(gameObject.name);
            _sm._gameObjectSelectionManager._NewSelection(gameObject);
        }
    }
}
