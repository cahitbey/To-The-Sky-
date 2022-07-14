using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;
    [SerializeField] float nextLevetDelay = 0.5f;
    [SerializeField] float crashDelay = 0.4f;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;


    AudioSource audioSource;
    bool isTransitioning = false;
    bool collisionDisable = false;

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();       
    }
    void Update() 
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable; //This will toggle collision
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning || collisionDisable)    {return;}

        switch (other.gameObject.tag)
        {
            case "Friendly":
            Debug.Log("hit a friendly");
            break;
            case "Finish":
            NextLevelSequence();
            break;
            default:
            StartCrashSequence();
            break;
        }

    }
    void NextLevelSequence()
        {
            isTransitioning = true;
            audioSource.Stop();
            audioSource.PlayOneShot(success);
            successParticles.Play();
            GetComponent<Movement>().enabled = false;
            Invoke("LoadNextLevel", nextLevetDelay);
        }
    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", crashDelay);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    
}