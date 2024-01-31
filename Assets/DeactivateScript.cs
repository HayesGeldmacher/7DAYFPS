using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateScript : MonoBehaviour
{

    public void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }
}
