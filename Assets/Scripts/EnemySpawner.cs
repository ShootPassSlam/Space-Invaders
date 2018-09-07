using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
  public GameObject enemyPrefab;
	public float width;
  public float height;
  public float speed=2f;
  public float spawnDelay = 0.5f;

  private bool movingRight = false;
  private float xmax, xmin;


  // Use this for initialization
	void Start () {
    float distance = transform.position.z - Camera.main.transform.position.z;
    Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
    Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
    xmax = rightmost.x;
    xmin = leftmost.x;
    SpawnUntilFull();
	}
	
  public void OnDrawGizmos(){
    Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
  }

	// Update is called once per frame
	void Update () {
    if(movingRight){
      transform.position += Vector3.right * speed * Time.deltaTime;  
      CheckDirection();
    }else{
      transform.position += Vector3.left * speed * Time.deltaTime; 
      CheckDirection();
    }
    if(AllMembersDead()){
      SpawnUntilFull();
    } 
	}

  void CheckDirection(){
    float rightEdgeOfFormation = transform.position.x + (0.5f*width);
    float leftEdgeOfFormation = transform.position.x - (0.5f*width);
    if(leftEdgeOfFormation < xmin){
      movingRight = true;
    }else if(rightEdgeOfFormation > xmax){
      movingRight = false;
    }
  }

  bool AllMembersDead(){
    foreach(Transform childPositionGameObject in transform){
      if(childPositionGameObject.childCount>0){
        return false;
      }
    }
    return true;
  }

  Transform NextFreePosition(){
    foreach(Transform childPositionGameObject in transform){
      if(childPositionGameObject.childCount==0){
        return childPositionGameObject;
      }
    }
    return null;  
  }
  void SpawnUntilFull(){
    Transform freePosition = NextFreePosition();
    if(freePosition){
      GameObject enemy = Instantiate(enemyPrefab,freePosition.position,Quaternion.identity) as GameObject;
      enemy.transform.parent = freePosition;
    }
    if(NextFreePosition()){  
      Invoke("SpawnUntilFull", spawnDelay);
    }
  } 
}