using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject warriorPrefab;
    public GameObject wizardPrefab;

    void Start()
    {
        Spawn(PlayerClass.Wizard);
    }

    public enum PlayerClass
    {
        Wizard,
        Warrior
    }

    public void Spawn(PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case PlayerClass.Warrior:
                Instantiate(warriorPrefab);
                break;
            case PlayerClass.Wizard:
                Instantiate(wizardPrefab);
                break;
        }
    }
}
