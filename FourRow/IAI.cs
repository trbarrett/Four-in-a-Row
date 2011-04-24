using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.AI;

namespace FourRow
{
    public interface IAI
    {
        AIDifficulty Difficulty { get; }
        void PlayTurn(IGameController gameController, Object player);
        bool IsInteractive();
    }
}
