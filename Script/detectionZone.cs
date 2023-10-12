using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class detectionZone : MonoBehaviour
{
    public UnityEvent noColliderRemain;
    
    public List<Collider2D> detectionCollider = new List<Collider2D>();
    Collider2D col;

    public void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectionCollider.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectionCollider.Remove(collision);

        if(detectionCollider.Count <= 0 ) 
        {
            noColliderRemain.Invoke();
        }
    }

}
