using GameStateLabs;
using GameStateLabs.Common;

namespace Match3
{
    public class LevelMoves : Level
    {

        public int numMoves;
        public int targetScore;

        private int _movesUsed = 0;

        private void Start()
        {
            type = LevelType.Moves;

            hud.SetLevelType(type);
            hud.SetScore(currentScore);
            hud.SetTarget(targetScore);
            hud.SetRemaining(numMoves);
        }

        public override void OnMove()
        {
            _movesUsed++;
            
            var moveEvent = new GslEvent("move player");
            moveEvent.SetCustomProperty("move used", new IntType(_movesUsed));
            moveEvent.SetCustomProperty("target score", new IntType(targetScore));
            moveEvent.SetCustomProperty("moves used", new IntType(_movesUsed));
            moveEvent.Track();
            
            hud.SetRemaining(numMoves - _movesUsed);

            if (numMoves - _movesUsed != 0) return;
        
            if (currentScore >= targetScore)
            {
                GameWin();
            }
            else
            {
                GameLose();
            }
        }
    }
}
