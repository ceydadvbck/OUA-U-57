using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class DestroyInteractable : InteractableBase   // Anahtar almak i�in kullan�r�z
    {

        public override void OnInteract()
        {
            base.OnInteract();

            Destroy(gameObject);
        }
    }
}