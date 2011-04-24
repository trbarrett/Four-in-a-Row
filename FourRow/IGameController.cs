using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourRow
{
    public interface IGameController
    {
        Game.GameCore Game { get; }
        IPlayer CurrentPlayer { get; }
    }
}
