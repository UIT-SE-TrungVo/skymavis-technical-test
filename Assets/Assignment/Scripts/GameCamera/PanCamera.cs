using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assignment.GameCamera
{
    public class PanCamera : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private float panScreenRatio;
        [SerializeField] private float panSpeed;
        private Camera gameCamera;
        private bool isCameraNull;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.gameCamera = this.GetComponent<Camera>();
            this.isCameraNull = (this.gameCamera == null);
        }

        private void Update()
        {
            this.TrySnapCamera();
            this.TryPanCamera();
        }

        #endregion

        #region METHODS

        private void TrySnapCamera()
        {
            if (Keyboard.current.spaceKey.IsPressed())
            {
                Vector3 curCamPos = this.gameCamera.transform.position;
                this.gameCamera.transform.DOMove(new Vector3(0, curCamPos.y, 0), 0.05f)
                    .SetEase(Ease.InOutSine);
            }
        }

        private void TryPanCamera()
        {
            if (this.isCameraNull) return;
            Vector2 mousePos = Mouse.current.position.ReadValue();

            float xMoveCamera = this.CalcCameraMove(mousePos.x, Screen.width);
            float yMoveCamera = this.CalcCameraMove(mousePos.y, Screen.height);

            Vector3 vetMoveCamera = new Vector3(xMoveCamera, 0, yMoveCamera).normalized;
            vetMoveCamera *= (Time.unscaledDeltaTime * this.panSpeed);
            vetMoveCamera = Quaternion.Euler(-this.transform.rotation.eulerAngles.x, 0, 0) * vetMoveCamera;

            this.transform.Translate(vetMoveCamera, Space.Self);
        }

        private float CalcCameraMove(float mousePos, float sideLength)
        {
            //get the distance from nearest boundary to pos
            //if mouse in the trigger zone -> (nearer to boundary means higher value)
            float direction;
            float triggerRatio = this.panScreenRatio;
            float mousePosRatio = Mathf.Clamp(mousePos, 0, sideLength) / sideLength;
            if (mousePosRatio <= triggerRatio)
            {
                direction = -1;
            }
            else if (mousePosRatio >= 1.0f - triggerRatio)
            {
                mousePosRatio = 1 - mousePosRatio;
                direction = 1;
            }
            else return 0;

            return direction * mousePosRatio;
        }

        #endregion
    }
}