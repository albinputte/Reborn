using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    private void Awake()
    {
        if (_i != null && _i != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        _i = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes
    }
    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }

    public Transform pfDamagePopup;
}
