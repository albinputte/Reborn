using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControllert : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform LaserSource;
    public Transform LaserTarget;

    // Start is called before the first frame update
    void Start()
    {
        AsignTarget(LaserSource, LaserTarget);
    }

    public void AsignTarget(Transform startPos, Transform endPos)
    {
         lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos.position);
        LaserTarget = endPos;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(1,LaserTarget.position);
    }
}
