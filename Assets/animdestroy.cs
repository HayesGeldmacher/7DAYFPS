using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animdestroy : MonoBehaviour
{
   public void Deactivate()
    {
      transform.gameObject.SetActive(false);
    }
}
