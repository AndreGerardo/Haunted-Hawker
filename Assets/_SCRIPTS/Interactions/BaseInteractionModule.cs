using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectBounceAnim))]
public class BaseInteractionModule : MonoBehaviour, IInteractable
{

    protected ObjectBounceAnim interactionAnim;
    protected CookingStationManager cookingStationManager;

    private void Awake()
    {
        interactionAnim = GetComponent<ObjectBounceAnim>();
    }

    public virtual void Interact()
    {
        FeedbackBounceAnim();
    }

    public virtual void InitModule(CookingStationManager c)
    {
        cookingStationManager = c;
    }

    protected void FeedbackBounceAnim()
    {
        interactionAnim.Play();
    }



    private void OnMouseDown()
    {
        Interact();
    }

}
