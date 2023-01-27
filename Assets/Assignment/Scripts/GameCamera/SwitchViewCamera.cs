using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assignment.GameCamera
{
    public class SwitchViewCamera : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private float timeChangeCameraView;
        [SerializeField] private int initViewIndex;
        [SerializeField] private List<CameraViewInfo> listViewInfo;
        [SerializeField] private Camera gameCamera;

        private int currentViewIndex;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY EVENTS

        private void Start()
        {
            const float defaultChangeTime = 0.3F;
            if (this.timeChangeCameraView <= 0) this.timeChangeCameraView = defaultChangeTime;

            this.currentViewIndex = this.initViewIndex;
            this.SetView(this.currentViewIndex, true);
        }

        private void Update()
        {
            if (Keyboard.current.tabKey.wasReleasedThisFrame)
            {
                this.ChangeCameraView();
            }
        }

        #endregion

        #region METHODS

        private void ChangeCameraView()
        {
            this.currentViewIndex = GetNextIndexInCircle(this.currentViewIndex, this.listViewInfo.Count);
            this.SetView(this.currentViewIndex);
        }

        private void SetView(int index, bool snapImmediate = false)
        {
            if (index < 0 || index >= this.listViewInfo.Count) return;
            CameraViewInfo info = this.listViewInfo[index];

            if (snapImmediate)
            {
                this.gameCamera.transform.position = info.position;
                this.gameCamera.transform.rotation = Quaternion.Euler(info.rotation);
            }
            else
            {
                this.gameCamera.DOKill();
                this.gameCamera.transform.DOMove(info.position, this.timeChangeCameraView)
                    .SetEase(Ease.InOutSine);
                this.gameCamera.transform.DORotateQuaternion(Quaternion.Euler(info.rotation), this.timeChangeCameraView);
            }
        }

        private static int GetNextIndexInCircle(int current, int size)
        {
            return (current + 1) % size;
        }

        #endregion
    }

    [Serializable]
    public struct CameraViewInfo
    {
        public Vector3 position;
        public Vector3 rotation;
    }
}