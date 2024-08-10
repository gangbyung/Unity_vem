using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] LockCharacter;
    public GameObject[] unLockCharacter;
    public GameObject uiNotice;
    enum Achive { UnLockPotato, UnLockBean }
    Achive[] achives;

    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }

    }

    void Start()
    {
        UnLockCharacter();
    }

    void UnLockCharacter()
    {
        for (int index = 0; index < LockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnLock = PlayerPrefs.GetInt(achiveName) == 1;
            LockCharacter[index].SetActive(!isUnLock);
            unLockCharacter[index].SetActive(isUnLock);
        }
    }

    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            ChecjAchive(achive);
        }
    }

    void ChecjAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.UnLockPotato:
                isAchive = GameManager.Instance.kill >= 10;
                break;
            case Achive.UnLockBean:
                isAchive = GameManager.Instance.gameTime == GameManager.Instance.maxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

        AudioManager.Instance.PlaySfx(AudioManager.Sfx.LecelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
