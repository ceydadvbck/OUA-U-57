using UnityEngine;

namespace VHS
{
    public class InputHandler : MonoBehaviour
    {
        #region Data
        [SerializeField] private InteractionInputData interactionInputData = null;
        
        #endregion

        #region BuiltIn Methods
        void Start()
        {
            interactionInputData.ResetInput();
        }

        void Update()
        {
            GetInteractionInputData();
        }
        #endregion

        #region Custom Methods
        void GetInteractionInputData()
        {
            interactionInputData.InteractedClicked = Input.GetKeyDown(KeyCode.E);
         //   Debug.Log("E clicked" + interactionInputData.InteractedClicked);
            interactionInputData.InteractedReleased = Input.GetKeyUp(KeyCode.E);
         //   Debug.Log("E released" + interactionInputData.InteractedReleased);
        }
        #endregion
    }
}