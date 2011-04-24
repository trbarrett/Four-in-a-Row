using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FourRow.AI;

namespace FourRow
{
    public interface IPlayer
    {
        IAI AI { get; }
        bool IsInteractive();

        void PlayTurn(IGameController gameController);
    }
}
