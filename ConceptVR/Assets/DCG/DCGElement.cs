using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCGElement {
    private static int currentID = 0;
    public int elementID;
    public int lastMoveID;

    public int nextElementID()
    {
        return currentID++;
    }
}
