namespace AlgoTreeDraw.Model
{
    public class RBT : Node
    {

        public RBT ()
        {
            color.R = 255;
            color.G = 0;
            color.B = 0;

            preColor.R = 255;
            preColor.G = 0;
            preColor.B = 0;


        }
       
        public override Node NewNode()
        {
            return new RBT() { X = -145, Y = 20, diameter = 50};
        }
    }
}
