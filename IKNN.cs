using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP01_Heart_Diagnostic
{
    internal interface IKNN
    {
        /* main methods */
        void Train(string filename_train_samples_csv, int k = 1, int distance = 1);
        float Evaluate(string filename_test_samples_csv);
        bool Predict(Heart sample_to_predict);
        /* utils */
        float EuclideanDistance(Heart first_sample, Heart second_sample);
        bool Vote(List<bool> sorted_labels);
        void ConfusionMatrix(List<bool> predicted_labels, List<bool> expert_labels, bool[] labels);
        void ShellSort(List<float> distances, List<bool> labels);
        List<Heart> ImportSamples(string filename_samples_csv);
    }
}