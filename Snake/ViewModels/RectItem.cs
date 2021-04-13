namespace Snake.ViewModels
{
    public class RectItem : ViewModelBase
    {
        private string _color;
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Color
        {
            get { return _color; }
            set
            {
                if(_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }
        public RectItem(double x,double y, double widht, double height, string color)
        {
            X = x;
            Y = y;
            Width = widht;
            Height = height;
            Color = color;
        }
    }
}
