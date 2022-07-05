//This file is automatically generated by CMAKE. DO NOT MODIFY.
using System;
using System.Collections.Generic;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      /// <summary>
      /// The file name of the cvextern library
      /// </summary>
#if (__IOS__ || UNITY_IPHONE || UNITY_WEBGL) && (!UNITY_EDITOR)
      public const string ExternLibrary = "__Internal";
#else
      public const string ExternLibrary = "cvextern";
#endif
	  
      /// <summary>
      /// The file name of the cvextern library
      /// </summary>
      public const string ExternCudaLibrary = ExternLibrary;

      /// <summary>
      /// The file name of the opencv_ffmpeg library
      /// </summary>
      public const string OpencvFFMpegLibrary = "opencv_videoio_ffmpeg455_64";

       
      /// <summary>
      /// The file name of the img_hash library
      /// </summary>
      #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
      public const string OpencvImg_hashLibrary = "opencv_img_hash455";
      #elif UNITY_EDITOR_OSX
      public const string OpencvImg_hashLibrary = "Assets/Plugins/emgucv.bundle/Contents/MacOS/libopencv_img_hash.4.5.5.dylib";   
      #elif UNITY_STANDALONE_OSX
      public const string OpencvImg_hashLibrary = "@executable_path/../Plugins/emgucv.bundle/Contents/MacOS/libopencv_img_hash.4.5.5.dylib";   
      #elif __IOS__ || UNITY_IPHONE || UNITY_WEBGL
      public const string OpencvImg_hashLibrary = "__Internal";
      #elif __ANDROID__ || UNITY_ANDROID
      public const string OpencvImg_hashLibrary = "opencv_img_hash";
      #else
      public const string OpencvImg_hashLibrary = "opencv_img_hash455";
      #endif
 
      /// <summary>
      /// The file name of the world library
      /// </summary>
      #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
      public const string OpencvWorldLibrary = "opencv_world455";
      #elif UNITY_EDITOR_OSX
      public const string OpencvWorldLibrary = "Assets/Plugins/emgucv.bundle/Contents/MacOS/libopencv_world.4.5.5.dylib";   
      #elif UNITY_STANDALONE_OSX
      public const string OpencvWorldLibrary = "@executable_path/../Plugins/emgucv.bundle/Contents/MacOS/libopencv_world.4.5.5.dylib";   
      #elif __IOS__ || UNITY_IPHONE || UNITY_WEBGL
      public const string OpencvWorldLibrary = "__Internal";
      #elif __ANDROID__ || UNITY_ANDROID
      public const string OpencvWorldLibrary = "opencv_world";
      #else
      public const string OpencvWorldLibrary = "opencv_world455";
      #endif

	  
      /// <summary>
      /// The List of the opencv modules
      /// </summary>
	  public static List<String> OpenCVModuleList = new List<String>
	  {
#if !(__ANDROID__ || __IOS__ || UNITY_IPHONE  || UNITY_WEBGL || UNITY_ANDROID || NETFX_CORE)
        OpencvFFMpegLibrary,
#endif        
        
      OpencvImg_hashLibrary,
      OpencvWorldLibrary,
        ExternLibrary
      };

	  
   }
}
