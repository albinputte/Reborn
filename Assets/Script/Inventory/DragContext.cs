using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragContext 
{
    public static DragSourceType SourceType;
    public static int SourceIndex = -1;
}
public enum DragSourceType { Inventory, Chest, AccesorieSlot }
