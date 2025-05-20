using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStructure
{

    public bool PerformBuild(int index);
    public bool CancelBuild(int index);
}
