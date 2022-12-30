using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DataService.Shared.Helpers.Cmd
{
    /// <summary>
    /// 描述：执行Cmd命令
    /// 作者：leopisces
    /// 创建日期：2022/7/22 13:57:59
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    public class RunCmd
    {
        private Process proc = new Process();
        public RunCmd()
        {
            //proc = new Process();
        }

        public void Exe(string cmd)
        {
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

            //proc.OutputDataReceived += new DataReceivedEventHandler(sortProcess_OutputDataReceived);
            proc.Start();
            StreamWriter cmdWriter = proc.StandardInput;
            proc.BeginOutputReadLine();
            if (!String.IsNullOrEmpty(cmd))
            {
                cmdWriter.WriteLine(cmd);
            }
            cmdWriter.Close();          
            proc.Close();
        }

      
        private void sortProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine(e.Data);
                //this.BeginInvoke(new Action(() => { this.listBox1.Items.Add(e.Data); }));
            }
        }
    }
}
