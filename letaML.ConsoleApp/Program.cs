// This file was auto-generated by ML.NET Model Builder. 

using System;
using LetaML.Model;

namespace LetaML.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Hora = @"16:52",
                Dia_semana = @"Quarta-Feira",
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Tempo_Min with predicted Tempo_Min from sample data...\n\n");
            Console.WriteLine($"Hora: {sampleData.Hora}");
            Console.WriteLine($"Dia_semana: {sampleData.Dia_semana}");
            Console.WriteLine($"\n\nPredicted Tempo_Min: {predictionResult.Score}\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}
