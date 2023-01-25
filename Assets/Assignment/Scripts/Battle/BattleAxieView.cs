using System.Collections;
using System.Collections.Generic;
using Assignment.ScriptableObjects;
using DG.Tweening;
using Game;
using Newtonsoft.Json.Linq;
using Spine;
using UnityEngine;
using UnityEngine.Networking;

namespace Assignment.Battle
{
    public class BattleAxieView : AxieFigure
    {
        #region FIELDS

        private readonly List<string> animAttack = new List<string>()
        {
            "attack/melee/horn-gore",
            "attack/melee/mouth-bite",
            "attack/melee/multi-attack",
            "attack/melee/normal-attack",
            "attack/melee/tail-multi-slap",
            "attack/melee/tail-smash",
            "attack/melee/tail-thrash"
        };

        private readonly List<string> animHurt = new List<string>()
        {
            "defense/hit-by-normal"
        };

        private static Dictionary<string, string> axieId2GenreString = new Dictionary<string, string>();

        private new Camera camera;
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
            this.transform.rotation = Quaternion.Euler(this.camera.transform.eulerAngles.x, 0, 0);
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

        public new void SetGenes(string axieId, string genreStr)
        {
            base.SetGenes(axieId, genreStr);

            Vector3 originScale = this.transform.localScale;
            this.transform.localScale = Vector3.zero;
            this.transform.DOScale(originScale, 0.5f)
                .SetEase(Ease.OutBack);

            this.transform.DOLocalJump(Vector3.zero, 16, 1, 0.5f);
            this.DoAnimIdle();
        }

        public void DoAnimAttack()
        {
            string anim = RandomHelper.GetRandomElementFromList(this.animAttack);
            this.SkeletonAnimation.timeScale = 1f;
            this.SkeletonAnimation.AnimationState.SetAnimation(0, anim, false);
        }

        public void DoAnimDamaged()
        {
            string anim = RandomHelper.GetRandomElementFromList(this.animHurt);
            this.SkeletonAnimation.timeScale = 1f;
            this.SkeletonAnimation.AnimationState.SetAnimation(0, anim, false);
        }

        public void DoAnimMove()
        {
            this.SkeletonAnimation.timeScale = 1f;
            this.SkeletonAnimation.AnimationState.SetAnimation(0, "action/move-forward", false);
        }

        public void DoAnimDie(System.Action callback = null)
        {
            this.SkeletonAnimation.timeScale = 1f;
            this.SkeletonAnimation.AnimationState.SetAnimation(0, "defense/hit-by-normal-dramatic", false);
            if (callback != null)
            {
                this.SkeletonAnimation.AnimationState.End += (trackEntry) => callback();
            }
        }

        public void DoAnimIdle()
        {
            this.SkeletonAnimation.timeScale = 1f;
            this.SkeletonAnimation.AnimationState.AddAnimation(0, "action/idle/normal", true, 0);
        }

        #endregion
    }
}