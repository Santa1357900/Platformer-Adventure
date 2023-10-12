using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healitem : MonoBehaviour
{
    public int healthRestore = 20;
    public Vector3 spinSpeed = new Vector3(0, 180, 0);

    AudioSource itemSound;

    private void Awake()
    {
        itemSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damageable damageable = collision.GetComponent<damageable>();

        if (damageable)
        {
            bool wasHealed = damageable.Heal(healthRestore);

            if (wasHealed)
            {
                if (itemSound)
                    AudioSource.PlayClipAtPoint(itemSound.clip, gameObject.transform.position, itemSound.volume);

                Destroy(gameObject);

            }

        }
    }


    private void Update()
    {
        transform.eulerAngles += spinSpeed * Time.deltaTime;
    }

}

