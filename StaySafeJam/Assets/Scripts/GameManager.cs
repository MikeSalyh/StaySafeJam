using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class GameManager : MonoBehaviour
{
  [Header("Game Balancing Vars")]

  public int startingHP = 3;
  private int _remainingHP;
  public int HP
  {
    get { return _remainingHP; }
    set { _remainingHP = value; }
  }

  public static bool active = true;
  public N95Mask n95;

  [Range(0f,1f)]
  public float difficulty = 0;
  public DifficultyVariables easy, hard;
  public AnimationCurve spawnVariance;
  public float difficultyIncrementPace = 2f;
  public float difficultyIncrementAmount = 0.1f;
  public Doctor[] doctors, ambulances;

  public float DoctorTime
  {
    get { return Mathf.Lerp(easy.docTime, hard.docTime, difficulty); }
  }
 public float AmbulanceTime
  {
    get { return Mathf.Lerp(easy.ambulanceTime, hard.ambulanceTime, difficulty); }
  }
  public float SpawnChance
  {
    get { return Mathf.Lerp(easy.spawnChance, hard.spawnChance, difficulty) * spawnVariance.Evaluate(Time.time); }
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha0))
    {
      active = !active;
      n95.active = active;
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    MetagameManager.SwitchState(MetagameManager.GameState.Gameplay);

    _remainingHP = startingHP;
    foreach (Doctor md in GameObject.FindObjectsOfType<Doctor>())
    {
      md.OnGiveMask += HandleGiveMask;
      md.OnFail += HandleFail;
    }
    StartCoroutine(SpawnGameplay());
    StartCoroutine(IncreaseDifficulty());

    active = true;
  }

  private IEnumerator IncreaseDifficulty(){
    while (true)
    {
      yield return new WaitForSeconds(difficultyIncrementPace);
      difficultyIncrementPace *= 1.5f;
      difficulty += difficultyIncrementAmount;
    }
  }

  private IEnumerator SpawnGameplay(){
    yield return new WaitForSeconds(1f);
    SpawnDoctor(doctors, DoctorTime);
    yield return new WaitForSeconds(0.25f);
    while (active)
    {
      float randomValue = Random.value;
      if (randomValue < SpawnChance / 2f)
        SpawnDoctor(ambulances, AmbulanceTime);
      if (randomValue < SpawnChance)
        SpawnDoctor(doctors, DoctorTime);

      yield return new WaitForSeconds(0.5f);
    }
  }

  private bool SpawnDoctor(Doctor[] input, float time)
  {
    Doctor[] freeDocs = input.Where(x => x.state == Doctor.State.Hidden).ToArray();
    if (freeDocs.Length > 0)
    {
      freeDocs[Random.Range(0, freeDocs.Length)].Appear(time);
      return true;
    }
    else
    {
      return false;
    }
  }



  void HandleGiveMask(Doctor md)
  {
    MetagameManager.score += md.pointValue;
  }

  void HandleFail(Doctor md)
  {
    if (md.givesPenalty)
    {
      StartCoroutine(HandleFailCoroutine(md));
    }
  }

  private IEnumerator HandleFailCoroutine(Doctor md)
  {
    StopCoroutine(SpawnGameplay());
    foreach (Doctor doc in GameObject.FindObjectsOfType<Doctor>())
    {
      if (doc != md)
      {
        doc.ForceHide();
      }
    }
    _remainingHP--;
    active = false;
    n95.active = false;
    difficulty -= 0.1f;
    yield return new WaitForSeconds(2f);
    if (_remainingHP == 0)
    {
      FinishGameplay();
    }
    else
    {
      active = true;
      n95.active = true;
      StartCoroutine(SpawnGameplay());
    }
  }

  public void FinishGameplay()
  {
    MetagameManager.GoToFinale();
  }
}
