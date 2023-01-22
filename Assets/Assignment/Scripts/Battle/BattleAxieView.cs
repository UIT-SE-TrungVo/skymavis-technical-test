using System.Collections;
using Assignment.ScriptableObjects;
using Game;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Vector3 = System.Numerics.Vector3;

namespace Assignment.Battle
{
    public class BattleAxieView : AxieFigure
    {
        #region FIELDS

        private Camera camera;
        private bool isLookAtCamera = false;

        #endregion

        #region PROPETIES

        #endregion

        #region UNITY EVENTS

        private void Start()
        {
            this.camera = Camera.main;
            this.isLookAtCamera = camera != null;
        }

        private void Update()
        {
            this.LookTowardCamera();
        }

        #endregion

        #region METHODS

        private void LookTowardCamera()
        {
            if (!this.isLookAtCamera) return;
            Quaternion quaternionLook = Quaternion.LookRotation(this.transform.position - this.camera.transform.position);
            quaternionLook.y = 0;
            quaternionLook.z = 0;
            this.transform.rotation = quaternionLook;
        }

        public void ApplyViewConfig(AxieConfigInfo info)
        {
            this.StartCoroutine(GetAxiesGenes(info.axieId));
        }

        private IEnumerator GetAxiesGenes(string axieId)
        {
            string searchString = "{ axie (axieId: \"" + axieId + "\") { id, genes, newGenes}}";
            JObject jPayload = new JObject { new JProperty("query", searchString) };

            var wr = new UnityWebRequest("https://graphql-gateway.axieinfinity.com/graphql", "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jPayload.ToString().ToCharArray());
            wr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            wr.downloadHandler = new DownloadHandlerBuffer();
            wr.SetRequestHeader("Content-Type", "application/json");
            wr.timeout = 10;
            yield return wr.SendWebRequest();
            if (wr.error == null)
            {
                var result = wr.downloadHandler != null ? wr.downloadHandler.text : null;
                if (!string.IsNullOrEmpty(result))
                {
                    JObject jResult = JObject.Parse(result);
                    string genesStr = (string)jResult["data"]?["axie"]?["newGenes"];
                    this.SetGenes(axieId, genesStr);
                }
            }
        }

        #endregion
    }
}