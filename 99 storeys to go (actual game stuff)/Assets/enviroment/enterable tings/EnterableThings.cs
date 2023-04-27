using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterableThings : MonoBehaviour
{
    public LevelData levelData;

    public void SwichScene(Collider col)
    {
        for (int i = 0; i < levelData.changeLevelData.Count; i++)
        {
            if (col.gameObject.name == levelData.changeLevelData[i].changeLevelTrigger.name)
            {
                Debug.Log("hh");
                SceneManager.LoadScene(levelData.changeLevelData[i].newLevel.Name.ToString());
                GameObject.Find("Player").transform.position = levelData.changeLevelData[i].spawnPoint;
            }
        }
    }
}
