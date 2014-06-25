namespace csimages
{
    public class Complex
    {
        private double _re;
        private double _im;

        public Complex(double re, double im)
        {
            _re = re;
            _im = im;
        }

        public static readonly Complex Zero = new Complex(0, 0);
        public static readonly Complex R = new Complex(1, 0);
        public static readonly Complex I = new Complex(0, 1);
        public static readonly Complex One = new Complex(1, 1);

        public double Re
        {
            get { return _re; }
            set { _re = value;}
        }

        public double Im
        {
            get { return _im; }
            set { _im = value; }
        }
    }
}