using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractableType type { get; set; }
    void Interact();
}

[Serializable]
public enum InteractableType
{
    Bush = 0,
    Minning = 1,
    Crafting = 2,
}
