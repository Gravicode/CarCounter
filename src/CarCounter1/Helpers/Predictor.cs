using Microsoft.ML;
using System.Drawing;

namespace CarCounter1.Helpers
{
    public class Predictor
    {
        private MLContext _mLContext;
        private PredictionEngine<ImageData, ImagePrediction> _predictionEngine;

        public Predictor(ITransformer trainedModel)
        {
            _mLContext = new MLContext();
            _predictionEngine = _mLContext.Model
                               .CreatePredictionEngine<ImageData, ImagePrediction>(trainedModel);
        }

        public ImagePrediction Predict(Bitmap image)
        {
            return _predictionEngine.Predict(new ImageData() { Image = image });
        }
    }
}
