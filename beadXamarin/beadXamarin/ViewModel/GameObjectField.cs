using System;

namespace beadXamarin.ViewModel
{
    public class GameObjectField : ViewModelBase
    {

        public int width;

        public int height;

        public int x;

        public int y;

        public int speed;

        public String imageSource;

        public GameObjectField(int width, int height, int positionX, int positionY, int speed, String imageSource)
        {
            this.width = width;
            this.height = height;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.speed = speed;
            this.imageSource = imageSource;
            OnPropertyChanged();

        }

        public String ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int PositionY
        {
            get { return y; }
            set { y = value; OnPropertyChanged(); }
        }

        public int PositionX
        {
            get { return x; }
            set { x = value; OnPropertyChanged(); }
        }
    }
}
