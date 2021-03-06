//----------------------------------------------------------------------------
//  This file is automatically generated, do not modify.      
//----------------------------------------------------------------------------



using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Ccm
{
   public static partial class CcmInvoke
   {

     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveColorCorrectionModelSetCcmType(
        IntPtr obj,  
        ColorCorrectionModel.CcmType val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern double cveColorCorrectionModelGetLoss(IntPtr obj);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveColorCorrectionModelSetDistanceType(
        IntPtr obj,  
        ColorCorrectionModel.DistanceType val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveColorCorrectionModelSetLinearType(
        IntPtr obj,  
        ColorCorrectionModel.LinearType val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveColorCorrectionModelSetLinearGamma(
        IntPtr obj,  
        double val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveColorCorrectionModelSetLinearDegree(
        IntPtr obj,  
        int val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveColorCorrectionModelSetWeightCoeff(
        IntPtr obj,  
        double val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveColorCorrectionModelSetMaxCount(
        IntPtr obj,  
        int val);
     
   }

   public partial class ColorCorrectionModel
   {

     /// <summary>
     /// Ccm type
     /// </summary>
	 /// <param name="value">The value</param>
     public void SetCcmType(ColorCorrectionModel.CcmType value)
     {
        CcmInvoke.cveColorCorrectionModelSetCcmType(_ptr, value); 
     }
     
     /// <summary>
    /// Loss
     /// </summary>
     public double Loss
     {
        get { return CcmInvoke.cveColorCorrectionModelGetLoss(_ptr); } 
     }
     
     /// <summary>
     /// The type of color distance
     /// </summary>
	 /// <param name="value">The value</param>
     public void SetDistanceType(ColorCorrectionModel.DistanceType value)
     {
        CcmInvoke.cveColorCorrectionModelSetDistanceType(_ptr, value); 
     }
     
     /// <summary>
     /// The method of linearization
     /// </summary>
	 /// <param name="value">The value</param>
     public void SetLinearType(ColorCorrectionModel.LinearType value)
     {
        CcmInvoke.cveColorCorrectionModelSetLinearType(_ptr, value); 
     }
     
     /// <summary>
     /// The gamma value of gamma correction
     /// </summary>
	 /// <param name="value">The value</param>
     public void SetLinearGamma(double value)
     {
        CcmInvoke.cveColorCorrectionModelSetLinearGamma(_ptr, value); 
     }
     
     /// <summary>
     /// The degree of linearization polynomial
     /// </summary>
	 /// <param name="value">The value</param>
     public void SetLinearDegree(int value)
     {
        CcmInvoke.cveColorCorrectionModelSetLinearDegree(_ptr, value); 
     }
     
     /// <summary>
     /// The exponent number of L* component of the reference color in CIE Lab color space
     /// </summary>
	 /// <param name="value">The value</param>
     public void SetWeightCoeff(double value)
     {
        CcmInvoke.cveColorCorrectionModelSetWeightCoeff(_ptr, value); 
     }
     
     /// <summary>
     /// Used in MinProblemSolver-DownhillSolver, terminal criteria to the algorithm
     /// </summary>
	 /// <param name="value">The value</param>
     public void SetMaxCount(int value)
     {
        CcmInvoke.cveColorCorrectionModelSetMaxCount(_ptr, value); 
     }
     
   }
}
