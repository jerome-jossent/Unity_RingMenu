﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ToHTML : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void HelloString(string str);

    [DllImport("__Internal")]
    private static extern void PrintFloatArray(float[] array, int size);

    [DllImport("__Internal")]
    private static extern int AddNumbers(int x, int y);

    [DllImport("__Internal")]
    private static extern string StringReturnValueFunction();

    [DllImport("__Internal")]
    private static extern void BindWebGLTexture(int texture);


    [DllImport("__Internal")]
    private static extern void FillCode1(string str);
    [DllImport("__Internal")]
    private static extern void FillCode2(string str);
    [DllImport("__Internal")]
    private static extern void FillCode3(string str);

    void Start()
    {
        //Hello();

        //HelloString("This is a string.");

        //float[] myArray = new float[10];
        //PrintFloatArray(myArray, myArray.Length);

        //int result = AddNumbers(5, 7);
        //Debug.Log(result);

        //Debug.Log(StringReturnValueFunction());

        //var texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        //BindWebGLTexture(texture.GetNativeTexturePtr());
    }

    public void _OBJ_TO_HTML(string obj)
    {
        FillCode1(obj);
    }
    public void _MTL_TO_HTML(string mtl)
    {
        FillCode2(mtl);
    }
    public void _FBX_TO_HTML(string fbx)
    {
        FillCode3(fbx);
    }
}