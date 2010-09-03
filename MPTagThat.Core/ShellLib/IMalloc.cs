#region Copyright (C) 2009-2010 Team MediaPortal

// Copyright (C) 2009-2010 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MPTagThat is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MPTagThat is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MPTagThat. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region

using System;
using System.Runtime.InteropServices;

#endregion

namespace MPTagThat.Core.ShellLib
{
  [ComImport]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("00000002-0000-0000-C000-000000000046")]
  public interface IMalloc
  {
    // Allocates a block of memory.
    // Return value: a pointer to the allocated memory block.
    [PreserveSig]
    IntPtr Alloc(
      UInt32 cb);

    // Size, in bytes, of the memory block to be allocated. 

    // Changes the size of a previously allocated memory block.
    // Return value:  Reallocated memory block 
    [PreserveSig]
    IntPtr Realloc(
      IntPtr pv, // Pointer to the memory block to be reallocated.
      UInt32 cb);

    // Size of the memory block (in bytes) to be reallocated.

    // Frees a previously allocated block of memory.
    [PreserveSig]
    void Free(
      IntPtr pv);

    // Pointer to the memory block to be freed.

    // This method returns the size (in bytes) of a memory block previously allocated with 
    // IMalloc::Alloc or IMalloc::Realloc.
    // Return value: The size of the allocated memory block in bytes 
    [PreserveSig]
    UInt32 GetSize(
      IntPtr pv);

    // Pointer to the memory block for which the size is requested.

    // This method determines whether this allocator was used to allocate the specified block of memory.
    // Return value: 1 - allocated 0 - not allocated by this IMalloc instance. 
    [PreserveSig]
    Int16 DidAlloc(
      IntPtr pv);

    // Pointer to the memory block

    // This method minimizes the heap as much as possible by releasing unused memory to the operating system, 
    // coalescing adjacent free blocks and committing free pages.
    [PreserveSig]
    void HeapMinimize();
  }
}