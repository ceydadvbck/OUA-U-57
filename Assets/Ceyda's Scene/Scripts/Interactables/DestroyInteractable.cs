using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class DestroyInteractable : InteractableBase   // Anahtar almak için kullanýrýz
    {

        public override void OnInteract()
        {
            base.OnInteract();

            Destroy(gameObject);
        }
    }
}