using System;

namespace _Project.Develop.Logic.Gameplay
{
    public class GameplayConditionFactory
    {
        public Func<GameMode, bool> CreateWinCondition(GameplayWinType winType, int value)
        {
            return winType switch
            {
                GameplayWinType.Time => gameMode => gameMode.CurrentTimePerSeconds.Value >= value,
                GameplayWinType.CountKillEnemies => gameMode => gameMode.KilledEnemies.Value >= value,
                _ => throw new ArgumentOutOfRangeException(nameof(winType), winType, null)
            };
        }

        public Func<GameMode, bool> CreateDefeatCondition(GameplayDefeatType defeatType, int value)
        {
            return defeatType switch
            {
                GameplayDefeatType.CountEnemies => gameMode => gameMode.EnemiesCount >= value,
                GameplayDefeatType.HeroDeath => gameMode => gameMode.MainHeroIsDead.Value,
                _ => throw new ArgumentOutOfRangeException(nameof(defeatType), defeatType, null)
            };
        }
    }
}