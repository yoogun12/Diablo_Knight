using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.Instance.player.input;
        float dirX = playerDir.x < 0 ? -1f : 1f;
        float dirY = playerDir.y < 0 ? -1f : 1f;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 25.6f);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 25.6f);
                }


                break;


            case "Enemy":
                if (coll.enabled)
                {
                    transform.Translate(playerDir *  25.6f + new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f),0f));
                }
                break;
        }


    }
}
