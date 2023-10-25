using System;
using System.Runtime.InteropServices;

namespace DriverExample.Win32 {

    public enum NtStatus: ulong
    {
        STATUS_SUCCESS = 0x00000000,
        STATUS_DRIVER_UNABLE_TO_LOAD = 0xC000026C,
        STATUS_DRIVER_BLOCKED = 0xC000036C,
        STATUS_DRIVER_INTERNAL_ERROR = 0xC0000183
    }

    public static class IrpMajorFunctionCode
    {
        public const int IRP_MJ_CREATE                   = 0x00;
        public const int IRP_MJ_CREATE_NAMED_PIPE        = 0x01;
        public const int IRP_MJ_CLOSE                    = 0x02;
        public const int IRP_MJ_READ                     = 0x03;
        public const int IRP_MJ_WRITE                    = 0x04;
        public const int IRP_MJ_QUERY_INFORMATION        = 0x05;
        public const int IRP_MJ_SET_INFORMATION          = 0x06;
        public const int IRP_MJ_QUERY_EA                 = 0x07;
        public const int IRP_MJ_SET_EA                   = 0x08;
        public const int IRP_MJ_FLUSH_BUFFERS            = 0x09;
        public const int IRP_MJ_QUERY_VOLUME_INFORMATION = 0x0a;
        public const int IRP_MJ_SET_VOLUME_INFORMATION   = 0x0b;
        public const int IRP_MJ_DIRECTORY_CONTROL        = 0x0c;
        public const int IRP_MJ_FILE_SYSTEM_CONTROL      = 0x0d;
        public const int IRP_MJ_DEVICE_CONTROL           = 0x0e;
        public const int IRP_MJ_INTERNAL_DEVICE_CONTROL  = 0x0f;
        public const int IRP_MJ_SHUTDOWN                 = 0x10;
        public const int IRP_MJ_LOCK_CONTROL             = 0x11;
        public const int IRP_MJ_CLEANUP                  = 0x12;
        public const int IRP_MJ_CREATE_MAILSLOT          = 0x13;
        public const int IRP_MJ_QUERY_SECURITY           = 0x14;
        public const int IRP_MJ_SET_SECURITY             = 0x15;
        public const int IRP_MJ_POWER                    = 0x16;
        public const int IRP_MJ_SYSTEM_CONTROL           = 0x17;
        public const int IRP_MJ_DEVICE_CHANGE            = 0x18;
        public const int IRP_MJ_QUERY_QUOTA              = 0x19;
        public const int IRP_MJ_SET_QUOTA                = 0x1a;
        public const int IRP_MJ_PNP                      = 0x1b;
        public const int IRP_MJ_PNP_POWER                = IRP_MJ_PNP; // Obsolete (?)
        public const int IRP_MJ_MAXIMUM_FUNCTION         = 0x1b;
    }

