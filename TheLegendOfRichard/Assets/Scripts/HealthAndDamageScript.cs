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
            levelManager.playerDamage(health);
        }
        else {
            Debug.Log("Enemy damage received" + " Enemy Health: " + health);
            if(health <= 0){
                //if is enemy, call LevelManager enemy died method 
                levelManager.EnemyDied(gameObject);
            }
        }
    }

    public void Heal() {
        health++;
        levelManager.playerHealed(health);
    }

    //
    private void OnTriggerEnter(Collider other) {
        if(gameObject.CompareTag("Player")){
            
            if(other.gameObject.CompareTag("HealthPickup")){
                HealthAndDamageScript hdScript = gameObject.GetComponent<HealthAndDamageScript>();
                if(hdScript.health < 3){
                    hdScript.Heal();
                    Destroy(other.gameObject); // destroy health pickup when picked up
                }
            }
            else if(other.gameObject.CompareTag("PowerPickup")){
                // give player temporary powerup
                PlayerMovement playerScript = gameObject.GetComponent<PlayerMovement>();
                if (!playerScript.powered){
                    Debug.Log("Powerup received");
                    playerScript.Powerup();
                    Destroy(other.gameObject); // destroy powerup when picked up
                }
            }
        }
    }
    
}
