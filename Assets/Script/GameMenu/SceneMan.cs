using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private string sceneToLoadName;
    //We declare the prefab "Loading Screen";
    //We declare the name of the scene we will load, here our GameScene;
    //Both of these variables will be;

public void LoadScene()
{
    StartCoroutine(Load());
//We're calling our Corountine function in order to use it;
}

private IEnumerator Load()
{
    var loadingScreenInstance = Instantiate(loadingScreen);
    //Contains the Prefab;
    var loadingAnimator = loadingScreenInstance.GetComponent<Animator>();
    //We're declaring a loadingAnimator variable that will contain our GameObject's Animator;

    var animationTime = loadingAnimator.GetCurrentAnimatorStateInfo(0).length;
    //Getting the duration of our animations;
    

    DontDestroyOnLoad(loadingScreenInstance);
    var loading = SceneManager.LoadSceneAsync(sceneToLoadName);
    //Will open the scene once it is fully charged;

    loading.allowSceneActivation = false;
    //We don't automatically load the scene when it's fully charged;

    while (loading.progress < 0.9f)
    {//We're cheking every frame till the scene is completely loaded;
        yield return new WaitForSeconds(animationTime);
        //Yield = waiting;
        //For each loading frame, we're going to wait our animation's duration;
    }

    loading.allowSceneActivation = true;
    //We finally load the Scene;
    loadingAnimator.SetTrigger("EndLoading");
    //We trigger the Animator's parameter "EndLoading";
    //The Parameter "EndLoading" is enabling the transition from our "Appearing" animation to the "Disappearing" one;
}

public void ExitGame()
{
    Application.Quit();
    //Quits the game;
    Debug.Log("Game closed.");
    //We're using a Debug.Log here to print "Game closed." since we can't check in Unity if the game is closed;
}


}
