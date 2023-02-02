using CsvHelper.Configuration.Attributes;
using TP01_Heart_Diagnostic;

class Heart : IData
{
    public Heart(float[] features, bool label)
    {
        Features = features;
        Label = label;
    }
    private float[] features;
    private bool label;

    public float[] Features { get; }
    public bool Label { get; }
    public void PrintInfo()
    {


    }
}