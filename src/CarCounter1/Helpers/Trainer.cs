using Microsoft.ML;
using System.Collections.Generic;
using System.Linq;

using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;

namespace CarCounter1.Helpers
{
    public class Trainer
    {
        private MLContext _mlContext;


        public Trainer()
        {
            _mlContext = new MLContext();
        }
        public ITransformer BuildAndTrain(string yoloModelPath)
        {
            var pipeline = _mlContext.Transforms.ResizeImages(inputColumnName: "image",
                        outputColumnName: "input_1:0", imageWidth: 416, imageHeight: 416,
                        resizing: ResizingKind.IsoPad)
            .Append(_mlContext.Transforms.ExtractPixels(outputColumnName: "input_1:0",
                                                        scaleImage: 1f / 255f,
                                                        interleavePixelColors: true))
            .Append(_mlContext.Transforms.ApplyOnnxModel(outputColumnNames: new[]
                {
                        "Identity:0",
                        "Identity_1:0",
                        "Identity_2:0"
                }, inputColumnNames: new[]
                {
                        "input_1:0"
                },
               
                modelFile: yoloModelPath,
                shapeDictionary: new Dictionary<string, int[]>()
                {
                        { "input_1:0", new[] { 1, 416, 416, 3 } },
                        { "Identity:0", new[] { 1, 52, 52, 3, 85 } },
                        { "Identity_1:0", new[] { 1, 26, 26, 3, 85 } },
                        { "Identity_2:0", new[] { 1, 13, 13, 3, 85 } },
                }
               ,null,false));

            return pipeline.Fit(_mlContext.Data.LoadFromEnumerable(new List<ImageData>()));
        }
        /*
         //car
         public ITransformer BuildAndTrain(string yoloModelPath)
         {
             var pipeline = _mlContext.Transforms.ResizeImages(inputColumnName: "image",
                         outputColumnName: "modelInput", imageWidth: 448, imageHeight: 640,
                         resizing: ResizingKind.IsoPad)
             .Append(_mlContext.Transforms.ExtractPixels(outputColumnName: "modelInput",
                                                         scaleImage: 1f / 255f,
                                                         interleavePixelColors: true))
             .Append(_mlContext.Transforms.ApplyOnnxModel(
                  outputColumnNames: new[]
                 {
                         //"modelOutput",
                         "onnx::Sigmoid_527",
                         "onnx::Sigmoid_799",
                         "onnx::Sigmoid_1071"
                 },
                   inputColumnNames: new[]
                 {
                         "modelInput"
                 }, modelFile: yoloModelPath,
                 shapeDictionary: new Dictionary<string, int[]>()
                 {
                         { "modelInput", new[] { 1, 3, 448, 640 } },
                         //{ "modelOutput", new[] { 1, 17640, 10 } },
                         { "onnx::Sigmoid_527", new[] { 1, 3, 56, 80, 10 } },
                         { "onnx::Sigmoid_799", new[] { 1, 3, 28, 40, 10 } },
                         { "onnx::Sigmoid_1071", new[] { 1, 3, 14, 20, 10 } }
                 }, null
                 ,false
                )) ;

             return pipeline.Fit(_mlContext.Data.LoadFromEnumerable(new List<ImageData>()));
         }
        */
    }
}
