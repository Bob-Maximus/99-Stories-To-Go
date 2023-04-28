using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Eflatun.SceneReference;

public class EnterableThings : MonoBehaviour
{
    public Button enterButton;
    public LevelData levelData;

    public void SwichScene(Collider col)
    {
        for (int i = 0; i < levelData.changeLevelData.Count; i++)
        {
            if (col.gameObject.name == levelData.changeLevelData[i].changeLevelTrigger.name)
            {
                if (levelData.changeLevelData[i].needsInput == false)
                {
                    SceneManager.LoadScene(levelData.changeLevelData[i].newLevel.ToString());
                    GameObject.Find("Player").transform.position = levelData.changeLevelData[i].spawnPoint;
                } else
                {
                    enterButton.gameObject.SetActive(true);
                    enterButton.onClick.AddListener(delegate { swichScene(i); });
                }
            } 
        }
    }

    void  swichScene(int i)
    {
        Scene newScene = SceneManager.GetSceneByName(levelData.changeLevelData[i - 1].newLevel.Name.ToString());
        SceneManager.MoveGameObjectToScene(GameObject.Find("Player"), newScene);
        SceneManager.LoadScene(levelData.changeLevelData[i - 1].newLevel.ToString());
    }
}
