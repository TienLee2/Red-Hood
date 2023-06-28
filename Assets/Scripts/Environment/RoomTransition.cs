using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject backGround;
    public AudioSource _AudioSource;
    public AudioClip _AudioClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(true);
            backGround.SetActive(true);
            _AudioSource.clip = _AudioClip;
            _AudioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(false);
            backGround.SetActive(false);
            _AudioSource.Pause();
        }
    }
}
