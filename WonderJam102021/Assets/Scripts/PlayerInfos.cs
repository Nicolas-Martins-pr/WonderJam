using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    public static PlayerInfos PI;

    private void OnEnable()
    {
        if (PlayerInfos.PI == null)
        {
            PlayerInfos.PI = this;
        }
        else
        {
            if (PlayerInfos.PI != this)
            {
                Destroy(PlayerInfos.PI.gameObject);
                PlayerInfos.PI = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

}
