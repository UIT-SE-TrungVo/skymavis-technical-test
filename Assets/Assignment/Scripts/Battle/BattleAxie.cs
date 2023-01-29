using System;
using Assignment.Battle.BattleItem;
using Assignment.Battle.BattleItem.Enum;
using Assignment.Battle.GuideMap;
using Assignment.Battle.Model;
using Assignment.Battle.UI;
using Assignment.ScriptableObjects;
using Assignment.Sound;
using Assignment.Sound.Enum;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Assignment.Battle
{
    public class BattleAxie : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private AxieConfig config;
        [SerializeField] private BattleAxieSide axieSide;

        private float currentHealth;
        private int luckyBattleNumber;
        private float latestDamage;

        private BattleAxieInfo stats;
        private BattleAxieView axieView;
        private BattleField battleField;
        private BattleAxie target;

        private BattleItem.BattleItem item;

        #endregion

        #region PROPERTIES

        public BattleAxieSide AxieSide => axieSide;

        public BattleAxieInfo Stats => stats;

        public float CurrentHealth
        {
            get => currentHealth;
            private set => currentHealth = value;
        }

        public BattleField BattleField
        {
            get => battleField;
            set => battleField = value;
        }

        public BattleAxie Target
        {
            get => target;
            private set => target = value;
        }

        public int LuckyBattleNumber
        {
            get => luckyBattleNumber;
            private set => luckyBattleNumber = value;
        }

        public float LatestDamage
        {
            get => latestDamage;
            private set => latestDamage = value;
        }

        public BattleItem.BattleItem Item
        {
            get => item;
            private set => item = value;
        }

        #endregion

        #region UNITY EVENTS

        private void Awake()
        {
            this.axieView = this.GetComponentInChildren<BattleAxieView>();

            this.ApplyConfig();
            this.CurrentHealth = this.Stats.InitHealth;

            this.LuckyBattleNumber = this.GenerateLuckyBattleNumber();
        }

        private void Start()
        {
            this.Item = BattleItemFactory.GetRandomItemName(this);
        }

        private void LateUpdate()
        {
            if (this.IsDead())
            {
                this.BattleField.RemoveAxie(this);
                this.axieView.DoAnimDie();

                Destroy(this.gameObject, 3.0f);
                this.battleField.OnAxieDie();
            }
        }

        #endregion

        #region METHODS

        public bool CanMove()
        {
            return this.stats.MoveSpeed > 0;
        }

        public bool IsDead()
        {
            return this.CurrentHealth <= 0;
        }

        private void ApplyConfig()
        {
            AxieConfigInfo info;
            switch (this.AxieSide)
            {
                case BattleAxieSide.Attacker:
                    info = config.attackerInfo;
                    break;
                case BattleAxieSide.Defender:
                    info = config.defenderInfo;
                    break;
                default:
                    return;
            }

            this.stats = new BattleAxieInfo(info);
            if (this.axieView != null)
            {
                this.axieView.ApplyViewConfig(info);
            }
        }

        public System.Action GetNextAction()
        {
            if (this.IsDead()) return null;
            BattleFieldGuideMgr guideMgr = this.BattleField.GuideMgr;
            BattleFieldPositionMgr positionMgr = this.BattleField.PositionMgr;

            Vector2Int? currentCoord = positionMgr.GetCoordOfAxie(this);
            if (!currentCoord.HasValue) return null;

            Vector2Int? targetCoord = this.Target == null || this.Target.IsDead()
                ? null
                : positionMgr.GetCoordOfAxie(this.Target);


            if (!targetCoord.HasValue || Vector2Int.Distance(currentCoord.Value, targetCoord.Value) > 1)
            {
                Vector2Int? nearbyEnemyCoord = guideMgr.GetNearbyEnemyCoord(currentCoord.Value, this.AxieSide);
                if (nearbyEnemyCoord.HasValue)
                {
                    this.Target = positionMgr.GetAxieAtCoord(nearbyEnemyCoord.Value);
                    targetCoord = nearbyEnemyCoord;
                }
                else
                {
                    if (!this.CanMove()) return null;
                    Vector2Int? nextMoveCoord = guideMgr.GetNextMoveCoord(currentCoord.Value);
                    if (!nextMoveCoord.HasValue) return null;

                    this.FaceDirection(currentCoord.Value, nextMoveCoord.Value);
                    return () => this.GetMovement(nextMoveCoord.Value);
                }
            }

            if (targetCoord.HasValue)
            {
                this.FaceDirection(currentCoord.Value, targetCoord.Value);
            }

            this.axieView.DoAnimAttack();
            SoundPlayer.AssignPlaySound(SoundCategory.Hit);
            float damage = this.CalculateDamageBetweenAxies(this.Target);
            this.LatestDamage = damage;
            return () => this.Target.GetDamage(this, damage);
        }

        private float CalculateDamageBetweenAxies(BattleAxie receiver)
        {
            int pointSender = this.LuckyBattleNumber;
            int pointReceiver = receiver.LuckyBattleNumber;
            int diffPoint = (3 + pointSender - pointReceiver) % 3;
            return this.config.battleConfig.diffAndDamage[diffPoint];
        }

        public void GetDamage(BattleAxie attacker, float value)
        {
            DamageInfo damageInfo = new DamageInfo(value, attacker, this);
            damageInfo.ConductDamageFactors();
            if (damageInfo.IsDodged)
            {
                Debug.LogFormat("Axie dodged {0} {1}", this.GetInstanceID(), this.axieSide);
                attacker.Item?.OnDodgedAttack();
                this.Item?.OnDodgedAttack();
                this.axieView.DoAnimDodge();

                BattleEffect.DisplayDamagedEffect(this.gameObject, "missed");
            }
            else
            {
                float totalDamage = damageInfo.GetFinalDamage();
                Debug.LogFormat("Axie damaged {0} {1} {2}({3})", this.GetInstanceID(), this.axieSide, totalDamage,
                    this.currentHealth);

                this.CurrentHealth -= value;
                this.axieView.DoAnimDamaged();

                attacker.Item?.OnSuccessfulAttack(damageInfo, totalDamage);
                this.Item?.OnSuccessfulAttack(damageInfo, totalDamage);

                BattleEffect.DisplayDamagedEffect(this.gameObject, Mathf.RoundToInt(totalDamage).ToString());
            }
        }

        public void GetMovement(Vector2Int toCoord)
        {
            Debug.LogFormat("Axie move {0} {1}", this.GetInstanceID(), toCoord);
            BattleFieldPositionMgr positionMgr = this.BattleField.PositionMgr;

            bool success = positionMgr.PutAxieAtCoord(toCoord, this);
            if (!success) return;

            Vector3 posWorld = this.BattleField.GetWorldPosition(toCoord);
            this.transform.DOMove(posWorld, 0.5f).SetEase(Ease.OutBack);

            this.axieView.DoAnimMove();
            SoundPlayer.AssignPlaySound(SoundCategory.Move);
        }

        private void FaceDirection(Vector2Int curCoord, Vector2Int toCoord)
        {
            if (curCoord.x == toCoord.x) return;
            bool isFlipX = curCoord.x < toCoord.x;
            this.axieView.flipX = isFlipX;
        }

        private int GenerateLuckyBattleNumber()
        {
            int minRandom = this.config.battleConfig.minRandomGenNumber;
            int maxRandom = this.config.battleConfig.maxRandomGenNumber;
            return RandomHelper.GetRandomInt(minRandom, maxRandom);
        }

        public bool TrySetBattleItem(BattleItem.BattleItem setItem)
        {
            if (this.Item != null) return false;
            this.Item = setItem;
            return true;
        }

        #endregion
    }
}