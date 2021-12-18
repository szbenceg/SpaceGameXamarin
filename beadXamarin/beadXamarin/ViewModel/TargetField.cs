using beadXamarin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame.ViewModel
{
    public class TargetField : GameObjectField
    {
        public TargetField(int width, int height, int positionX, int positionY) : base(width, height, positionX, positionY, 10, "/images/target.png")
        {
        }
    }
}
