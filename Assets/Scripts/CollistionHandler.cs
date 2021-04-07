using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollistionHandler : MonoBehaviour
{
    //l - nex lvl - c off collisons

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip defeatSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem defeatParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false; // żeby audioclip nie grał kilka razy jak się zniszczymy
    bool disableCollison = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning) { return; } // return - dont go any further in this method, just return that means whole switch wont be executed

        switch (collision.gameObject.tag) //gameObject znajduję się w Hierarchy
        {
            case "Friendly":
                Debug.Log("Uderzyliśmy w Friendly");           
                break;
            case "Finish":
                    StartSuccessSequence(); // robimy to metodą żeby potem móc tam dodać sporo rzeczy - tutaj ma być porządek
                break;
            default:
                    StartCrashSequence();
                break;
        }
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        PressLtoLoadNextLevel();
        PressCtoDisableCollision();
    }

    private void PressCtoDisableCollision()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            disableCollison = !disableCollison; //disableCollison = true; - to tylko wyłącza - a tym co mamy wpisane możemy zmieniać między wyłączonym a włączonym
        }
    }

    private void PressLtoLoadNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    void StartSuccessSequence()
    {       
        isTransitioning = true;
        audioSource.Stop(); //tutaj dzwięk rakiety jak acceleruje był
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;     
        Invoke("LoadNextLevel", levelLoadDelay);
        
    }

    void StartCrashSequence()
    {
        if (disableCollison) { return; }
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(defeatSound);
        defeatParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void LoadNextLevel()
    {
        // jeżeli przejdziemy ostatni poziom to wracamy do pierwszego poziomu
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) // liczymy ile mamy scen
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        //clear clean code habit
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
