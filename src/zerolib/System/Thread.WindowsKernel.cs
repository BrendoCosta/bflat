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

using System;
using System.Runtime;
using Internal.Runtime.CompilerHelpers;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System.Threading
{
    public static class Thread
    {
        public enum KPROCESSOR_MODE {
            KernelMode,
            UserMode,
            MaximumMode
        }

        [DllImport("ntoskrnl.exe", EntryPoint = "KeDelayExecutionThread")]
        public static unsafe extern ulong KeDelayExecutionThread(KPROCESSOR_MODE processorMode, bool alertable, long* interval);

        public static unsafe void Sleep(int delayMs)
        {
            long delay = delayMs;
            KeDelayExecutionThread(KPROCESSOR_MODE.KernelMode, false, &delay);
        }
    }
}

#endif
