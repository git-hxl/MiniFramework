using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    // Use this for initialization
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
			Debug.Log("xxxxxxxxxxxxxxxxxxxxxxxxxxxx");
        }
    }
}
