using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    AudioSource audioSource;

    bool IsTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {

        if(IsTransitioning)
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
            // todo add particle effect on success
            GetComponent<Movement>().enabled = false;
            Invoke(nameof(LoadNextLevel), levelLoadDelay);
        }

        void StartCrashSequence()
        {
            IsTransitioning = true;
            audioSource.Stop();
            audioSource.PlayOneShot(crash);
            // todo add particle effect on crash
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
