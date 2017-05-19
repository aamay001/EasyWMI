/*************************************************************************************************************************
** Object: WMI_ALIAS Class
** Name: EasyWMI.WMI_ALIAS.cs
** Date: 5/18/2017
** Author: Andy Amaya

License: EasyWMI

Copyright © 2017 Andy S. Amaya

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.				 

** Edit Date		Edited By		Changes
** 5/18/2017		Andy Amaya		Inital Version
*************************************************************************************************************************/
using System;

namespace EasyWMI
{
    /// <summary>
    /// Alias object used for storing WMI_ALIAS values. 
    /// </summary>
    public class Alias
    {
        public Alias()
        {
            _value = "";
        }

        /// <summary>
        /// Constructor with string value paramter.
        /// </summary>
        /// <param name="s">String to use as alias value.</param>
        public Alias(String s)
        {
            _value = s;
        }

        public String Value
        {
            get
            {
                return _value;
            }
        }
        private String _value;
        
        /// <summary>
        /// To string method override to return value property.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _value;
        }
        
        /// <summary>
        /// Equals method override for equality checking.
        /// </summary>
        /// <param name="obj">Object to check equality against.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Alias a = obj as Alias;
            if ((Object)a == null)
                return false;

            return _value == a._value;
        }
        
        /// <summary>
        /// Required for Equals method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Equal operator override for equality checking.
        /// </summary>
        /// <param name="a">Alias on left.</param>
        /// <param name="b">Alias on right.</param>
        /// <returns></returns>
        public static bool operator ==(Alias a, Alias b)
        {
            if (Object.ReferenceEquals(a,b))
            {
                return true;
            }

            if ((Object)a == null || (Object)b == null)
            {
                return false;
            }

            return a._value == b._value;
        }

        /// <summary>
        /// Not Equals operator override for equality checking.
        /// </summary>
        /// <param name="a">Alias on left.</param>
        /// <param name="b">Alias on righ.</param>
        /// <returns></returns>
        public static bool operator !=(Alias a, Alias b)
        {
            return !(a._value == b._value);
        }
    }

    /// <summary>
    /// WMI aliases for requesting information.
    /// </summary>
    public static class WMI_ALIAS
    {
        public static Alias NONE = new Alias("");
        public static Alias BASEBOARD = new Alias( "baseboard");
        public static Alias BIOS = new Alias( "bios");
        public static Alias BOOT_CONFIG = new Alias( "bootconfig");
        public static Alias CDROM = new Alias( "cdrom");
        public static Alias COMPUTER_SYSTEM = new Alias( "computersystem");
        public static Alias CPU = new Alias( "cpu");
        public static Alias DATAFILE = new Alias( "datafile");
        public static Alias DCOMAPP = new Alias( "dcomapp");
        public static Alias DESKTOP = new Alias( "desktop");
        public static Alias DESKTOP_MONITOR = new Alias( "desktopmonitor");
        public static Alias DISK_DRIVE = new Alias( "diskdrive");
        public static Alias DISK_QUOTA = new Alias( "diskquota");
        public static Alias ENVIRONMENT = new Alias( "environment");
        public static Alias FSDIR = new Alias( "fsdir");
        public static Alias GROUP = new Alias( "group");
        public static Alias IDE_CONTROLLER = new Alias( "idecontroller");
        public static Alias IRQ = new Alias( "irq");
        public static Alias JOB = new Alias( "job");
        public static Alias LOAD_ORDER = new Alias( "loadorder");
        public static Alias LOGICAL_DISK = new Alias( "logicaldisk");
        public static Alias MEMORY_CACHE = new Alias( "memcache");
        public static Alias MEMORY_LOGICAL = new Alias( "memlogical");
        public static Alias MEMORY_PHYSICAL = new Alias( "memphysical");
        public static Alias NETCLIENT = new Alias( "netclient");
        public static Alias NETLOGIN = new Alias( "netlogin");
        public static Alias NETPROTOCOL = new Alias( "netprotocol");
        public static Alias NETUSE = new Alias( "netuse");
        public static Alias NETWORK_INTERFACE_CARD = new Alias( "nic");
        public static Alias NETWORK_INTERFACE_CARD_CONFIG = new Alias( "nicconfig");
        public static Alias DOMAIN = new Alias( "ntdomain");
        public static Alias NT_EVENT = new Alias( "ntevent");
        public static Alias INTEGRATED_DEVICES = new Alias( "onboarddevice");
        public static Alias OPERATING_SYSTEM = new Alias( "os");
        public static Alias PAGEFILE = new Alias( "pagefile");
        public static Alias PAGEFILE_SET = new Alias( "pagefileset");
        public static Alias PARTITION = new Alias( "partition");
        public static Alias PRINTER = new Alias( "printer");
        public static Alias PRINTJOB = new Alias( "printjob");
        public static Alias PROCESS = new Alias( "process");
        public static Alias PRODUCT = new Alias( "product");
        public static Alias QFE = new Alias( "qfe");
        public static Alias REGISTRY = new Alias( "registry");
        public static Alias SCSI_CONTROLLER = new Alias( "scsicontroller");
        public static Alias SERVER = new Alias( "server");
        public static Alias SERVICE = new Alias( "service");
        public static Alias SOUND_DEVICE = new Alias( "sounddev");
        public static Alias STARTUP = new Alias( "startup");
        public static Alias SYSTEM_ACCOUNT = new Alias( "sysaccount");
        public static Alias SYSTEM_DRIVER = new Alias( "sysdriver");
        public static Alias SYSTEM_ENCLOSURE = new Alias( "systemenclosure");
        public static Alias SYSTEM_SLOT = new Alias( "systemslot");
        public static Alias TAPE_DRIVE = new Alias( "tapedrive");
        public static Alias TIMEZONE = new Alias( "timezone");
        public static Alias USER_ACCOUNT = new Alias( "useraccount");
        public static Alias MEMORY_CHIP = new Alias( "memorychip");
        public static Alias CSPRODUCT = new Alias("csproduct");
    }
}
