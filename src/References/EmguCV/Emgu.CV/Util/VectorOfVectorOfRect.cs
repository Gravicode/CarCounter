﻿//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//  Vector of VectorOfRect
//
//  This file is automatically generated, do not modify.
//----------------------------------------------------------------------------



using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wrapped class of the C++ standard vector of VectorOfRect.
   /// </summary>
   [Serializable]
   [DebuggerTypeProxy(typeof(VectorOfVectorOfRect.DebuggerProxy))]
   public partial class VectorOfVectorOfRect : Emgu.CV.Util.UnmanagedVector
#if true
      , IInputOutputArray
#endif
   {
      private readonly bool _needDispose;
   
      static VectorOfVectorOfRect()
      {
         CvInvoke.Init();
      }

      /// <summary>
      /// Create an empty standard vector of VectorOfRect
      /// </summary>
      public VectorOfVectorOfRect()
         : this(VectorOfVectorOfRectCreate(), true)
      {
      }

      internal VectorOfVectorOfRect(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
      }

      /// <summary>
      /// Create an standard vector of VectorOfRect of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfVectorOfRect(int size)
         : this( VectorOfVectorOfRectCreateSize(size), true)
      {
      }

      /// <summary>
      /// Create an standard vector of VectorOfRect with the initial values
      /// </summary>
      /// <param name="values">The initial values</param>
      public VectorOfVectorOfRect(params VectorOfRect[] values)
        : this()
      {
        Push(values);
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public override int Size
      {
         get
         {
            return VectorOfVectorOfRectGetSize(_ptr);
         }
      }
	  
	  /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public override IntPtr StartAddress
      {
         get
         {
            return VectorOfVectorOfRectGetStartAddress(_ptr);
         }
      }
	  
	  /// <summary>
      /// The pointer to memory address at the end of the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public override IntPtr EndAddress
      {
         get
         {
            return VectorOfVectorOfRectGetEndAddress(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         VectorOfVectorOfRectClear(_ptr);
      }

      /// <summary>
      /// Push a value into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(VectorOfRect value)
      {
         VectorOfVectorOfRectPush(_ptr, value.Ptr);
      }

      /// <summary>
      /// Push multiple values into the standard vector
      /// </summary>
      /// <param name="values">The values to be pushed to the vector</param>
      public void Push(VectorOfRect[] values)
      {
         foreach (VectorOfRect value in values)
            Push(value);
      }

      /// <summary>
      /// Push multiple values from the other vector into this vector
      /// </summary>
      /// <param name="other">The other vector, from which the values will be pushed to the current vector</param>
      public void Push(VectorOfVectorOfRect other)
      {
         VectorOfVectorOfRectPushVector(_ptr, other);
      }
      
      /// <summary>
      /// Get the item in the specific index
      /// </summary>
      /// <param name="index">The index</param>
      /// <returns>The item in the specific index</returns>
      public VectorOfRect this[int index]
      {
         get
         {
            IntPtr itemPtr = IntPtr.Zero;
            VectorOfVectorOfRectGetItemPtr(_ptr, index, ref itemPtr);
            return new VectorOfRect(itemPtr, false);
         }
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            VectorOfVectorOfRectRelease(ref _ptr);
      }

#if true
      /// <summary>
      /// Get the pointer to cv::_InputArray
      /// </summary>
      /// <returns>The input array</returns>
      public InputArray GetInputArray()
      {
        return new InputArray( cveInputArrayFromVectorOfVectorOfRect(_ptr), this );
      }

      /// <summary>
      /// Get the pointer to cv::_OutputArray
      /// </summary>
      /// <returns>The output array</returns>
      public OutputArray GetOutputArray()
      {
         return new OutputArray( cveOutputArrayFromVectorOfVectorOfRect(_ptr), this );
      }

      /// <summary>
      /// Get the pointer to cv::_InputOutputArray
      /// </summary>
      /// <returns>The input output array</returns>
      public InputOutputArray GetInputOutputArray()
      {
         return new InputOutputArray( cveInputOutputArrayFromVectorOfVectorOfRect(_ptr), this );
      }
#endif
      
      /// <summary>
      /// The size of the item in this Vector, counted as size in bytes.
      /// </summary>
      public static int SizeOfItemInBytes
      {
         get { return VectorOfVectorOfRectSizeOfItemInBytes(); }
      }

#if true
      /// <summary>
      /// Create the standard vector of VectorOfRect 
      /// </summary>
      /// <param name="values">The values to be pushed to the vector</param>
      public VectorOfVectorOfRect(Rectangle[][] values)
         : this()
      {
         using (VectorOfRect v = new VectorOfRect())
         {
            for (int i = 0; i < values.Length; i++)
            {
               v.Push(values[i]);
               Push(v);
               v.Clear();
            }
         }
      }

      /// <summary>
      /// Convert the standard vector to arrays of arrays of Rectangle
      /// </summary>
      /// <returns>Arrays of arrays of the Rectangle</returns>
      public Rectangle[][] ToArrayOfArray()
      {
         int size = Size;
         Rectangle[][] res = new Rectangle[size][];
         for (int i = 0; i < size; i++)
         {
            using (VectorOfRect v = this[i])
            {
               res[i] = v.ToArray();
            }
         }
         return res;
      }
#endif

      internal class DebuggerProxy
      {
         private VectorOfVectorOfRect _v;

         public DebuggerProxy(VectorOfVectorOfRect v)
         {
            _v = v;
         }

#if true
         public Rectangle[][] Values
         {
            get { return _v.ToArrayOfArray(); }
         }
#else
         public VectorOfRect[] Values
         {
            get
            {
               VectorOfRect[] result = new VectorOfRect[_v.Size];
               for (int i = 0; i < result.Length; i++)
               {
                  result[i] = _v[i];
               }
               return result;
            }
         }
#endif
      }


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfRectCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfRectCreateSize(int size);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfRectRelease(ref IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfVectorOfRectGetSize(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfRectGetStartAddress(IntPtr v);
	  
	  [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfVectorOfRectGetEndAddress(IntPtr v);
	  
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfRectPush(IntPtr v, IntPtr value);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfRectPushVector(IntPtr ptr, IntPtr otherPtr);
      
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfRectClear(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfVectorOfRectGetItemPtr(IntPtr vec, int index, ref IntPtr element);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfVectorOfRectSizeOfItemInBytes();

#if true
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveInputArrayFromVectorOfVectorOfRect(IntPtr vec);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveOutputArrayFromVectorOfVectorOfRect(IntPtr vec);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveInputOutputArrayFromVectorOfVectorOfRect(IntPtr vec);
#endif
   }
}


