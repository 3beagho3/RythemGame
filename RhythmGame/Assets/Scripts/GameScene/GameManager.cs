using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP, RIGHT, DOWN, LEFT
}
public class GameManager : MonoBehaviour
{
    public AudioSource audioSource;

    public class MusicSync
    {
        public float time;
        public string direction;

        public MusicSync(float _time, string _direction)
        {
            time = _time;
            direction = _direction;
        }
    }
    public MusicSync[] musicSync;

    public float startWaitTime = 0;

    private int currentSync = 0;

    public float judgementScale = 2.5f;
    public GameObject judgementPointObj;
    public class JudgementPoint
    {
        public GameObject obj;
        public int direction;

        public JudgementPoint(GameObject _obj, int _direction)
        {
            obj = _obj;
            direction = _direction;
        }
    }
    private List<JudgementPoint> judgementPoints = new List<JudgementPoint>();
    private JudgementPoint tempjudgementPoint;

    public TextAsset musicSyncText;
    private string[] textLine;

    [Header("판정 범위")]
    public float perfectRange;
    public float GoodRange;
    public float NormalRange;

    void Start()
    {
        textLine = musicSyncText.text.Split('\n');
        musicSync = new MusicSync[textLine.Length];

        for (int i = 0; i < textLine.Length; i++)
        {
            musicSync[i] = new MusicSync(0, string.Empty);

            string[] tempText = textLine[i].Split(',');

            musicSync[i].time = float.Parse(tempText[0]);

            //string 뒷부분에 null값이 자동으로 들어가기 때문에 마지막 null 삭제
            musicSync[i].direction = tempText[1].Substring(0, tempText[1].Length - 1);
            //string 마지막은 null이 생기지 않기때문에 예외처리  /  \n 예외처리
            for (int enumAmount = 0; enumAmount < 4; enumAmount++)
            {
                Direction tempDirection = (Direction)enumAmount;
                if (tempText[1].Length.Equals(tempDirection.ToString().Length) || i.Equals(textLine.Length - 1))
                {
                    musicSync[i].direction = tempText[1];
                    break;
                }
            }
        }

        StartCoroutine(MusicStart());
        StartCoroutine(MusicSyncSet());
    }

    IEnumerator MusicStart()
    {
        yield return new WaitForSeconds(startWaitTime);
        audioSource.Play();
    }

    IEnumerator MusicSyncSet()
    {
        while (currentSync < textLine.Length)
        {
            float waitTime = 0;
            if (!(currentSync - 1 < 0))
                waitTime = musicSync[currentSync].time - musicSync[currentSync - 1].time;
            else
                waitTime = musicSync[currentSync].time;

            yield return new WaitForSeconds(waitTime);

            CreateJudgement();

            currentSync++;
        }
    }

    void CreateJudgement()
    {
        int direction = (int)(Direction)System.Enum.Parse(typeof(Direction), musicSync[currentSync].direction);
        GameObject directionObj = GameObject.FindGameObjectWithTag(musicSync[currentSync].direction);

        GameObject tempObj;
        tempObj = Instantiate(judgementPointObj, directionObj.transform.position, Quaternion.identity);
        tempObj.transform.localScale = Vector2.one * judgementScale;
        judgementPoints.Add(new JudgementPoint(tempObj, direction));
    }

    void FixedUpdate()
    {
        foreach (JudgementPoint judgementPoint in judgementPoints)
        {
            if (judgementPoint == null || judgementPoint.obj == null) continue;

            judgementPoint.obj.transform.localScale -= Vector3.one * Time.deltaTime;
            if (judgementPoint.obj.transform.localScale.x <= 0.0f)
            {
                tempjudgementPoint = judgementPoint;
                Destroy(judgementPoint.obj);
                break;
            }
        }
        if (tempjudgementPoint != null)
            judgementPoints.Remove(tempjudgementPoint);
    }

    public void JudgementCheck(int direction)
    {
        foreach (JudgementPoint judgementPoint in judgementPoints)
        {
            if (judgementPoint == null) continue;

            if (judgementPoint.direction.Equals(direction))
            {
                tempjudgementPoint = judgementPoint;
                if(tempjudgementPoint.obj.transform.localScale.x <= 1f - perfectRange || 
                   tempjudgementPoint.obj.transform.localScale.x <= 1f + perfectRange)
                {
                    FindObjectOfType<ScoreManager>().ScoreUp("Perfect");
                }
                else if (tempjudgementPoint.obj.transform.localScale.x <= 1f - GoodRange || 
                         tempjudgementPoint.obj.transform.localScale.x <= 1f + GoodRange)
                {
                    FindObjectOfType<ScoreManager>().ScoreUp("Good");
                }
                else if (tempjudgementPoint.obj.transform.localScale.x <= 1f - NormalRange || 
                         tempjudgementPoint.obj.transform.localScale.x <= 1f + NormalRange)
                {
                    FindObjectOfType<ScoreManager>().ScoreUp("Normal");
                }
                else
                {
                    FindObjectOfType<ScoreManager>().ScoreUp("Bad");
                }
                Destroy(judgementPoint.obj);
                break;
            }
        }
        if (tempjudgementPoint != null)
            judgementPoints.Remove(tempjudgementPoint);
    }
}
