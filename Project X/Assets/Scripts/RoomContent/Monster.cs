using UnityEngine;

public class Monster : ARoomContentData
{
    private int _health;
    private int _damage;
    private int _fameReward;
    private EntityTag[] _killTagMask;

    public override void Interact(Entity entity)
    {
        foreach (EntityTag tagMask in entity.tagMask)
        {
            //check if any of the hero tags are found in the monsters kill tags
            for (int i = 0; i < _killTagMask.Length; i++)
            {
                if (_killTagMask[i] == tagMask)
                {
                    Kill();
                    return;
                }
            }

            //check if any of the hero tags are found in the monsters interactable tags
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

    public void Kill()
    {
        //add logic to kill the monster itself
        throw new System.NotImplementedException();
    }
}
