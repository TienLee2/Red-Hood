using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(DestroyFF(3f));
    }

    IEnumerator DestroyFF(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
