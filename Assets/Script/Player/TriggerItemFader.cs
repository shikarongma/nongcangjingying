using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFader[] faders = other.GetComponentsInChildren<ItemFader>();

        if (faders != null)
        {
            foreach (var item in faders)
            {
                item.FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemFader[] faders = other.GetComponentsInChildren<ItemFader>();

        if (faders != null)
        {
            foreach(var item in faders)
            {
                item.FadeIn();
            }
        }
    }
}
