using UnityEngine;

public class Trap : ARoomContentData
{
    private int _damage;
    private EntityTag[] _disarmTagMask;
    
    public override void Interact(Entity entity)
    {
        foreach (EntityTag tagMask in entity.tagMask)
        {
            //check if any of the hero tags are found in the trap's disarm tags
            for (int i = 0; i < _disarmTagMask.Length; i++)
            {
                if (_disarmTagMask[i] == tagMask)
                {
                    Disarm();
                    return;
                }
            }

            //check if any of the hero tags are found in the trap's interactable tags
            for (int i = 0; i < _interactableTagMask.Length; i++)
            {
                if (_interactableTagMask[i] == tagMask)
                {
                    //add logic to kill the hero
                    return;
                }
            }
        }    
    }

    public void Disarm()
    {
        //add logic to disarm the trap
        throw new System.NotImplementedException();
    }
}
