class Program
{

    public static void Main(String[] args)
    {
        KNN model = new KNN();

        int kIndex = Array.IndexOf(args, "-k");
        if (kIndex < 0)
        {
            Console.WriteLine("Erreur : Il Faut Entrer la valeur de K");
            return;
        }

        int K = Int32.Parse(args[kIndex + 1]);

        int trainPathIndex = Array.IndexOf(args, "-t");
        if (trainPathIndex < 0)
        {
            Console.WriteLine("Erreur : Il Faut Entrer le path de fichier d'apprentissage");
            return;
        }
        string trainDataPath = args[trainPathIndex + 1];
        model.Train(trainDataPath, K);

        int evaluationPathIndex = Array.IndexOf(args, "-e");
        if (evaluationPathIndex > 0)
        {
            string evaluationPath = args[evaluationPathIndex + 1];
            Console.WriteLine(string.Format("Precision : {0} % .", model.Evaluate(evaluationPath) * 100));

        }

        int predictionPathIndex = Array.IndexOf(args, "-p");
        if (predictionPathIndex > 0)
        {
            string predictionPath = args[predictionPathIndex + 1];
            List<bool> predicitions = new List<bool>();
            List<Heart> data = model.GetDataFromCsv(predictionPath);
            foreach (var entity in data)
            {
                predicitions.Add(model.Predict(entity));
            }

            Console.WriteLine(string.Format("La liste des predictions: ({0}).", string.Join(", ", predicitions)));
        }
    }
}