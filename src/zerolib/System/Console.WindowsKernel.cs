// bflat minimal runtime library
// Copyright (C) 2021-2022 Michal Strehovsky
// Copyright (C) 2023 Brendo Costa
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

#if WINDOWSKERNEL

using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System
{
    public static unsafe partial class Console
    {
        static bool isTextBufferAllocated = false;
        static readonly int DBGPRINT_BUFFER_MAX = 512;
        static int bufferPos = 0;
        static readonly int TEXT_BUFFER_SIZE = DBGPRINT_BUFFER_MAX / sizeof(char);
        static byte* textBuffer = null;

        [DllImport("ntoskrnl.exe", EntryPoint = "DbgPrint")]
        public static extern ulong DbgPrint(byte* format, void* buffer);

        public static unsafe void Write(char c)
        {
            if (!isTextBufferAllocated) {
                fixed (byte* tb = new byte[TEXT_BUFFER_SIZE])
                {
                    textBuffer = tb;
                }
                ClearTextBuffer();
                isTextBufferAllocated = true;        
            }
            
            bufferPos = bufferPos % (DBGPRINT_BUFFER_MAX - 1);

            if (bufferPos > 0 && *((char*)(textBuffer + bufferPos - 1)) == '\r' && c == '\n')
            {
                bufferPos = 0;
                // TODO: Replace this with C# 11 UTF-8 string literals.
                byte* fb = stackalloc byte[3] {
                    0x25, // %
                    0x73, // s
                    0x00  // NULL
                };
                DbgPrint(fb, (void*) textBuffer);
                ClearTextBuffer();
            }
            else
            {
                textBuffer[bufferPos % (DBGPRINT_BUFFER_MAX - 1)] = (byte) c;
                bufferPos++;
            }
        }

        private static unsafe void ClearTextBuffer()
        {
            if (textBuffer != null)
            {
                for (int i = 0; i < TEXT_BUFFER_SIZE; i++)
                {
                    textBuffer[i] = (byte) 0x00;
                }
            }
        }

    }
}

#endif