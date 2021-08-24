using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : Singleton<LevelLoader>
{   
    private void Awake(){
        GetComponent<AudioSource>().Play();
    }

    public void LoadLevel(int index){
        StartCoroutine(LoadScene(index));
    }

    IEnumerator LoadScene(int index){
        // wait for transition
        yield return new WaitForSeconds(1);

        // load the scene
        SceneManager.LoadScene(index);
    }

    public void QuitGame(){
        Application.Quit();
    }
}