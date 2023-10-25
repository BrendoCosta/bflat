using System;
using System.Runtime.InteropServices;
using DriverExample.Win32;

namespace DriverExample
{
    public static class DriverExample
    {

        [UnmanagedCallersOnly(EntryPoint = "DriverStartio")]
        public unsafe static void DriverStartio(DriverObject* driverObject, Irp* irp)
        {
            Console.WriteLine("Starting driver IO...");

            irp->IoStatus.Status = NtStatus.STATUS_SUCCESS;
            irp->IoStatus.Information = null;
            
            IofCompleteRequest(irp, PriorityBoost.IO_NO_INCREMENT);

        }

        [UnmanagedCallersOnly(EntryPoint = "DriverUnload")]
        public unsafe static void DriverUnload(DriverObject* driverObject)
        {
            Console.WriteLine("Unloading driver...");
            
            if (driverObject->DeviceObject != null)
            {
                IoDeleteDevice(driverObject->DeviceObject);
            }

            return;
        }

        [UnmanagedCallersOnly(EntryPoint = "DeviceCreateIrpDispatchCallback")]
        public unsafe static NtStatus DeviceCreateIrpDispatchCallback(DriverObject* driverObject, Irp* irp)
        {
            Console.WriteLine("Some userland process is calling the driver through CreateFile call");
            
            irp->IoStatus.Status = NtStatus.STATUS_SUCCESS;
            irp->IoStatus.Information = null;
            
            IofCompleteRequest(irp, PriorityBoost.IO_NO_INCREMENT);

            return NtStatus.STATUS_SUCCESS;
        }

        [UnmanagedCallersOnly(EntryPoint = "DeviceCloseIrpDispatchCallback")]
        public unsafe static NtStatus DeviceCloseIrpDispatchCallback(DriverObject* driverObject, Irp* irp)
        {
            Console.WriteLine("Some userland process is closing the device handle");

            irp->IoStatus.Status = NtStatus.STATUS_SUCCESS;
            irp->IoStatus.Information = null;
            
            IofCompleteRequest(irp, PriorityBoost.IO_NO_INCREMENT);

            return NtStatus.STATUS_SUCCESS;
        }

        [UnmanagedCallersOnly(EntryPoint = "DeviceControlIrpDispatchCallback")]
        public unsafe static NtStatus DeviceControlIrpDispatchCallback(DriverObject* driverObject, Irp* irp)
        {
            Console.WriteLine("The driver received a control code");

            irp->IoStatus.Status = NtStatus.STATUS_SUCCESS;
            irp->IoStatus.Information = null;
            
            IofCompleteRequest(irp, PriorityBoost.IO_NO_INCREMENT);

            return NtStatus.STATUS_SUCCESS;
        }

        /*
         * ntoskrnl.exe p-invokes
        */

        [DllImport("ntoskrnl.exe", EntryPoint="IoCreateDevice")]
        public extern unsafe static NtStatus IoCreateDevice(
            DriverObject*  DriverObject,
            ulong DeviceExtensionSize,
            UnicodeString* DeviceName,
            DeviceType DeviceType,
            ulong DeviceCharacteristics,
            int Exclusive,
            DeviceObject* DeviceObject
        );

        [DllImport("ntoskrnl.exe", EntryPoint="IoCreateSymbolicLink")]
        public extern unsafe static NtStatus IoCreateSymbolicLink(UnicodeString* SymbolicLinkName, UnicodeString* DeviceName);

        [DllImport("ntoskrnl.exe", EntryPoint="IoDeleteSymbolicLink")]
        public extern unsafe static NtStatus IoDeleteSymbolicLink(UnicodeString* SymbolicLinkName);

        [DllImport("ntoskrnl.exe", EntryPoint="IoDeleteDevice")]
        public extern unsafe static void IoDeleteDevice(DeviceObject* DeviceObject);

        [DllImport("ntoskrnl.exe", EntryPoint="IofCompleteRequest")]
        public extern unsafe static void IofCompleteRequest(Irp* irp, sbyte priorityBoost);

        /*
         * Driver entry point
        */

        [UnmanagedCallersOnly(EntryPoint = "DriverEntry")]
        public unsafe static NtStatus DriverEntry(DriverObject* driverObject, UnicodeString* registryPath)
        {
            // ./bflat build Driver.cs Win32.cs --stdlib:zero --os:windowskernel -o:CSharpDriver.sys
            
            Console.WriteLine("Hello world from C#!");

            NtStatus status = NtStatus.STATUS_DRIVER_UNABLE_TO_LOAD;
            const string NT_DEVICE_NAME = "\\Device\\CSharpDriver\0";
            const string DOS_DEVICE_NAME = "\\DosDevices\\CSharpDriver\0";
            fixed(char* pDRIVER_DEVICE_PATH = NT_DEVICE_NAME)
            fixed(char* pDRIVER_SYMLINK_PATH = DOS_DEVICE_NAME)
            {
                UnicodeString driverDeviceName = new UnicodeString()
                {
                    Length = (ushort) (NT_DEVICE_NAME.Length * 2),
                    MaximumLength = (ushort) (NT_DEVICE_NAME.Length * 2 + 2),
                    Buffer = pDRIVER_DEVICE_PATH
                };
                
                UnicodeString driverSymlinkPath = new UnicodeString()
                {
                    Length = (ushort) (DOS_DEVICE_NAME.Length * 2),
                    MaximumLength = (ushort) (DOS_DEVICE_NAME.Length * 2 + 2),
                    Buffer = pDRIVER_SYMLINK_PATH
                };

                DeviceObject deviceObject = new DeviceObject();

                status = IoCreateDevice(driverObject, 0, &driverDeviceName, DeviceType.FILE_DEVICE_UNKNOWN, 0, 1, &deviceObject);
                if (status != NtStatus.STATUS_SUCCESS)
                {
                    Console.WriteLine("Failed to create the device object");
                    return status;
                }

                status = IoDeleteSymbolicLink(&driverSymlinkPath);
                status = IoCreateSymbolicLink(&driverSymlinkPath, &driverDeviceName);
                if (status != NtStatus.STATUS_SUCCESS)
                {
                    Console.WriteLine("Failed to create the symbolic link");
                    IoDeleteDevice(&deviceObject);
                    return status;
                }
            }
            
            driverObject->MajorFunction[IrpMajorFunctionCode.IRP_MJ_CREATE] = &DeviceCreateIrpDispatchCallback;
            driverObject->MajorFunction[IrpMajorFunctionCode.IRP_MJ_CLOSE] = &DeviceCloseIrpDispatchCallback;
            driverObject->MajorFunction[IrpMajorFunctionCode.IRP_MJ_DEVICE_CONTROL] = &DeviceControlIrpDispatchCallback;
            driverObject->DriverStartIo = &DriverStartio;
            driverObject->DriverUnload = &DriverUnload;

            // var someFunctionPointer = (delegate* unmanaged<DriverObject*, Irp*, NtStatus>) driverObject->MajorFunction[...];
            // someFunctionPointer(driverObject, registryPath);
            
            Console.WriteLine("Successfully initialized the driver");
            return NtStatus.STATUS_SUCCESS;
        }
    }
}