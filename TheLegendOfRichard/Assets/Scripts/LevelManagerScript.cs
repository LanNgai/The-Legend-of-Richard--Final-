using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//This Script is responsible for spawning enemies and keeping track of when they die. 
//It is also responsible for handling the win and lose events in the event of all enemies being killed or the player being killed.

public class LevelManagerScript : MonoBehaviour
{
    [Header("Enemy Spawn Settings")]
    [SerializeField] List<GameObject> enemies;
    [SerializeField] Vector3[] spawnPoints;

    [Header("Pickup Settings")]
    [SerializeField] GameObject healthPickup;
    [SerializeField] GameObject powerPickup;

    [Header("Menu Scenes")]
    [SerializeField] GameObject nextLevelScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject levelMainMenu;

    [Header("HUD")]
    [SerializeField] GameObject hud;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Sprite heartEmpty; 
    [SerializeField] Sprite heartFull;
    [SerializeField] Image[] heartImages;

    [Header("Audio")]
    [SerializeField] AudioClip playerDeathSound;
    AudioSource audioSource;

    
    GameObject player;
    List<GameObject> currentEnemies = new List<GameObject>{};
    bool allEnemiesSpawned;


    // Start is called before the first frame update
    void Start(){
        allEnemiesSpawned = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        if(player == null){
                //get reference to player
                player = GameObject.FindWithTag("Player");
        }
        nextLevelScreen.SetActive(false);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        hud.SetActive(false); 
        if(SceneManager.GetActiveScene().buildIndex != 0){
            //if not in first scene, hide mainmenu and start spawning enemies immediately 
            StartGame();
        }
        else {
            //if in first scene, show main menu and wait for player to start
            levelMainMenu.SetActive(true);
        }
        
    }

    public void StartGame(){
        levelMainMenu.SetActive(false); 
        //set level text to current level
        levelText.text = "Level: " + (SceneManager.GetActiveScene().buildIndex + 1);
        hud.SetActive(true);
        player.GetComponent<PlayerMovement>().gameActive = true;
        StartCoroutine(SpawnEnemies());
        //start spawning pickups
        StartCoroutine(SpawnPickups());
    }

    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartNextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }

    public void OpenGithub() {
        Application.OpenURL("https://github.com/LanNgai/The-Legend-of-Richard--Final-");
    }


    public void EnemyDied(GameObject enemy){
        //remove enemy from list of current enemies
        currentEnemies.Remove(enemy);
        Debug.Log(currentEnemies.Count);
        
        //check if all enemies are dead
        if (currentEnemies.Count == 0 && allEnemiesSpawned)
        {
            //all enemies are dead, Win State
            Debug.Log("All enemies are dead!");
            player.GetComponent<PlayerMovement>().gameActive = false;
            StopCoroutine(SpawnPickups()); 
            if(SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings){
                winScreen.SetActive(true);
            }
            else{
                nextLevelScreen.SetActive(true); 
            }
        } 
        Destroy(enemy);

    }

    public void playerDamage(int health){
        //get heart images from array and replace with heartEmpty if index is less than health - 1
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i <= health - 1)
            {
                heartImages[i].sprite = heartFull;
            }
            else
            {
                heartImages[i].sprite = heartEmpty;
            }
        }      
        if (health <=0){
        foreach (GameObject enemy in currentEnemies)
        {
            //stop all enemy behavior
            enemy.GetComponent<EnemyScript>().enabled = false;   
        }
        //remove player character
        Destroy(player); 
        //play player death sound
        audioSource.PlayOneShot(playerDeathSound, 1.0f);
        //stop spawning enemies
        StopCoroutine(SpawnEnemies());
        StopCoroutine(SpawnPickups()); 
        //remove hud & show lose screen
        hud.SetActive(false);
        loseScreen.SetActive(true);
        }
    }

    public void playerHealed(int health){
        //get heart images from array and replace with heartFull image if index is greater than health - 1
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i <= health - 1)
            {
                heartImages[i].sprite = heartFull;
            }

        }
    }

    IEnumerator SpawnEnemies(){
        //wait for 3 seconds at the start of the level before spawning enemies
        yield return new WaitForSeconds(3f);
        int totalNumberOfEnemies = enemies.Count;
        for (int i = 0; i <= totalNumberOfEnemies; i++)
        {
            //choose a random spawn point
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Vector3 spawnPoint = spawnPoints[randomIndex];
            
            //create an enemy at the chosen spawn point and add to list of current enemies
            GameObject enemy = Instantiate(enemies[i], spawnPoint, Quaternion.identity);
            currentEnemies.Add(enemy);
            if((i + 1) == totalNumberOfEnemies){
                break;
            }
            //wait for between 2 and 3 seconds
            yield return new WaitForSeconds(Random.Range(2f, 3f));
        }
        allEnemiesSpawned = true;
        //all enemies have been spawned, finish Coroutine
        StopCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnPickups(){
        while(true){
            //choose a random spawn point within a 20 Unit radius around the center of the map 
            Vector3 center = new Vector3(0, 0, 0);
            Vector3 randomPosition = Random.insideUnitSphere * 20;
            Vector3 spawnPoint = new Vector3(center.x + randomPosition.x, 0.5f, center.z + randomPosition.z);
            
            //create a pickup at the chosen spawn point
            int randomPickup = Random.Range(0, 3);
            switch(randomPickup){
                case 0:
                    if(player.GetComponent<HealthAndDamageScript>().health < 3){
                    Instantiate(healthPickup, spawnPoint, healthPickup.transform.rotation);
                    }
                    else {
                        Instantiate(powerPickup, spawnPoint, powerPickup.transform.rotation);
                    }
                    break;
                case 1:
                    Instantiate(powerPickup, spawnPoint, powerPickup.transform.rotation);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(Random.Range(6f, 10f));
        }
    }
}
