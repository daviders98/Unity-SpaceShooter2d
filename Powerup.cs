using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    // Update is called once per frame
    [SerializeField]
    private int powerID;
    private AudioSource _powerSound;

    private void Start()
    {
        _powerSound = GameObject.Find("Powerup_Sound").GetComponent<AudioSource>();
        if (_powerSound == null)
        {
            Debug.LogError("powerup sound is null");
        }
    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _powerSound.Play();
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                switch (powerID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.speedBoost();
                        break;
                    case 2:
                        player.ShieldsUp();
                        break;
                    default:
                        break;
                }
                
            }
            Destroy(this.gameObject);
        }
    }
}
