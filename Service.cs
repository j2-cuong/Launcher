
using System.Diagnostics;
using System.Security.Principal;

namespace Launcher
{
    internal class Service
    {
        public static void btn_Start(object sender, EventArgs e)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string fileName = "Main.exe";
            string filePath = Path.Combine(currentDirectory, fileName);

            // Kiểm tra file có tồn tại không
            if (File.Exists(filePath))
            {
                // Mở file .exe
                Process.Start(filePath);
            }
            else
            {
                MessageBox.Show("File không tồn tại, Vui lòng tải lại game");
            }
        }
       
        private void btn_DangKy(object sender, EventArgs e)
        {

        }

        private void btn_mode(object sender, EventArgs e)
        {

        }
    }
}
