using System.Collections;
using Game;
using Newtonsoft.Json.Linq;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace Assignment.GameAxie
{
    public class GameAxie : AxieFigure
    {
        #region FIELDS

        [SerializeField] private AxieType axieType;

        #endregion

        #region PROPETIES

        public AxieType AxieType => axieType;

        #endregion

        #region UNITY EVENTS

        private void Start()
        {
            string axieId = AxieTypeDict.GetAxieId(axieType);
            StartCoroutine(GetAxiesGenes(axieId));
        }

        #endregion

        #region METHODS

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