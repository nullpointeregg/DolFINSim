using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolFINSim_junuver
{
    public class SaveFile
    {
        private readonly string m_directoryPath = @"C:\DolFINSim\SaveFiles\";
        private readonly DirectoryInfo m_directory;
        private readonly FileInfo m_fileInfo;

        public void UpdateFile()
        {
            m_fileInfo.OpenRead();
        }

        public SaveFile(string _fileName)
        {
            m_directory = Directory.CreateDirectory(m_directoryPath);
            m_fileInfo = m_directory.EnumerateFiles().FirstOrDefault(f => f.Name.Equals(_fileName));
            if (m_fileInfo == null)
            {
                m_fileInfo = new FileInfo($@"{m_directoryPath}{_fileName}");
                m_fileInfo.Create();
            }
        }
    }
}
