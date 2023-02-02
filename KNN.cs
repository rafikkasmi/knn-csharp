using System.Data;
using System.Globalization;
using CsvHelper;
using TP01_Heart_Diagnostic;

class KNN : IKNN
{
    private List<Heart> trainData;
    private int k, distance;
    public List<Heart> GetDataFromCsv(string filename)
    {
        var records = new List<Heart>();
        using (var reader = new StreamReader(filename))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                bool label = csv.GetField("target") == "0" ? false : true;
                float[] features = {
                float.Parse(csv.GetField("cp")),
                float.Parse(csv.GetField("thal")),
                float.Parse(csv.GetField("ca")),
                float.Parse(csv.GetField("oldpeak")),
                float.Parse(csv.GetField("thalach"))
                };
                var record = new Heart(features, label);
                records.Add(record);
            }
        }
        return records;
    }
    public void Train(string filename_train_samples_csv, int k = 1, int distance = 1)
    {
        trainData = GetDataFromCsv(filename_train_samples_csv);
        this.k = k;
        this.distance = distance;
    }
    public float Evaluate(string filename_test_samples_csv)
    {

        List<Heart> testData = GetDataFromCsv(filename_test_samples_csv);

        List<bool> expert_labels = new List<bool>();

        List<bool> predicted_labels = new List<bool>();

        foreach (var entity in testData)
        {
            expert_labels.Add(entity.Label);

            predicted_labels.Add(Predict(entity));

        }
        float sum = 0;
        for (int i = 0; i < expert_labels.Count; i++)
        {
            if (expert_labels[i] == predicted_labels[i]) sum++;
        }
        bool[] labels = { true, false };
        ConfusionMatrix(predicted_labels, expert_labels, labels);
        return sum / expert_labels.Count;
    }
    public bool Predict(Heart sample_to_predict)
    {
        List<float> distances = new List<float>();
        List<bool> expert_labels = new List<bool>();


        foreach (var entity in trainData)
        {
            expert_labels.Add(entity.Label);
            distances.Add(EuclideanDistance(sample_to_predict, entity));

        }
        ShellSort(distances, expert_labels);

        return Vote(expert_labels);

    }
    /* utils */
    public float EuclideanDistance(Heart first_sample, Heart second_sample)
    {
        float sum = 0;
        for (int i = 0; i < 5; i++)
        {
            sum += MathF.Pow((first_sample.Features[i] - second_sample.Features[i]), 2);
        }

        return MathF.Sqrt(sum);
    }
    public bool Vote(List<bool> sorted_labels)
    {

        //vote de majorité
        int countTrue = 0, countFalse = 0;
        int n = sorted_labels.Count;
        for (int i = 0; i < k; i++)
        {
            if (sorted_labels[i] == true)
                countTrue++;
            else
                countFalse++;

        }
        return countTrue > countFalse ? true : false;

    }
    public void ConfusionMatrix(List<bool> predicted_labels, List<bool> expert_labels, bool[] labels)
    {
        //0 : [0,0], 1 : [0,1] , 2 : [1,0] ,3 : [1,1]
        int[] counts = { 0, 0, 0, 0 };

        for (int i = 0; i < predicted_labels.Count; i++)
        {
            if (!predicted_labels[i] && !expert_labels[i]) counts[0]++;
            if (!predicted_labels[i] && expert_labels[i]) counts[1]++;
            if (predicted_labels[i] && !expert_labels[i]) counts[2]++;
            if (predicted_labels[i] && expert_labels[i]) counts[3]++;
        }

        Console.WriteLine("|------------- Matrice Du Confusion ------------|");
        Console.WriteLine("|-------------------------------|");
        Console.WriteLine("|           |         Predites        |");
        Console.WriteLine("|-------------------------------------|");
        Console.WriteLine("|           |   |     0    |     1    |");
        Console.WriteLine("|           --------------------------|");
        Console.WriteLine(string.Format("|   Expert  | 0 |     {0}    |   {0}    |", counts[0], counts[1]));
        Console.WriteLine("|           --------------------------|");
        Console.WriteLine(string.Format("|           | 1 |     {0}    |   {0}    |", counts[2], counts[3]));
        Console.WriteLine("|-------------------------------------|");
    }
    public void ShellSort(List<float> distances, List<bool> labels)
    {
        int n = distances.Count;

        for (int gap = n / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < n; i += 1)
            {
                float temp = distances[i];
                bool tempLabel = labels[i];
                int j;
                for (j = i; j >= gap && distances[j - gap] > temp; j -= gap)
                {
                    distances[j] = distances[j - gap];
                    labels[j] = labels[j - gap];
                }
                distances[j] = temp;
                labels[j] = tempLabel;
            }
        }
    }
    public List<Heart> ImportSamples(string filename_samples_csv)
    {
        List<Heart> Hearts = new List<Heart>();
        return Hearts;
    }
}