using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollider : MonoBehaviour
{
    public event Action<Item> OnCollideItem;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<ItemView>())
        {
            Item item = collision.gameObject.GetComponent<ItemView>().item;
            if (item != null)
            {
                OnCollideItem?.Invoke(item);
            }
        }
    }
}
