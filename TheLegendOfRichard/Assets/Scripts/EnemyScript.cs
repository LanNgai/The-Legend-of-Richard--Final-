using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject player;

    [SerializeField] Collider hurtbox;

    private float attackRange = 6.0f;
    private float boost = 20.0f;
    private bool hasattacked = false;

    
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        if(hurtbox)hurtbox.enabled = false;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) return;

        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        
        //find rotation for enemy to look at player
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        
        if(!gameObject.CompareTag("EnemyTrash")) {
            if (Vector3.Distance(player.transform.position, transform.position) > attackRange)
            {
                //set rotation of enemy toward player
                transform.rotation = lookRotation;  
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

            }else if (Vector3.Distance(player.transform.position, transform.position) <= attackRange && !hasattacked)
            {
                if(hurtbox)hurtbox.enabled = true;
                transform.rotation = lookRotation;
                enemyRb.AddForce(lookDirection * boost, ForceMode.Impulse);
                hasattacked = true;
                StartCoroutine(DelayAttack()); 
            }
        }
    }

    void FixedUpdate() {
        if (player == null) return; 

        if(gameObject.CompareTag("EnemyTrash")){
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();

            // Apply force towards player
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            rb.AddForce(directionToPlayer * 10f);

            // Apply random torque for tumbling
            Vector3 randomTorque = new Vector3(Random.value, Random.value, Random.value) * 10f;
            rb.AddTorque(randomTorque);  
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !gameObject.CompareTag("EnemyTrash"))
        {
            if(hurtbox)hurtbox.enabled = false;
            Debug.Log(hurtbox + " " + hurtbox.enabled + " " + other);
            //stop movement
            enemyRb.velocity = Vector3.zero;
            //deal damage to player
            HealthAndDamageScript handdscript = other.gameObject.GetComponent<HealthAndDamageScript>();
            if(handdscript != null) handdscript.Damage(1, this.gameObject);
            
        }
    }

    private void OnCollisionEnter(Collision other) {
            if (other.gameObject.name == "Player" && gameObject.CompareTag("EnemyTrash") && !hasattacked)
            {
                enemyRb.velocity = Vector3.zero;
                HealthAndDamageScript handdscript = other.gameObject.GetComponent<HealthAndDamageScript>();
                if(handdscript != null) handdscript.Damage(1, this.gameObject);
                StartCoroutine(DelayAttack()); 
            }
    }
    
    //coroutine for attack cooldown
    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(3.0f);      
        if (hurtbox && hurtbox.enabled){
            hurtbox.enabled = false;  //turn off hurtbox when attack is done //this will prevent the hurtbox from staying active while the enemy is not attacking.
        }
        hasattacked = false;
    }
}
