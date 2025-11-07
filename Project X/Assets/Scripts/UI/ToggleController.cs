using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ToggleController : MonoBehaviour
{
   public KeyCode ToggleKey = KeyCode.Space;
   [SerializeField] private List<Behaviour> _components = new List<Behaviour>();

   private void Update()
   {
      if(Input.GetKeyDown(ToggleKey))
         Toggle();
   }

   public void Toggle()
   {
      foreach (var component in _components)
      {
         component.enabled = !component.enabled;
      }
   }
}
