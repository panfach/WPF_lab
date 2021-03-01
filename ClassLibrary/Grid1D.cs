

namespace Lab
{
    struct Grid1D
    {
        public float Step { get; set; }
        public int Size { get; set; }

        public Grid1D(float step, int size)
        {
            Step = step;
            Size = size;
        }

        public override string ToString()
        {
            return "(step: " + Step + " size: " + Size + ")";
        }

        public string ToString(string format)
        {
            return $"(step: {Step.ToString(format)} size: {Size.ToString(format)})";
        }
    }
}
