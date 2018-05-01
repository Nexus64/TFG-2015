using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Door : MonoBehaviour {
	public int maxLive;
	private int live;
	public HealthBar health;
    public int hitRepetition;

    protected Animator animator;
    protected MainCharacter player;
    protected Room roomControler;

    // Use this for initialization
    void Awake () {
        animator = GetComponent<Animator>();
        roomControler = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacter>();
    }

    void Start()
    {
        live = maxLive;
    }

    public bool DoDamage(int damage) {
        live -= damage;
        health.DecreaseLife(damage);
        roomControler.DamageBoss(damage);
        animator.SetTrigger("Hit");
        animator.SetInteger("Live", live);
        animator.SetInteger("Repetition", hitRepetition);

        return live <= 0;
    }

    public void EndLevel() {
        player.WinCinematic();
        roomControler.DestroyBricks();
    }

    public void DestroyBoss()
    {
        float destroyTime = roomControler.DestroyBoss();
        Invoke("Open", destroyTime);
    }

    void Open()
    {
        animator.SetTrigger("Open");
    }

    public void SetLive(int newLive)
    {
        maxLive = newLive;
        live = newLive;
        health.SetHealth(newLive);
        animator.SetInteger("Live", live);
    }

    public void ReduceHitRepetition()
    {
        animator.SetInteger("Repetition", animator.GetInteger("Repetition") - 1);
    }
}
