using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSelectionManager : MonoBehaviour
{
    public GameObject _Selection;

    internal void _NewSelection(GameObject gameObject)
    {
        _Selection = gameObject;
    }
}
