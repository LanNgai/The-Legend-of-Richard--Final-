using UnityEngine;

public class Hairball : MonoBehaviour
{
    public float speed = 40;
    public int damageAmount;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        HealthAndDamageScript healthAndDamageScript = other.gameObject.GetComponent<HealthAndDamageScript>();
        
        if (healthAndDamageScript != null)
        {
            //make enemy take damage
            healthAndDamageScript.Damage(damageAmount, this.gameObject);
            //destroy projectile upon hitting enemy
        }
            Destroy(gameObject);
    }
}
