using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using UnityAsync;

using UnityEngine;
using UnityEngine.UI;

namespace WebGLFix
{
	[RequireComponent(typeof(Button))]
	public class WebGLRunner : MonoBehaviour
	{
		private const string WEBSERVER_PROGRAM_FILE = "webserver_prog.loc";
		private const string WEBSERVER_DIRECTORY = @"C:\Program Files\Unity\Hub\Editor\2020.3.5f1\Editor\Data\PlaybackEngines\WebGLSupport\BuildTools\";

		private Process serverProcess;

		private void Awake()
		{
			Button button = gameObject.GetComponent<Button>();
			button.onClick.AddListener(RunWebServer);

			Application.quitting += () => serverProcess?.Close();
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
				Application.Quit();
		}

		private async void RunWebServer() => await Run();

		private async Task Run()
		{
			ValidateServerFile();

			await new WaitForEndOfFrame();
			
			serverProcess?.Close();
			
			OpenFileInfo ofn = new OpenFileInfo();
			ofn.structSize = Marshal.SizeOf(ofn);
			ofn.filter = "All Files\0*.*\0\0";
			ofn.file = new string(new char[256]);
			ofn.maxFile = ofn.file.Length;
			ofn.fileTitle = new string(new char[64]);
			ofn.maxFileTitle = ofn.fileTitle.Length;
			ofn.initialDir = ".\\..\\";
			ofn.title = "Locate Web GL Build";
			ofn.defExt = "HTML";
			// ReSharper disable all CommentTypo
			ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008; //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
			if(OpenFileDialogue.GetOpenFileName(ofn))
			{
				ProcessStartInfo startInfo = new ProcessStartInfo
				{
					CreateNoWindow = false,
					UseShellExecute = true,
					FileName = File.ReadAllText(Path.Combine(".\\", WEBSERVER_PROGRAM_FILE)),
					WindowStyle = ProcessWindowStyle.Normal,
					Arguments = $"\"{ofn.file.Replace("\\index.html", "")}\" 8000"
				};

				serverProcess = new Process();
				serverProcess.StartInfo = startInfo;
				serverProcess.Start();

				await new WaitForEndOfFrame();

				Application.OpenURL("http://localhost:8000/");
			}
		}

		private void ValidateServerFile()
		{
			if(!File.Exists(Path.Combine(".\\", WEBSERVER_PROGRAM_FILE)))
			{
				OpenFileInfo ofn = new OpenFileInfo();
				ofn.structSize = Marshal.SizeOf(ofn);
				ofn.filter = ".exe";
				ofn.file = new string(new char[256]);
				ofn.maxFile = ofn.file.Length;
				ofn.fileTitle = new string(new char[64]);
				ofn.maxFileTitle = ofn.fileTitle.Length;
				ofn.initialDir = WEBSERVER_DIRECTORY;
				ofn.title = "Locate WebServer";
				ofn.defExt = "exe";
				// ReSharper disable all CommentTypo
				ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008; //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
				if(OpenFileDialogue.GetOpenFileName(ofn))
				{
					File.WriteAllText(Path.Combine(".\\", WEBSERVER_PROGRAM_FILE), ofn.file);
				}
			}
		}
	}
}