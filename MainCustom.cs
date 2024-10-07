using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class MainCustom : Form
    {
        // URL API kiểm tra phiên bản
        static string apiVersionUrl = "https://example.com/api/checkversion";

        // URL tải file cập nhật
        static string updateFileUrl = "https://example.com/api/download/updatefile.zip";

        // Phiên bản hiện tại của ứng dụng
        static string currentVersion = "1.0.0";

        // Tạo hàm kéo form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private bool IsRunAsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public MainCustom()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!IsRunAsAdministrator())
            {
                MessageBox.Show("Vui lòng chạy ứng dụng với quyền quản trị.");
            }
            KillProcesses("main");
            Button btn_Start = new Button();
            Button btn_Close = new Button();
            Button btn_DangKy = new Button();
            Button btn_mode = new Button();
            Button btn_Update = new Button();
            ProgressBar progressBar = new ProgressBar();

            btn_Start.Location = new Point(725, 350);
            btn_Start.Size = new Size(75, 40);
            btn_Start.Text = "Vào Game";
            btn_Start.ForeColor = Color.YellowGreen;
            btn_Start.BackColor = Color.FromArgb(25, 137, 185, 255);
            btn_Start.FlatStyle = FlatStyle.Flat;

            btn_Close.Location = new Point(770, 5);
            btn_Close.Size = new Size(25, 25);
            btn_Close.Text = "X";
            btn_Close.ForeColor = Color.Red;
            btn_Close.BackColor = Color.FromArgb(25, 137, 185, 255);
            btn_Close.FlatStyle = FlatStyle.Flat;

            btn_DangKy.Location = new Point(645, 350);
            btn_DangKy.Size = new Size(75, 40);
            btn_DangKy.Text = "Đăng ký";
            btn_DangKy.ForeColor = Color.YellowGreen;
            btn_DangKy.BackColor = Color.FromArgb(25, 137, 185, 255);
            btn_DangKy.FlatStyle = FlatStyle.Flat;

            btn_mode.Location = new Point(565, 350);
            btn_mode.Size = new Size(75, 40);
            btn_mode.Text = "WinMode";
            btn_mode.ForeColor = Color.YellowGreen;
            btn_mode.BackColor = Color.FromArgb(25, 137, 185, 255);
            btn_mode.FlatStyle = FlatStyle.Flat;

            btn_Update.Location = new Point(485, 350);
            btn_Update.Size = new Size(75, 40);
            btn_Update.Text = "Check Update";
            btn_Update.ForeColor = Color.YellowGreen;
            btn_Update.BackColor = Color.FromArgb(25, 137, 185, 255);
            btn_Update.FlatStyle = FlatStyle.Flat;

            progressBar.Location = new Point(20, 375);
            progressBar.Size = new Size(455, 13);
            progressBar.BackColor = SystemColors.ActiveCaption;


            // Thêm sự kiện Click cho Button
            btn_Start.Click += (sender, e) =>
            {
                if (!IsRunAsAdministrator())
                {
                    MessageBox.Show("Vui lòng chạy ứng dụng với quyền quản trị.");
                } else
                {
                    Service.btn_Start(sender, e);
                }
            };

            // Thêm sự kiện Click cho Button
            btn_Close.Click += (sender, e) =>
            {
                Close();
            };

            // Thêm sự kiện Click cho Button
            btn_DangKy.Click += (sender, e) =>
            {
                if (!IsRunAsAdministrator())
                {
                    MessageBox.Show("Vui lòng chạy ứng dụng với quyền quản trị.");
                }
                else
                {
                    Service.btn_Start(sender, e);
                }
            };

            // Thêm sự kiện Click cho Button
            btn_mode.Click += (sender, e) =>
            {
                if (!IsRunAsAdministrator())
                {
                    MessageBox.Show("Vui lòng chạy ứng dụng với quyền quản trị.");
                }
                else
                {
                    Service.btn_Start(sender, e);
                }
            };

            // Thêm sự kiện Click cho Button
            btn_Update.Click += (sender, e) =>
            {
                if (!IsRunAsAdministrator())
                {
                    MessageBox.Show("Vui lòng chạy ứng dụng với quyền quản trị.");
                }
                else
                {
                    Service.btn_Start(sender, e);
                }
            };

            this.MouseDown += new MouseEventHandler(Form_MouseDown);
            pictureBox1.MouseDown += new MouseEventHandler(Form_MouseDown);
            pictureBox1.Controls.Add(btn_Start);
            pictureBox1.Controls.Add(btn_Close);
            pictureBox1.Controls.Add(btn_DangKy);
            pictureBox1.Controls.Add(btn_mode);
            pictureBox1.Controls.Add(btn_Update);
            pictureBox1.Controls.Add(progressBar);
        }
        // Phương thức giúp kéo form khi bấm giữ chuột
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        // Hàm kiểm tra phiên bản mới từ API
        static async Task<string> CheckForUpdate()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiVersionUrl);
                if (response.IsSuccessStatusCode)
                {
                    // Giả sử API trả về chuỗi phiên bản mới
                    string latestVersion = await response.Content.ReadAsStringAsync();
                    return latestVersion;
                }
                else
                {
                    return currentVersion;
                }
            }
        }

        // Hàm tải file cập nhật từ server
        static async Task DownloadUpdate()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(updateFileUrl);
                if (response.IsSuccessStatusCode)
                {
                    byte[] fileData = await response.Content.ReadAsByteArrayAsync();

                    // Lưu file cập nhật vào thư mục tạm thời
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "update.zip");
                    File.WriteAllBytes(filePath, fileData);
                }
            }
        }

        // Hàm áp dụng cập nhật và ghi đè file hiện tại
        static void ApplyUpdate()
        {
            // Giả sử file update là file zip, bạn có thể giải nén vào thư mục gốc
            string updateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "update.zip");

            // Giải nén và ghi đè file
            if (File.Exists(updateFilePath))
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(updateFilePath, AppDomain.CurrentDomain.BaseDirectory, true);
                Process.Start(AppDomain.CurrentDomain.FriendlyName);
            }
        }

        private void KillProcesses(string processName)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    foreach (var process in processes)
                    {
                        process.Kill(); // Kill tiến trình
                        process.WaitForExit(); // Chờ cho tiến trình kết thúc
                    }
                    MessageBox.Show($"Đã tắt {processes.Length} cửa sổ game để tiến hành Update.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
