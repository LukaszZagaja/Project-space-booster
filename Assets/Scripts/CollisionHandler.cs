using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool IsTransitioning = false;
    bool CollisionDisabled = false;

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
            CollisionDisabled = !CollisionDisabled; // this toggles collision, changes between true and false or false and true

        }
        
    }
    void OnCollisionEnter(Collision other)
    {

        if(IsTransitioning || CollisionDisabled)
        {
            return;
        }

        switch(other.gameObject.tag)
        {
            case "Friendly":
                {
                    Debug.Log("This thing is friendly");
                    break;
                }
            case "Finish":
                {
                    StartSuccessSequence();
                    break;
                }
            default:
                {
                    StartCrashSequence();

                    break;
                }
        }
        
    }
    void StartSuccessSequence()
        {
            IsTransitioning = true;
            audioSource.Stop();
            audioSource.PlayOneShot(success);
            successParticles.Play();
            GetComponent<Movement>().enabled = false;
            Invoke(nameof(LoadNextLevel), levelLoadDelay);
        }

        void StartCrashSequence()
        {
            IsTransitioning = true;
            audioSource.Stop();
            audioSource.PlayOneShot(crash);
            crashParticles.Play();
            GetComponent<Movement>().enabled = false;
            Invoke(nameof(ReloadLevel), levelLoadDelay);
        }

        void LoadNextLevel()
        {
            IsTransitioning = false;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }
            SceneManager.LoadScene(nextSceneIndex);
        }
        void ReloadLevel()
        {
        IsTransitioning = false;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }       

}
