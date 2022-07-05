//----------------------------------------------------------------------------
//  This file is automatically generated, do not modify.      
//----------------------------------------------------------------------------



using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {

     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern double cveDualTVL1OpticalFlowGetTau(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetTau(
        IntPtr obj,  
        double val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern double cveDualTVL1OpticalFlowGetLambda(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetLambda(
        IntPtr obj,  
        double val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern double cveDualTVL1OpticalFlowGetTheta(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetTheta(
        IntPtr obj,  
        double val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern double cveDualTVL1OpticalFlowGetGamma(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetGamma(
        IntPtr obj,  
        double val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern int cveDualTVL1OpticalFlowGetScalesNumber(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetScalesNumber(
        IntPtr obj,  
        int val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern int cveDualTVL1OpticalFlowGetWarpingsNumber(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetWarpingsNumber(
        IntPtr obj,  
        int val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern double cveDualTVL1OpticalFlowGetEpsilon(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetEpsilon(
        IntPtr obj,  
        double val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern int cveDualTVL1OpticalFlowGetInnerIterations(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetInnerIterations(
        IntPtr obj,  
        int val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern int cveDualTVL1OpticalFlowGetOuterIterations(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetOuterIterations(
        IntPtr obj,  
        int val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     [return: MarshalAs(CvInvoke.BoolMarshalType)]
     internal static extern bool cveDualTVL1OpticalFlowGetUseInitialFlow(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetUseInitialFlow(
        IntPtr obj, 
        [MarshalAs(CvInvoke.BoolMarshalType)] 
        bool val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern double cveDualTVL1OpticalFlowGetScaleStep(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetScaleStep(
        IntPtr obj,  
        double val);
     
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)] 
     internal static extern int cveDualTVL1OpticalFlowGetMedianFiltering(IntPtr obj);
     [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
     internal static extern void cveDualTVL1OpticalFlowSetMedianFiltering(
        IntPtr obj,  
        int val);
     
   }

   public partial class DualTVL1OpticalFlow
   {

     /// <summary>
     /// Time step of the numerical scheme
     /// </summary>
     public double Tau
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetTau(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetTau(_ptr, value); }
     }
     
     /// <summary>
     /// Weight parameter for the data term, attachment parameter
     /// </summary>
     public double Lambda
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetLambda(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetLambda(_ptr, value); }
     }
     
     /// <summary>
     /// Weight parameter for (u - v)^2, tightness parameter
     /// </summary>
     public double Theta
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetTheta(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetTheta(_ptr, value); }
     }
     
     /// <summary>
     /// Coefficient for additional illumination variation term
     /// </summary>
     public double Gamma
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetGamma(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetGamma(_ptr, value); }
     }
     
     /// <summary>
     /// Number of scales used to create the pyramid of images
     /// </summary>
     public int ScalesNumber
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetScalesNumber(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetScalesNumber(_ptr, value); }
     }
     
     /// <summary>
     /// Number of warpings per scale
     /// </summary>
     public int WarpingsNumber
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetWarpingsNumber(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetWarpingsNumber(_ptr, value); }
     }
     
     /// <summary>
     /// Stopping criterion threshold used in the numerical scheme, which is a trade-off between precision and running time
     /// </summary>
     public double Epsilon
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetEpsilon(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetEpsilon(_ptr, value); }
     }
     
     /// <summary>
     /// Inner iterations (between outlier filtering) used in the numerical scheme
     /// </summary>
     public int InnerIterations
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetInnerIterations(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetInnerIterations(_ptr, value); }
     }
     
     /// <summary>
     /// Outer iterations (number of inner loops) used in the numerical scheme
     /// </summary>
     public int OuterIterations
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetOuterIterations(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetOuterIterations(_ptr, value); }
     }
     
     /// <summary>
     /// Use initial flow
     /// </summary>
     public bool UseInitialFlow
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetUseInitialFlow(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetUseInitialFlow(_ptr, value); }
     }
     
     /// <summary>
     /// Step between scales (less than 1)
     /// </summary>
     public double ScaleStep
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetScaleStep(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetScaleStep(_ptr, value); }
     }
     
     /// <summary>
     /// Median filter kernel size (1 = no filter) (3 or 5)
     /// </summary>
     public int MedianFiltering
     {
        get { return CvInvoke.cveDualTVL1OpticalFlowGetMedianFiltering(_ptr); } 
        set { CvInvoke.cveDualTVL1OpticalFlowSetMedianFiltering(_ptr, value); }
     }
     
   }
}
