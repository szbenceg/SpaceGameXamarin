using beadXamarin.ViewModel;
using System;
using System.Collections.Generic;

namespace SpaceGame.ViewModel
{
    public class PlayerField : GameObjectField
    {
        public PlayerField(int width, int height, int positionX, int positionY) : base(width, height, positionX, positionY, 10, "/images/rocket.png")
        {
        }
    }
}
