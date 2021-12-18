﻿namespace beadXamarin.Model
{
    public class ComponentProperties
    {

        private int speed;
        private int positionY;
        private int positionX;

        private int width;
        private int height;

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }
        public int PositionY
        {
            get
            {
                return positionY;
            }
            set
            {
                positionY = value;
            }
        }

        public int PositionX
        {
            get
            {
                return positionX;
            }
            set
            {
                positionX = value;
            }
        }

        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        public void moveLeft() {
            PositionX -= speed;
        }

        public void moveRight() {
            PositionX += speed;
        }

        public void moveDown()
        {
            PositionY += speed;
        }

    }
}
