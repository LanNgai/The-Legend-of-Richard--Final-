using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//This Script is responsible for spawning enemies and keeping track of when they die. 
//It is also responsible for handling the win and lose events in the event of all enemies being killed or the player being killed.

public class LevelManagerScript : MonoBehaviour
{
    [Header("Enemy Spawn Settings")]
    [SerializeField] List<GameObject> enemies;
    [SerializeField] Vector3[] spawnPoints;

    [Header("Menu Scenes")]
    [SerializeField] GameObject nextLevelScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject levelMainMenu;

    
    GameObject player;
    List<GameObject> currentEnemies = new List<GameObject>{};
    bool allEnemiesSpawned;


    // Start is called before the first frame update
    void Start()
    {
        allEnemiesSpawned = false;
        if(player == null){
                //get reference to player
                player = GameObject.FindWithTag("Player");
        }
        nextLevelScreen.SetActive(false);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
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
        player.GetComponent<PlayerMovement>().gameActive = true;
        StartCoroutine(SpawnEnemies());
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


    public void EnemyDied(GameObject enemy)
    {
        //remove enemy from list of current enemies
        currentEnemies.Remove(enemy);
        Debug.Log(currentEnemies.Count);
        
        //check if all enemies are dead
        if (currentEnemies.Count == 0 && allEnemiesSpawned)
        {

            //all enemies are dead, Win State
            Debug.Log("All enemies are dead!");
            player.GetComponent<PlayerMovement>().gameActive = false;
            if(SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings){
                winScreen.SetActive(true);
            }
            else{
                nextLevelScreen.SetActive(true); 
            }
            //after delay open next level 
        }

    }

    public void PlayerDied()
    {
        //Player died, lose State
        Debug.Log("Player died!");
        foreach (GameObject enemy in currentEnemies)
        {
            //stop all enemies
            enemy.GetComponent<EnemyScript>().enabled = false;   
        }
        //stop spawning enemies
        StopCoroutine(SpawnEnemies());
        //show lose screen
        loseScreen.SetActive(true);
    }

    IEnumerator SpawnEnemies()
    {
        //wait for 3 seconds before spawning enemies
        yield return new WaitForSeconds(3f);
        int totalNumberOfEnemies = enemies.Count;
        for (int i = 0; i <= totalNumberOfEnemies; i++) //
        {
            //choose a random spawn point
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Vector3 spawnPoint = spawnPoints[randomIndex];
            
            //create an enemy at the chosen spawn point and add to list of current enemies
            GameObject enemy = Instantiate(enemies[i], spawnPoint, Quaternion.identity);
            currentEnemies.Add(enemy);
            Debug.Log(currentEnemies.Count);
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
}
