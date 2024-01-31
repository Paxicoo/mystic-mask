using System; 
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;

namespace MysticMask
{
    public partial class MainForm : Form
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private bool isMusicPlaying = true; // for checking if music is playing


        public MainForm()
        {
            InitializeComponent();
            InitializeMusic(); // initialize music
        }

        private void InitializeMusic()
        {
            // Dispose previous instances if they exist
            outputDevice?.Dispose();
            audioFile?.Dispose();

            // Initialize NAudio components
            outputDevice = new WaveOutEvent();
            audioFile = new AudioFileReader(@"Resources/moose.wav") { Volume = 0.2F }; // 0.2F = 20% volume
            outputDevice.Init(audioFile);

            // Play the music
            PlayMusic();
        }

        private void PlayMusic()
        {
            if (isMusicPlaying)
            {
                outputDevice.Play();
            }
            else
            {
                outputDevice.Stop();
            }
        }

        private void MusicIcon_Click(object sender, EventArgs e)
        {
            // Toggle music on/off
            isMusicPlaying = !isMusicPlaying;

            // Update icon and play/pause music accordingly
            if (isMusicPlaying)
            {
                MusicIcon.Image = Properties.Resources.VolumeOnIcon;
                PlayMusic();
            }
            else
            {
                MusicIcon.Image = Properties.Resources.VolumeOffIcon;
                PlayMusic();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // we need a filter that only accepts .txt files to then encrypt into .rosa files
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string selectedFilePath = openFileDialog.FileName;

                    // Read the contents of the selected file
                    byte[] textFile = File.ReadAllBytes(selectedFilePath);

                    // Encrypt the file contents (need to replace this with the encryption logic)
                    byte[] encryptedBytes = Encrypt(textFile);

                    // Prompt the user to choose the location to save the encrypted file
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "MysticMask Encrypted Files (*.rosa)|*.rosa";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.RestoreDirectory = true;

                    saveFileDialog.FileName = selectedFilePath;

                   
                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        string saveFilePath = saveFileDialog.FileName;

                        

                        // Save the encrypted data to the selected location
                        //File.WriteAllBytes(saveFilePath, encryptedBytes);


                        //MessageBox.Show("File encrypted and saved successfully!", "Encryption Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Encryption Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Replace this method with your encryption logic
        private byte[] Encrypt(byte[] data)
        {
            // Encryption function yet to be implemented
            return data;
        }



}
}

