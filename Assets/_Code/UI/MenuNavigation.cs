using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _Code.UI
{
    public class MenuNavigation : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        // [SerializeField] private GameObject defaultSelectedUIElement;
        [SerializeField] private float checkInterval = 0.1f;

        private PlayerInput _playerInput;
        private InputAction _movementAction;
        private InputAction _submitAction;
        private InputAction _cancelAction;

        private GameObject _currentSelectedUIElement;
        private Selectable[] _uiElements;

        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();
            _movementAction = _playerInput.actions["Navigate"];
            _submitAction = _playerInput.actions["Submit"];
            _cancelAction = _playerInput.actions["Cancel"];
            
            _uiElements = FindObjectsOfType<Selectable>();
        }

        private void OnEnable()
        {
            _movementAction.performed += OnNavigatePerformed;
            _submitAction.performed += OnSubmitPerformed;
            _cancelAction.performed += OnCancelPerformed;
            
            StartCoroutine(CheckForActiveUIElementCoroutine());
        }

        private void OnDisable()
        {
            _movementAction.performed -= OnNavigatePerformed;
            _submitAction.performed -= OnSubmitPerformed;
            _cancelAction.performed -= OnCancelPerformed;
            
            StopCoroutine(CheckForActiveUIElementCoroutine());
        }

        private void Start()
        {
            FindAndSelectFirstActiveSelectable();
        }
        
        private IEnumerator CheckForActiveUIElementCoroutine()
        {
            while (true)
            {
                if (_currentSelectedUIElement == null || !_currentSelectedUIElement.activeInHierarchy)
                {
                    FindAndSelectFirstActiveSelectable();
                }

                yield return new WaitForSeconds(checkInterval);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private void FindAndSelectFirstActiveSelectable()
        {
            if (_uiElements.Length == 0)
            {
                return;
            }

            var startIndex = -1;
            for (var i = 0; i < _uiElements.Length; i++)
            {
                if (_uiElements[i].gameObject != _currentSelectedUIElement) continue;
                startIndex = i;
                break;
            }

            if (startIndex == -1)
            {
                startIndex = 0;
            }

            var index = startIndex;
            do
            {
                index = (index + 1) %(_uiElements.Length);
                if (!_uiElements[index].gameObject.activeInHierarchy || !_uiElements[index].interactable) continue;
                eventSystem.SetSelectedGameObject(_uiElements[index].gameObject);
                _currentSelectedUIElement = _uiElements[index].gameObject;
                break;
            } while (index != startIndex);
        }

        private void OnNavigatePerformed(InputAction.CallbackContext context)
        {
            var movement = context.ReadValue<Vector2>();
            AxisEventData axisEventData = new(eventSystem)
            {
                moveVector = movement
            };
            ExecuteEvents.Execute(_currentSelectedUIElement, axisEventData, ExecuteEvents.moveHandler);
            _currentSelectedUIElement = axisEventData.selectedObject;
        }

        private void OnSubmitPerformed(InputAction.CallbackContext context)
        {
            if (_currentSelectedUIElement == null) return;
            ExecuteEvents.Execute(_currentSelectedUIElement, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);

            // Handle toggle state change explicitly
            if (_currentSelectedUIElement.TryGetComponent<Toggle>(out var toggleComponent))
            {
                toggleComponent.isOn = !toggleComponent.isOn;
            }
        }


        private static void OnCancelPerformed(InputAction.CallbackContext context)
        {
            // Implement cancel logic if needed
        }
    }
}
