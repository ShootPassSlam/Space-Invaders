using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
  public GameObject enemyLaserPrefab;
  public int health;
  public float enemyProjectileSpeed;
  public float shotsPerSeconds=0.5f;
  public int scoreValue=100;
  private ScoreKeeper scoreKeeper;
  public AudioClip fireSound;
  public AudioClip deathSound;

  void Start(){
    scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
  }

  void OnTriggerEnter2D(Collider2D col){
    Projectile missile = col.gameObject.GetComponent<Projectile>();
    if(missile){
      health -= missile.GetDamage();
      missile.Hit();
      if (health <= 0){
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        Destroy(gameObject);
        scoreKeeper.Score(scoreValue);
      } 
    }
  }

  void Update(){
    float probability = shotsPerSeconds*Time.deltaTime;
    if(Random.value < probability){
      Fire();
    } 
  }
  void Fire(){
    GameObject laser = Instantiate(enemyLaserPrefab,transform.position,Quaternion.identity) as GameObject;
    laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0,-enemyProjectileSpeed,0);
    AudioSource.PlayClipAtPoint(fireSound, transform.position);
  }
}
