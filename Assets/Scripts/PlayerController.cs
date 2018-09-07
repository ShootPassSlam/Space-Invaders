using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public GameObject laserPrefab;
  float farLeft, farRight;
  float padding = 0.5f;
  public float speed;
  public float projectileSpeed;
  public float firingRate;
  public int health;
  public AudioClip fireSound;

  void Start(){
    float distance = transform.position.z - Camera.main.transform.position.z;
    Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
    Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
    farLeft = leftmost.x + padding;
    farRight = rightmost.x - padding;
  }  

  void Update(){
    if(Input.GetKey(KeyCode.LeftArrow)){
      transform.position += Vector3.left * speed * Time.deltaTime; 
    }else if(Input.GetKey(KeyCode.RightArrow)){
      transform.position += Vector3.right * speed * Time.deltaTime; 
    }
    if(Input.GetKeyDown(KeyCode.Space)){
      InvokeRepeating("Fire", 0.000001f, firingRate);
    }
    if(Input.GetKeyUp(KeyCode.Space)){
      CancelInvoke("Fire");
    }
  //restrict player to playspace
  float newX = Mathf.Clamp(transform.position.x, farLeft, farRight);
  transform.position = new Vector3(newX, transform.position.y, transform.position.z);
  }
  
  void Fire(){
    GameObject laser = Instantiate(laserPrefab,transform.position,Quaternion.identity) as GameObject;
    laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0,projectileSpeed,0);
    AudioSource.PlayClipAtPoint(fireSound, transform.position);
  }
  
  void OnTriggerEnter2D(Collider2D col){
    Projectile missile = col.gameObject.GetComponent<Projectile>();
    if(missile){
      health -= missile.GetDamage();
      missile.Hit();
      if (health <= 0){
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
        Destroy(gameObject);
      } 
    }
  }

}