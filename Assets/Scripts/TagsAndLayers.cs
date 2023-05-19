using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TagsAndLayers
{
    // TAGS

    public static string MainCameraTag = "MainCamera";


    // LAYERS

    public static string DefaultLayerName = "Default";
    public static int DefaultLayerIndex = LayerMask.NameToLayer(DefaultLayerName);

    public static string UILayerName = "UI";
    public static int UILayerIndex = LayerMask.NameToLayer(UILayerName);
}