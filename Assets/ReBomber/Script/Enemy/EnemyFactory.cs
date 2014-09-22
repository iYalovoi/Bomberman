using UnityEngine;
using System.Collections;

namespace Assets.Script
{
    public interface IEnemyFactory
    {
        GameObject Produce(EnemyTypes power);
    }

    public class EnemyFactory : MonoBehaviour
    {
        public GameObject Prefab;

        private EnemiesStats _enemiesStats;

        void Awake()
        {
            _enemiesStats = new EnemiesStats();
        }

        public GameObject Produce(EnemyTypes enemyType)
        {           
            var enemyInstance = Instantiate(Prefab) as GameObject;
            enemyInstance.name = enemyType.ToString();

            var animationSkin = enemyInstance.GetComponent<ReSkinAnimation>();
            animationSkin.enemyType = enemyType;

            var enemyStats = _enemiesStats.GetStats(enemyType);

            var enemy = enemyInstance.GetComponent<Enemy>();
            enemy.MaxSpeed = enemyStats.Speed;
            enemy.Bounty = enemyStats.Bounty;
            enemy.Type = enemyType;

            var behaviour = new CompositePattern();
            if (enemyStats.PlayerVisionRange > 0.0f)
                behaviour.Append(new ChasingPlayer(enemyStats.PlayerVisionRange));
            if (enemyStats.BombVisionRange > 0.0f)
                behaviour.Append(new DodgingBombs(enemyStats.BombVisionRange));
            behaviour.Append(new RandomRoaming(enemyStats.Uncertainty));
            enemy.Behaviour = behaviour;

            if (enemyStats.IsGhost)
                enemyInstance.layer = LayerMask.NameToLayer("Ghost");

            return enemyInstance;
        }
    }
}