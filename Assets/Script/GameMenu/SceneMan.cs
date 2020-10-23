using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private string sceneToLoadName;
    //We declare the prefab "Loading Screen";

public void LoadScene()
{
    StartCoroutine(Load());
//We're calling our Corountine function in order to use it;
}

private IEnumerator Load()
{
    var loadingScreenInstance = Instantiate(loadingScreen);
    var loadingAnimator = loadingScreenInstance.GetComponent<Animator>();
    //We're declaring a loadingAnimator variable that will contain our GameObject's Animator;

    var animationTime = loadingAnimator.GetCurrentAnimatorStateInfo(0).length;
    //Getting the duration of our animations;
    

    DontDestroyOnLoad(loadingScreenInstance);
    var loading = SceneManager.LoadSceneAsync(sceneToLoadName);
    //Will open the scene once it is fully charged;

    loading.allowSceneActivation = false;

    while (loading.progress < 0.1f)
    {
        yield return new WaitForSeconds(animationTime);
        //For each loading frame, we're going to wait our animation's duration;
    }

    loading.allowSceneActivation = true;
    //We're loading the Scene;
    loadingAnimator.SetTrigger("EndLoading");
    //We trigger the Animator's parameter "EndLoading";
    //The Parameter "EndLoading" is enabling the transition from our "Appearing" animation to the "Disappearing" one;
}

public void ExitGame()
{
    Application.Quit();
    //Quits the game;
    Debug.Log("Game closed.");
    //We're using a Debug.Log here to print "Game closed.";
}


}
