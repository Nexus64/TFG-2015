using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator))]
public class FlashTransition : MonoBehaviour
{
    public Material effectMaterial;
    public Color flashColor = Color.white;
    [Range(0, 1)]
    public float fade = 1;

    protected Animator animator;
    //protected Rec
    protected bool inTransition = true;
    protected int targetScene;

    private void Start()
    {
        animator = GetComponent<Animator>();

        effectMaterial.SetColor("_Color", flashColor);
        effectMaterial.SetFloat("_Fade", fade);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        effectMaterial.SetFloat("_Fade", fade);
        Graphics.Blit(src, dst, effectMaterial);
    }

    public void ResetScene()
    {
        int thisID = SceneManager.GetActiveScene().buildIndex;

        StartTransition(thisID);
    }

    public void StartTransition(int newSceneID)
    {
        if (!inTransition)
        {
            targetScene = newSceneID;
            inTransition = true;
            animator.SetBool("Animated", true);
        }
    }

    public void EndFadeIn()
    {
        inTransition = false;
        animator.SetBool("Animated", false);

    }

    public void EndFadeOut()
    {
        inTransition = false;
        ChangeScene();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(targetScene);
    }

    public void SetColor(Color color)
    {
        flashColor = color;
        effectMaterial.SetColor("_Color", flashColor);
    }
}
