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
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) return;

        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        
        //find rotation for enemy to look at player
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        
        if (Vector3.Distance(player.transform.position, transform.position) > attackRange)
        {
            //set rotation of enemy toward player
            transform.rotation = lookRotation;  
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }else if (Vector3.Distance(player.transform.position, transform.position) <= attackRange && !hasattacked)
        {
            transform.rotation = lookRotation;
            enemyRb.AddForce(lookDirection * boost, ForceMode.Impulse);
            if(hurtbox)hurtbox.enabled = true;
            hasattacked = true;
            StartCoroutine(DelayAttack()); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if(hurtbox)hurtbox.enabled = false;
            //stop movement
            enemyRb.velocity = Vector3.zero;
            //deal damage to player
            HealthAndDamageScript handdscript = other.gameObject.GetComponent<HealthAndDamageScript>();
            if(handdscript != null) handdscript.Damage(1, this.gameObject);
            
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
