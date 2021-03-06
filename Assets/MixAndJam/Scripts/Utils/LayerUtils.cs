﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerUtils 
{
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}
