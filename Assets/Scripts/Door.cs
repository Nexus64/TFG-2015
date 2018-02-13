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
    protected LevelGenerator levelControler;

    // Use this for initialization
    void Start () {
		live = maxLive;
        animator = GetComponent<Animator>();
        GameObject room = GameObject.FindGameObjectWithTag("Room");
        levelControler = room.GetComponent<LevelGenerator>();
        GameObject playerObjects = GameObject.FindGameObjectWithTag("Player");
        player = playerObjects.GetComponent<MainCharacter>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool DoDamage(int damage) {
        live -= damage;
        health.DecreaseLife(damage);
        levelControler.DamageBoss(damage);
        animator.SetTrigger("Hit");
        animator.SetInteger("Live", live);
        animator.SetInteger("Repetition", hitRepetition);

        return live <= 0;
    }

    public void EndLevel() {
        player.WinCinematic();
        levelControler.DestroyBricks();
    }

    public void DestroyBoss()
    {
        float destroyTime = levelControler.DestroyBoss();
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
