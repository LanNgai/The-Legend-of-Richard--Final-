using UnityEngine;


//This Script is responsible for keeping track of a GameObjects health and, 
//in the event of receiving damage, reducing the health or, in the event of healing, increasing the health
public class HealthAndDamageScript : MonoBehaviour
{
    [SerializeField] int health; // Health of the GameObject
    LevelManagerScript levelManager; //reference to levelmanager, will be called when gameobject is "dead"

    void Start()
    {
        // get reference to level manager
        levelManager = FindObjectOfType<LevelManagerScript>();
    }

    // method to receive damage, will be called when enemy is shot by player or player is damaged by enemy
    public void Damage(int damageReceived, GameObject damageDealer){
        health -= damageReceived;
        if(gameObject.tag == "Player"){
            Debug.Log("Player damage received" + " Player Health: " + health);
            //if player is hit by enemy deal knockback in opposite direction from enemy position
            Vector3 knockbackDirection = (transform.position - damageDealer.transform.position).normalized;
            Rigidbody playerRb = gameObject.GetComponent<Rigidbody>();
            playerRb.AddForce(knockbackDirection * 20);
        }
        else if (gameObject.tag == "Enemy"){
            Debug.Log("Enemy damage received" + " Enemy Health: " + health);
        }
        if(health <= 0){

            if(gameObject.tag == "Enemy" || gameObject.tag == "EnemyTrash"){
                //if is enemy, call LevelManager enemy died method 
                levelManager.EnemyDied(this.gameObject);
            }
            else if(gameObject.tag == "Player"){
                // if is player, call LevelManager player died method
                levelManager.PlayerDied();
            }
            // destroy the game object
            Destroy(gameObject);
        }
    }
    
}
