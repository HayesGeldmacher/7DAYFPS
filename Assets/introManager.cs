using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introManager : MonoBehaviour
{
   public void LoadScene()
    {
        SceneManager.LoadScene("testScene");
    }
}
