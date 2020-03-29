using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Doctor : MonoBehaviour
{
  public enum State
  {
    Hidden,
    Ready,
    Happy,
    Sad
  }

  public State state;
  public Sprite waitingSprite, happySprite, sadSprite;
  public GameObject door;
  public Image doctorImage;

  public float hiddenX, shownX;

  private float patienceMax;
  private float patienceRemaining;
  public Slider patienceSlider;

  public int pointValue = 100;
  public bool givesPenalty = true;

  //Doctor delegates
  public delegate void GiveMaskDelegate(Doctor md);
  public GiveMaskDelegate OnGiveMask;
  public delegate void FailDelegate(Doctor md);
  public FailDelegate OnFail;

  protected AudioSource src;
  public AudioClip sadSound, appearSound;
  public AudioClip[] happySounds;
  private Vector3 defaultDrScale;

  // Start is called before the first frame update
  void Start()
  {
    state = State.Hidden;
    src = GetComponent<AudioSource>();
    defaultDrScale = doctorImage.transform.localScale;

    if (door != null)
      door.gameObject.SetActive(true);

    if (patienceSlider != null)
      patienceSlider.GetComponent<CanvasGroup>().alpha = 0f;
    EnterBuilding(0f);
  }


  public KeyCode debugKey;
  private void Update()
  {
    if (state == State.Ready)
    {
      if(patienceSlider != null)
        patienceSlider.value = patienceRemaining / patienceMax;
    }

    //DEBUG
    if (Input.GetKeyDown(debugKey))
    {
        DebugAppear();
    }
  }

  virtual protected void DebugAppear()
  {
    Appear(2f);
  }

  virtual public bool Appear(float time)
  {
    if (state != State.Hidden)
      return false;

    patienceMax = time;
    patienceRemaining = time;

    src.PlayOneShot(appearSound);

    state = State.Ready;
    ExitBuilding(0.5f);
    if (door != null)
      door.gameObject.SetActive(false);
    doctorImage.sprite = waitingSprite;
    StartCoroutine(AwaitMaskCoroutine());

    if (patienceSlider != null)
      patienceSlider.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
    return true;
  }

  IEnumerator AwaitMaskCoroutine()
  {
    while (patienceRemaining > 0f)
    {
      patienceRemaining -= 0.05f;
      yield return new WaitForSeconds(0.05f);
    }

    //The player has failed to give this doctor a mask
    if (state == State.Ready)
    {
      if (OnFail != null)
        OnFail(this);

      src.PlayOneShot(sadSound);

      state = State.Sad;
      if (patienceSlider != null)
        patienceSlider.GetComponent<CanvasGroup>().DOFade(0f, 0.25f);
      doctorImage.sprite = sadSprite;
      yield return HandleMissBlinking();
    }

    yield return GoInside();
  }

  protected virtual IEnumerator HandleMissBlinking()
  {
    for (int i = 0; i < 8; i++)
    {
      doctorImage.enabled = !doctorImage.enabled;
      yield return new WaitForSeconds(0.2f);
    }
  }

  protected virtual IEnumerator GoInside()
  {
    yield return new WaitForSeconds(0.35f);
    EnterBuilding(0.5f);
    yield return new WaitForSeconds(0.5f);
    state = State.Hidden;

    if (door != null)
      door.gameObject.SetActive(true);
  }

  public void GiveMask()
  {
    if (state != State.Ready)
      return;

    state = State.Happy;

    if (patienceSlider != null)
    {
      patienceSlider.GetComponent<CanvasGroup>().DOKill();
      patienceSlider.GetComponent<CanvasGroup>().DOFade(0f, 0.25f);
    }
    doctorImage.sprite = happySprite;
    patienceRemaining = 0;

    src.PlayOneShot(happySounds[Random.Range(0, happySounds.Length)]);
    doctorImage.transform.localScale = defaultDrScale * 1.25f;
    doctorImage.transform.DOScale(defaultDrScale, 0.25f).SetEase(Ease.OutSine);

    if (OnGiveMask != null)
      OnGiveMask(this);
  }

  protected virtual void EnterBuilding(float duration)
  {
    doctorImage.transform.DOLocalMoveX(hiddenX, duration);
  }

  protected virtual void ExitBuilding(float duration)
  {
    doctorImage.transform.DOLocalMoveX(shownX, duration);
  }

  public virtual void ForceHide()
  {
    StopAllCoroutines();
    doctorImage.transform.DOKill();
    state = State.Hidden;
    EnterBuilding(0f);
    if (patienceSlider != null)
      patienceSlider.GetComponent<CanvasGroup>().alpha = 0f;
    if (door != null)
      door.gameObject.SetActive(true);
  }
}
