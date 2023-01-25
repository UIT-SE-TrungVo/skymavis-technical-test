using UnityEngine;
using UnityEngine.InputSystem;

namespace Assignment.GameCamera
{
    public class ZoomCamera : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private float zoomSpeed;
        [SerializeField] private float minSize;
        [SerializeField] private float maxSize;
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
            if (this.isCameraNull) return;

            //inverted because scroll up -> zoom in / down -> zoom out
            float scrollValue = -Mouse.current.scroll.ReadValue().y;
            if (scrollValue == 0) return;

            float zoomValue = scrollValue * this.zoomSpeed * Time.deltaTime;
            float newZoom = Mathf.Clamp(this.gameCamera.orthographicSize + zoomValue, this.minSize, this.maxSize);
            this.gameCamera.orthographicSize = newZoom;
        }

        #endregion

        #region METHODS

        #endregion
    }
}