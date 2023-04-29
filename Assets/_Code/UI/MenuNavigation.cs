using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Code.UI
{
    public class MenuNavigation : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject defaultSelectedUIElement;

        private PlayerInput playerInput;
        private InputAction movementAction;
        private InputAction submitAction;
        private InputAction cancelAction;

        private GameObject currentSelectedUIElement;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            movementAction = playerInput.actions["Navigate"];
            submitAction = playerInput.actions["Submit"];
            cancelAction = playerInput.actions["Cancel"];
        }

        private void OnEnable()
        {
            movementAction.performed += OnNavigatePerformed;
            submitAction.performed += OnSubmitPerformed;
            cancelAction.performed += OnCancelPerformed;
        }

        private void OnDisable()
        {
            movementAction.performed -= OnNavigatePerformed;
            submitAction.performed -= OnSubmitPerformed;
            cancelAction.performed -= OnCancelPerformed;
        }

        private void Start()
        {
            if (defaultSelectedUIElement != null)
            {
                eventSystem.SetSelectedGameObject(defaultSelectedUIElement);
                currentSelectedUIElement = defaultSelectedUIElement;
            }
        }

        private void Update()
        {
            if (currentSelectedUIElement == null || !currentSelectedUIElement.activeInHierarchy)
            {
                FindAndSelectFirstActiveSelectable();
            }
        }


        private void FindAndSelectFirstActiveSelectable()
        {
            Selectable[] uiElements = GameObject.FindObjectsOfType<Selectable>();

            if (uiElements.Length == 0)
            {
                return;
            }

            int startIndex = -1;
            for (int i = 0; i < uiElements.Length; i++)
            {
                if (uiElements[i].gameObject == currentSelectedUIElement)
                {
                    startIndex = i;
                    break;
                }
            }

            if (startIndex == -1)
            {
                startIndex = 0;
            }

            int index = startIndex;
            do
            {
                index = (index + 1) % uiElements.Length;
                if (uiElements[index].gameObject.activeInHierarchy && uiElements[index].interactable)
                {
                    eventSystem.SetSelectedGameObject(uiElements[index].gameObject);
                    currentSelectedUIElement = uiElements[index].gameObject;
                    break;
                }
            } while (index != startIndex);
        }



        private void OnNavigatePerformed(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();
            AxisEventData axisEventData = new(eventSystem)
            {
                moveVector = movement
            };
            ExecuteEvents.Execute(currentSelectedUIElement, axisEventData, ExecuteEvents.moveHandler);
            currentSelectedUIElement = axisEventData.selectedObject;
        }

        private void OnSubmitPerformed(InputAction.CallbackContext context)
        {
            if (currentSelectedUIElement != null)
            {
                ExecuteEvents.Execute(currentSelectedUIElement, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);

                // Handle toggle state change explicitly
                if (currentSelectedUIElement.TryGetComponent<Toggle>(out var toggleComponent))
                {
                    toggleComponent.isOn = !toggleComponent.isOn;
                }
            }
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            // Implement cancel logic if needed
        }
    }
}
