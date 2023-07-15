using System.Collections;
using UnityEngine;
using DG.Tweening;

public class HammerMove : MonoBehaviour
{
    Animator anim;

    [SerializeField] AudioClip acHammer;

    public void Start()
    {
        anim = GetComponent<Animator>();
        if (gameObject.transform.name =="Hammer1")
        {
            anim.Play("HammerPush1");
        }
        else
        {
            anim.Play("HammerPush2");
        }
        
    }

    private void SoundHammer()
    {
        GetComponent<AudioSource>().PlayOneShot(acHammer);
    }
}

