using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject projectilePrefab;
    private float fireRate = 0.5f;
    private float nextFire = 0;
    private float horizontalInput;
    private float verticalInput;
    public float speed;

    Camera cam; 
    Ray ray;
    RaycastHit hit;
    Vector3 Hitpoint = Vector3.zero;

    public bool gameActive = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        if(gameActive){
            // Move the player based on the horizontal and vertical inputs
            transform.rotation = Quaternion.identity; //zeroing rotation before movement so that we dont move toward the mouse position
            transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime);
            transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        
            if(cam){
                ray = cam.ScreenPointToRay(Input.mousePosition); //creating a ray from where the mouse is on the screen into the game world

                    //if shooting the ray results in a hit...
                    if (Physics.Raycast(ray, out hit)){

                        Hitpoint = hit.point; //getting the point of intersection of the raycast and what it hits 
                        transform.LookAt(new Vector3(Hitpoint.x, transform.position.y, Hitpoint.z)); //rotate the player to face in the direction of the hitpoint, keeping its y rotate so it doesnt aim down or up.
                    }
            }
        
            // On mouse press, if enough time has passed since last fire, shoot hairball in the direction the player is facing
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;// reset nextFire to current time + fireRate
                Vector3 projectileSpawnPosition = transform.position + (transform.forward * 3);
                Instantiate(projectilePrefab, projectileSpawnPosition, transform.rotation);
            }
        }
    }
}
