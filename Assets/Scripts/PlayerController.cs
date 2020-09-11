using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float pullForce = 100f;
    public float rotateSpeed = 360f;
    private GameObject closestTower;
    private GameObject closestReverseTower;
    private GameObject hookedTower;
    private GameObject hookedReverseTower;
    private bool isPulled = false;
    private UIController uiControl;
    private AudioSource myAudio;
    private bool isCrashed = false;
    private Vector2 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        uiControl = GameObject.Find("Canvas").GetComponent<UIController>();
        myAudio = GetComponent<AudioSource>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z) && !isPulled)
        {
            if (closestTower != null && hookedTower == null)
            {
                hookedTower = closestTower;
            }
            else if(closestReverseTower != null && hookedReverseTower == null)
            {
                hookedReverseTower = closestReverseTower;
            }

            if (hookedTower)
            {
                float distance = Vector2.Distance(transform.position, hookedTower.transform.position);
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;
                float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
                rb.AddForce(pullDirection * newPullForce);
                rb.angularVelocity = -rotateSpeed / distance;

                isPulled = true;
            }
            else if(hookedReverseTower)
            {
                float distance = Vector2.Distance(transform.position, hookedReverseTower.transform.position);
                Vector3 pullDirection = (hookedReverseTower.transform.position - transform.position).normalized;
                float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
                rb.AddForce(pullDirection * newPullForce);
                rb.angularVelocity = rotateSpeed / distance;

                isPulled = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            rb.angularVelocity = 0;
            isPulled = false;
            hookedTower = null;
        }

        if (isCrashed)
        {
            if (!myAudio.isPlaying)
            {
                RestartPosition();
            }
        }
        else
        {
            rb.velocity = -transform.up * moveSpeed;
        }
    }

    public void Pull(Tower targetTower, bool reverseTower)
    {
        if (reverseTower)
        {
            float distance = Vector2.Distance(transform.position, targetTower.transform.position);
            Vector3 pullDirection = (targetTower.transform.position - transform.position).normalized;
            float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
            rb.AddForce(pullDirection * newPullForce);
            rb.angularVelocity = rotateSpeed / distance;
        }
		else
		{
			float distance = Vector2.Distance(transform.position, targetTower.transform.position);
            Vector3 pullDirection = (targetTower.transform.position - transform.position).normalized;
            float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
            rb.AddForce(pullDirection * newPullForce);
            rb.angularVelocity = -rotateSpeed / distance;
		}
    }

    public void StopPulling()
    {
        rb.angularVelocity = 0;
    }

    public void RestartPosition()
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(0, 0, 90);
        isCrashed = false;

        if (closestTower)
        {
            closestTower.GetComponent<SpriteRenderer>().color = Color.white;
            closestTower = null;
        }
        else if(closestReverseTower)
        {
            closestReverseTower.GetComponent<SpriteRenderer>().color = Color.white;
            closestReverseTower = null;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (!isCrashed)
            {
                myAudio.Play();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0;
                isCrashed = true;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            uiControl.EndGame();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            closestTower = collision.gameObject;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (collision.gameObject.tag == "ReverseTower")
        {
            closestReverseTower = collision.gameObject;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            closestTower = null;
            hookedTower = null;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (collision.gameObject.tag == "ReverseTower")
        {
            closestReverseTower = null;
            hookedReverseTower = null;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
