using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;

namespace ReInvented.Thickener.MLModel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly string _modelPath = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Desktop\ML\model.zip";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            List<ThickenerData> thickenerDataset = ThickenerData.GetTestData();

            ThickenerData testInput = ThickenerData.GetTestInputData();

            Trainer.TrainModel(thickenerDataset, _modelPath);
            float predictedWeight = Predictor.Predict(_modelPath, testInput);

            Debug.Print($"Predicted Steel Weight: {predictedWeight} ton");
        }



    }

    #region Predictor

    public class Predictor
    {
        public static float Predict(string modelPath, ThickenerData input)
        {
            MLContext mlContext = new();
            ITransformer model = mlContext.Model.Load(modelPath, out _);
            PredictionEngine<ThickenerData, ThickenerPrediction> predEngine =
                mlContext.Model.CreatePredictionEngine<ThickenerData, ThickenerPrediction>(model);

            ThickenerPrediction prediction = predEngine.Predict(input);
            return prediction.PredictedSteelWeight;
        }
    }

    #endregion

    #region Trainer

    public class Trainer
    {
        public static void TrainModel(List<ThickenerData> inputData, string modelPath)
        {
            MLContext mlContext = new();

            // Convert List<ThickenerData> to IDataView
            IDataView data = mlContext.Data.LoadFromEnumerable(inputData);

            // Define training pipeline
            var pipeline =
                mlContext.Transforms.Concatenate("Features",
                    nameof(ThickenerData.D),
                    nameof(ThickenerData.FloorSlope),
                    nameof(ThickenerData.SgOC),
                    nameof(ThickenerData.SgSC),
                    nameof(ThickenerData.H),
                    nameof(ThickenerData.HL),
                    nameof(ThickenerData.BL),
                    nameof(ThickenerData.AImp),
                    nameof(ThickenerData.ACon),
                    nameof(ThickenerData.FB)).Append(mlContext.Regression.Trainers.FastTree());

            //mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features")

            //mlContext.Regression.Trainers.FastTree()

            if (File.Exists(modelPath))
                File.Delete(modelPath);

            // Train model
            var model = pipeline.Fit(data);

            // Save model
            mlContext.Model.Save(model, data.Schema, modelPath);

            Debug.Print("Model trained and saved.");
        }
    }

    #endregion

    #region Thickener Prediction Class

    public class ThickenerPrediction
    {
        #region Public Properties

        [ColumnName("Score")]
        public float PredictedSteelWeight { get; set; }

        #endregion
    }

    #endregion

    #region Thickener Data Class

    public class ThickenerData
    {
        #region Default Constructor

        public ThickenerData()
        {

        }

        #endregion

        #region Public Properties

        public float D { get; set; }
        public float H { get; set; }
        public float FloorSlope { get; set; }
        public float BL { get; set; }
        public float HL { get; set; }
        public float FB { get; set; }
        public float SgOC { get; set; }
        public float SgSC { get; set; }
        public float AImp { get; set; }
        public float ACon { get; set; }

        [ColumnName("Label")]
        public float Weight { get; set; }

        #endregion

        #region Public Static Functions

        public static List<ThickenerData> GetTestData()
        {
            List<ThickenerData> data = new()
            {
                new ThickenerData() { D = 20.0f, H = 3.50f, FloorSlope = 9.0f, BL = 0.60f, HL = 0.80f, FB = 0.15f, SgOC = 1.22f, SgSC = 1.45f, AImp = 0.15f, ACon = 0.025f, Weight = 50.0f },
                new ThickenerData() { D = 20.0f, H = 3.50f, FloorSlope = 9.0f, BL = 0.60f, HL = 0.80f, FB = 0.15f, SgOC = 1.22f, SgSC = 1.45f, AImp = 0.18f, ACon = 0.025f, Weight = 54.0f },
                new ThickenerData() { D = 22.0f, H = 3.50f, FloorSlope = 9.0f, BL = 0.60f, HL = 0.80f, FB = 0.15f, SgOC = 1.22f, SgSC = 1.45f, AImp = 0.18f, ACon = 0.025f, Weight = 57.0f },
                new ThickenerData() { D = 20.0f, H = 3.00f, FloorSlope = 11.0f, BL = 0.60f, HL = 0.80f, FB = 0.15f, SgOC = 1.22f, SgSC = 1.45f, AImp = 0.18f, ACon = 0.025f, Weight = 57.0f },
                new ThickenerData() { D = 24.0f, H = 3.00f, FloorSlope = 9.0f, BL = 0.60f, HL = 0.80f, FB = 0.15f, SgOC = 1.28f, SgSC = 1.49f, AImp = 0.15f, ACon = 0.038f, Weight = 58.0f },
                new ThickenerData() { D = 25.0f, H = 3.00f, FloorSlope = 12.0f, BL = 0.60f, HL = 0.80f, FB = 0.15f, SgOC = 1.28f, SgSC = 1.49f, AImp = 0.17f, ACon = 0.038f, Weight = 83.0f },
                new ThickenerData() { D = 32.0f, H = 3.20f, FloorSlope = 9.0f, BL = 0.70f, HL = 0.75f, FB = 0.15f, SgOC = 1.33f, SgSC = 1.51f, AImp = 0.22f, ACon = 0.042f, Weight = 147.0f }
            };

            return data;
        }

        public static ThickenerData GetTestInputData()
        {
            return new ThickenerData() { D = 20.0f, H = 3.50f, FloorSlope = 9.0f, BL = 0.60f, HL = 0.80f, FB = 0.15f, SgOC = 1.22f, SgSC = 1.45f, AImp = 0.15f, ACon = 0.025f};
        }

        #endregion
    }

    #endregion

}
