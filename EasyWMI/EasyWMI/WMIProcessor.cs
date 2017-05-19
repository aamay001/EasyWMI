/*************************************************************************************************************************
** Object: WMIProcessor Class
** Name: EasyWMI.WMIProcessor.cs
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
using System.Diagnostics;
using System.Text;
using System.IO;
using System;

namespace EasyWMI
{
    /// <summary>
    /// WMI query processing class. 
    /// </summary>
    public class WMIProcessor
    {
        private readonly String SHELL_PATH = Environment.GetFolderPath(Environment.SpecialFolder.System);
        private const String SHELL_COMMAND = "cmd.exe";
        private const String WMI_COMMAND = "wmic";
        private const String WMI_GET = "get";
        private const String WMI_FORMAT = "/format:list";

        private Process _task;
        private Alias _request;
        private String _filter;
        private String _nodeName;
        private bool _processRemotely;

        #region Properties

        /// <summary>
        /// Remote host name or IP of device to query. Only used when _processRemotely is true.
        /// </summary>
        public String NodeName
        {
            get
            {
                return _nodeName;
            }

            set
            {
                _nodeName = value;
            }
        }

        public Alias Request
        {
            get
            {
                return _request;
            }

            set
            {
                _request = value;
            }
        }

        /// <summary>
        /// CSV string of properties to request from alias.
        /// </summary>
        public String Filter
        {
            get
            {
                return _filter;
            }
            
            set
            {
                _filter = value;
            }
        }

        /// <summary>
        /// Changes the execution style to remote if set to true. { /node: paramter will be included in request }
        /// </summary>
        public bool RemoteExecute
        {
            get
            {
                return _processRemotely;
            }

            set
            {
                _processRemotely = value;
            }
        }

        #endregion

        /// <summary>
        /// Default constructor. Sets properties to local machine values.
        /// </summary>
        public WMIProcessor()
        {
            _request = WMI_ALIAS.NONE;
            _filter = "";
            _nodeName = Environment.MachineName;
            _processRemotely = false;
            _task = null;
        }

        /// <summary>
        /// WMIProcessor constructor with request and computername
        /// parameter.
        /// </summary>
        /// <param name="request">Request parameter should contain the WMI Alias for the data to return. Use WMI_ALIAS class.</param>
        /// <param name="remote">Switch to set execution style. True = Remote Execution | (default)False = Local Execution</param>
        public WMIProcessor( Alias request, String filter = "", bool remote = false)
        {
            _request = request;
            _filter = filter;
            _nodeName = Environment.MachineName;
            _processRemotely = remote;
            _task = null;
        }

        /// <summary>
        /// WMIProcessor constructor with request and computername
        /// parameter.
        /// </summary>
        /// <param name="request">Request parameter should contain the WMI Alias for the data to return. Use WMI_ALIAS class.</param>
        /// <param name="filter">Filter results with a comma separated list of properties. Empty filter return all data for alias.</param>
        /// <param name="nodeName">Name of remote device to get data for.</param>
        /// <param name="remote">Switch to set execution style. True = Remote Execution | (default)False = Local Execution</param>
        public WMIProcessor( String nodeName, Alias request, String filter = "", bool remote = false )
        {
            _request = request;
            _filter = filter;
            _nodeName = nodeName;
            _processRemotely = remote;
            _task = null;
        }
        
        /// <summary>
        /// Execute the requested task and return the output steam.
        /// </summary>
        /// <returns></returns>
        public String ExecuteRequest()
        {
            if (ValidateTask())
            {
                _task = GetTask();

                try
                {
                    _task.Start();
                    _task.WaitForExit();
                }

                catch (ObjectDisposedException e) { throw e; }
                catch (InvalidOperationException e) { throw e; }
                catch (SystemException e) { throw e; }
                catch (Exception e) { throw e; }

                return GetTaskOutput();
            }

            throw new ArgumentException("Argument(s) missing from task. Request property must be specified.");
        }

        #region Utility Methods

        /// <summary>
        /// Checks all required paramters are valid.
        /// </summary>
        /// <returns></returns>
        private bool ValidateTask()
        {
            if (_request == WMI_ALIAS.NONE)
                return false;

            return true;
        }

        /// <summary>
        /// Builds the task file name parameter.
        /// </summary>
        /// <returns></returns>
        private String GetTaskFileName()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SHELL_PATH);
            sb.Append("\\");
            sb.Append(SHELL_COMMAND);
            return sb.ToString();
        }
        
        /// <summary>
        /// Builds the task arguments paramter. 
        /// </summary>
        /// <returns></returns>
        private String GetTaskArguments()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("/C ");
            sb.Append(WMI_COMMAND);
            sb.Append(" ");
            sb.AppendFormat(_processRemotely ? "/node:{0} " : "", _nodeName);
            sb.Append(_request);            
            sb.Append(_request.Value.Length == 0 ? "" : " ");
            sb.Append(WMI_GET);
            sb.Append(" ");
            sb.Append(_filter);
            sb.Append(_filter.Length == 0 ? "" : " ");
            sb.Append(WMI_FORMAT);
            return sb.ToString();
        }

        /// <summary>
        /// Builds the task object.
        /// </summary>
        /// <returns></returns>
        private Process GetTask()
        {
            ProcessStartInfo pinfo = new ProcessStartInfo();
            pinfo.CreateNoWindow = true;
            pinfo.UseShellExecute = false;
            pinfo.WindowStyle = ProcessWindowStyle.Hidden;
            pinfo.FileName = GetTaskFileName();
            pinfo.WorkingDirectory = SHELL_PATH;
            pinfo.Arguments = GetTaskArguments();
            pinfo.RedirectStandardOutput = true;
            pinfo.RedirectStandardError = true;
            Process t = new Process();
            t.StartInfo = pinfo;
            return t;
        }
        
        /// <summary>
        /// Gets the string content from a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private String GetTaskOutput()
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(_task.StandardOutput.BaseStream))
            {
                sb.Append(sr.ReadToEnd());
            }

            sb.Append(Environment.NewLine);
            
            using ( StreamReader sr = new StreamReader(_task.StandardError.BaseStream))
            {
                sb.Append(sr.ReadToEnd());
            }

            return sb.ToString().Trim();              
        }

        #endregion

#if DEBUG
        public String AssertShellPath()
        {
            return SHELL_PATH;
        }

        public String AssertArguments()
        {
            return GetTaskArguments();
        }
#endif
    }
}
