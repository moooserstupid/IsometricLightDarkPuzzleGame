using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GameState
{
    RUNNING,
    PAUSED,
    LOST,
    WON
}
public class GameManager : MonoBehaviour {

	public static GameManager instance;

    public LevelMetaData levelData;


    private int activatedObjectiveCount;

    private void OnEnable()
    {
        GameObjective.ObjectiveActivated += ObjectiveActivated;
        GameObjective.ObjectiveDeactivated += ObjectiveDeactivated;

    }
    private void OnDisable()
    {
        GameObjective.ObjectiveActivated -= ObjectiveActivated;
        GameObjective.ObjectiveDeactivated -= ObjectiveDeactivated;
    }
    
    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(instance);
    }
    
    public void ObjectiveActivated()
    {
        ++activatedObjectiveCount;
        if (activatedObjectiveCount >= levelData.objectiveCount)
        {
            StartCoroutine(GameWon(0.5f));
        }
    }

    public void ObjectiveDeactivated()
    {
        --activatedObjectiveCount;
    }


    IEnumerator GameWon(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (activatedObjectiveCount >= levelData.objectiveCount)
        {
            Debug.Log("GameWon");
        }
    }
}