    public static class PriorityBoost {
        public const sbyte EVENT_INCREMENT         = (sbyte) 1;
        public const sbyte IO_NO_INCREMENT         = (sbyte) 0;
        public const sbyte IO_CD_ROM_INCREMENT     = (sbyte) 1;
        public const sbyte IO_DISK_INCREMENT       = (sbyte) 1;
        public const sbyte IO_KEYBOARD_INCREMENT   = (sbyte) 6;
        public const sbyte IO_MAILSLOT_INCREMENT   = (sbyte) 2;
        public const sbyte IO_MOUSE_INCREMENT      = (sbyte) 6;
        public const sbyte IO_NAMED_PIPE_INCREMENT = (sbyte) 2;
        public const sbyte IO_NETWORK_INCREMENT    = (sbyte) 2;
        public const sbyte IO_PARALLEL_INCREMENT   = (sbyte) 1;
        public const sbyte IO_SERIAL_INCREMENT     = (sbyte) 2;
        public const sbyte IO_SOUND_INCREMENT      = (sbyte) 8;
        public const sbyte IO_VIDEO_INCREMENT      = (sbyte) 1;
        public const sbyte SEMAPHORE_INCREMENT     = (sbyte) 1;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct FsFilterCallbacks
    {
        public ulong SizeOfFsFilterCallbacks;
        public ulong Reserved;
        public long* PreAcquireForSectionSynchronization;
        public void* PostAcquireForSectionSynchronization;
        public long* PreReleaseForSectionSynchronization;
        public void* PostReleaseForSectionSynchronization;
        public long* PreAcquireForCcFlush;
        public void* PostAcquireForCcFlush;
        public long* PreReleaseForCcFlush;
        public void* PostReleaseForCcFlush;
        public long* PreAcquireForModifiedPageWriter;
        public void* PostAcquireForModifiedPageWriter;
        public long* PreReleaseForModifiedPageWriter;
        public void* PostReleaseForModifiedPageWriter;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct IoClientExtension
    {
        public IoClientExtension* NextExtension;
        public void* ClientIdentificationAddress;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct DriverExtension
    {
        public DriverObject* DriverObject;
        public long* AddDevice;
        public ulong Count;
        public UnicodeString ServiceKeyName;
        public IoClientExtension* ClientDriverExtension;
        public FsFilterCallbacks* FsFilterCallbacks;

    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct ListEntry
    {
        public ListEntry* Flink;
        public ListEntry* Blink;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct IoTimer
    {
        public short Type;
        public short TimerFlag;
        public ListEntry TimerList;
        public void* TimerRoutine;
        public void* Context;
        public DeviceObject* DeviceObject;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct Vpb
    {
        public short Type;
        public short Size;
        public ushort Flags;
        public ushort VolumeLabelLength;
        public DeviceObject* DeviceObject;
        public DeviceObject* RealDevice;
        public ulong SerialNumber;
        public ulong ReferenceCount;
        public char* VolumeLabel;
    }

    public enum DeviceType
    {
        FILE_DEVICE_8042_PORT           = 0x00000027,
        FILE_DEVICE_ACPI                = 0x00000032,
        FILE_DEVICE_BATTERY             = 0x00000029,
        FILE_DEVICE_BEEP                = 0x00000001,
        FILE_DEVICE_BUS_EXTENDER        = 0x0000002a,
        FILE_DEVICE_CD_ROM              = 0x00000002,
        FILE_DEVICE_CD_ROM_FILE_SYSTEM  = 0x00000003,
        FILE_DEVICE_CHANGER             = 0x00000030,
        FILE_DEVICE_CONTROLLER          = 0x00000004,
        FILE_DEVICE_DATALINK            = 0x00000005,
        FILE_DEVICE_DFS                 = 0x00000006,
        FILE_DEVICE_DFS_FILE_SYSTEM     = 0x00000035,
        FILE_DEVICE_DFS_VOLUME          = 0x00000036,
        FILE_DEVICE_DISK                = 0x00000007,
        FILE_DEVICE_DISK_FILE_SYSTEM    = 0x00000008,
        FILE_DEVICE_DVD                 = 0x00000033,
        FILE_DEVICE_FILE_SYSTEM         = 0x00000009,
        FILE_DEVICE_FIPS                = 0x0000003a,
        FILE_DEVICE_FULLSCREEN_VIDEO    = 0x00000034,
        FILE_DEVICE_INPORT_PORT         = 0x0000000a,
        FILE_DEVICE_KEYBOARD            = 0x0000000b,
        FILE_DEVICE_KS                  = 0x0000002f,
        FILE_DEVICE_KSEC                = 0x00000039,
        FILE_DEVICE_MAILSLOT            = 0x0000000c,
        FILE_DEVICE_MASS_STORAGE        = 0x0000002d,
        FILE_DEVICE_MIDI_IN             = 0x0000000d,
        FILE_DEVICE_MIDI_OUT            = 0x0000000e,
        FILE_DEVICE_MODEM               = 0x0000002b,
        FILE_DEVICE_MOUSE               = 0x0000000f,
        FILE_DEVICE_MULTI_UNC_PROVIDER  = 0x00000010,
        FILE_DEVICE_NAMED_PIPE          = 0x00000011,
        FILE_DEVICE_NETWORK             = 0x00000012,
        FILE_DEVICE_NETWORK_BROWSER     = 0x00000013,
        FILE_DEVICE_NETWORK_FILE_SYSTEM = 0x00000014,
        FILE_DEVICE_NETWORK_REDIRECTOR  = 0x00000028,
        FILE_DEVICE_NULL                = 0x00000015,
        FILE_DEVICE_PARALLEL_PORT       = 0x00000016,
        FILE_DEVICE_PHYSICAL_NETCARD    = 0x00000017,
        FILE_DEVICE_PRINTER             = 0x00000018,
        FILE_DEVICE_SCANNER             = 0x00000019,
        FILE_DEVICE_SCREEN              = 0x0000001c,
        FILE_DEVICE_SERENUM             = 0x00000037,
        FILE_DEVICE_SERIAL_MOUSE_PORT   = 0x0000001a,
        FILE_DEVICE_SERIAL_PORT         = 0x0000001b,
        FILE_DEVICE_SMARTCARD           = 0x00000031,
        FILE_DEVICE_SMB                 = 0x0000002e,
        FILE_DEVICE_SOUND               = 0x0000001d,
        FILE_DEVICE_STREAMS             = 0x0000001e,
        FILE_DEVICE_TAPE                = 0x0000001f,
        FILE_DEVICE_TAPE_FILE_SYSTEM    = 0x00000020,
        FILE_DEVICE_TERMSRV             = 0x00000038,
        FILE_DEVICE_TRANSPORT           = 0x00000021,
        FILE_DEVICE_UNKNOWN             = 0x00000022,
        FILE_DEVICE_VDM                 = 0x0000002c,
        FILE_DEVICE_VIDEO               = 0x00000023,
        FILE_DEVICE_VIRTUAL_DISK        = 0x00000024,
        FILE_DEVICE_WAVE_IN             = 0x00000025,
        FILE_DEVICE_WAVE_OUT            = 0x00000026
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct KDeviceQueue
    {
        public short Type;
        public short Size;
        public ListEntry DeviceListHead;
        public ulong Lock;
        public byte Busy;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct Kdpc
    {
        public byte Type;
        public byte Importance;
        public ushort Number;
        public ListEntry DpcListEntry;
        public void* DeferredRoutine;
        public void* DeferredContext;
        public void* SystemArgument1;
        public void* SystemArgument2;
        public void* DpcData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct Acl
    {
        public byte AclRevision;
        public byte Sbz1;
        public ushort AclSize;
        public ushort AceCount;
        public ushort Sbz2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct SecurityDescriptor
    {
        public byte Revision;
        public byte Sbz1;
        public ushort Control;
        public void* Owner;
        public void* Group;
        public Acl* Sacl;
        public Acl* Dacl;
    }

    public enum DevicePowerState
    {
        PowerDeviceUnspecified  = 0,
        PowerDeviceD0           = 1,
        PowerDeviceD1           = 2,
        PowerDeviceD2           = 3,
        PowerDeviceD3           = 4,
        PowerDeviceMaximum      = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct PowerChannelSummary
    {
        public ulong Signature;
        public ulong TotalCount;
        public ulong D0Count;
        public ListEntry NotifyList;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct DeviceObjectPowerExtension
    {
        public long IdleCount;
        public ulong ConservationIdleTime;
        public ulong PerformanceIdleTime;
        public DeviceObject* DeviceObject;
        public ListEntry IdleList;
        public byte DeviceType;
        public DevicePowerState State;
        public ListEntry NotifySourceList;
        public ListEntry NotifyTargetList;
        public PowerChannelSummary PowerChannelSummary;
        public ListEntry Volume;
        public ulong PreviousIdleCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct DevObjExtension
    {
        public short Type;
        public ushort Size;
        public DeviceObject* DeviceObject;
        public ulong PowerFlags;
        public DeviceObjectPowerExtension* Dope;
        public ulong ExtensionFlags;
        public void* DeviceNode;
        public DeviceObject* AttachedTo;
        public long StartIoCount;
        public long StartIoKey;
        public ulong StartIoFlags;
        public Vpb* Vpb;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct IoStatusBlock
    {
        public NtStatus Status;
        public void* Pointer;
        public ulong* Information;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct Irp
    {
        public short Type;
        public ushort Size;
        public void* MdlAddress;
        public ulong Flags;
        public Irp* AssociatedIrp_MasterIrp;
        public long AssociatedIrp_IrpCount;
        public void* AssociatedIrp_SystemBuffer;
        public ListEntry ThreadListEntry;
        public IoStatusBlock IoStatus;
        /* ... */
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct DeviceObject
    {
        public short Type;
        public ushort Size;
        public long ReferenceCount;
        public DriverObject* DriverObject;
        public DriverObject* NextDevice;
        public DriverObject* AttachedDevice;
        public void* CurrentIrp; // TODO: fix
        public IoTimer* Timer;
        public ulong Flags;
        public ulong Characteristics;
        public volatile Vpb* Vpb;
        public void* DeviceExtension;
        public DeviceType DeviceType;
        public sbyte StackSize;
        public byte* Queue; // TODO: replace it with a type
        public ulong AlignmentRequirement;
        public KDeviceQueue DeviceQueue;
        public Kdpc Dpc;
        public ulong ActiveThreadCount;
        public SecurityDescriptor* SecurityDescriptor;
        public byte* DeviceLock; // TODO: fix, 42 bytes
        public ushort SectorSize;
        public ushort Spare1;
        public DevObjExtension* DeviceObjectExtension;
        public void* Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct UnicodeString
    {
        public ushort Length;
        public ushort MaximumLength;
        public char* Buffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct FastIoDispatch
    {
        public ulong SizeOfFastIoDispatch;
        public byte* FastIoCheckIfPossible;
        public byte* FastIoRead;
        public byte* FastIoWrite;
        public byte* FastIoQueryBasicInfo;
        public byte* FastIoQueryStandardInfo;
        public byte* FastIoLock;
        public byte* FastIoUnlockSingle;
        public byte* FastIoUnlockAll;
        public byte* FastIoUnlockAllByKey;
        public byte* FastIoDeviceControl;
        public void* AcquireFileForNtCreateSection;
        public void* ReleaseFileForNtCreateSection;
        public void* FastIoDetachDevice;
        public byte* FastIoQueryNetworkOpenInfo;
        public long* AcquireForModWrite;
        public byte* MdlRead;
        public byte* MdlReadComplete;
        public byte* PrepareMdlWrite;
        public byte* MdlWriteComplete;
        public byte* FastIoReadCompressed;
        public byte* FastIoWriteCompressed;
        public byte* MdlReadCompleteCompressed;
        public byte* MdlWriteCompleteCompressed;
        public byte* FastIoQueryOpen;
        public long* ReleaseForModWrite;
        public long* AcquireForCcFlush;
        public long* ReleaseForCcFlush;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe ref struct DriverObject
    {
        /* 0x000 */ public short Type;
        /* 0x002 */ public short Size;
        /* 0x008 */ public DeviceObject* DeviceObject;
        /* 0x010 */ public ulong Flags;
        /* 0x018 */ public void* DriverStart;
        /* 0x020 */ public ulong DriverSize;
        /* 0x028 */ public void* DriverSection;
        /* 0x030 */ public DriverExtension* DriverExtension;
        /* 0x038 */ public UnicodeString DriverName;
        /* 0x048 */ public UnicodeString* HardwareDatabase;
        /* 0x050 */ public FastIoDispatch* FastIoDispatch;
        /* 0x058 */ public delegate* unmanaged<DriverObject*, UnicodeString*, NtStatus> DriverInit;
        /* 0x060 */ public delegate* unmanaged<DriverObject*, Irp*, void> DriverStartIo;
        /* 0x068 */ public delegate* unmanaged<DriverObject*, void> DriverUnload;
        /* 0x070 */ public delegate* unmanaged<DriverObject*, Irp*, NtStatus>* MajorFunction
        {
            get
            {
                fixed (DriverObject* baseAddr = &this)
                {
                    return (delegate* unmanaged<DriverObject*, Irp*, NtStatus>*) (((nuint*) baseAddr) + 0x070);
                }
            }
        }
    }
}