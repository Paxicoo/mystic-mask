using System;
using System.Windows.Forms;
using System.IO;
using NAudio.Wave;
using System.Security.Cryptography;
using System.Text;

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
            audioFile = new AudioFileReader(@"Resources/moose.wav") { Volume = 0.1F }; // 0.1F = 10% volume
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

        private byte[] Encrypt(byte[] data, string passphrase)
        {
            byte[] key, iv;
            GenerateEncryptionKeyIV(passphrase, out key, out iv);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    return PerformCryptography(data, encryptor);
                }
            }
        }

        private byte[] EncryptWithMetadata(byte[] data, string passphrase, string colorMetadata)
        {
            // Convert the color metadata to byte array
            byte[] metadataBytes = Encoding.UTF8.GetBytes(colorMetadata);
            byte[] combinedData = new byte[metadataBytes.Length + data.Length];

            // Copy metadata and file data to combinedData
            Buffer.BlockCopy(metadataBytes, 0, combinedData, 0, metadataBytes.Length);
            Buffer.BlockCopy(data, 0, combinedData, metadataBytes.Length, data.Length);

            // Encrypt combinedData instead of just data
            return Encrypt(combinedData, passphrase);
        }

        private void GenerateEncryptionKeyIV(string passphrase, out byte[] key, out byte[] iv)
        {
            // Use a key derivation function to generate a key and IV from the passphrase
            var keyGenerator = new Rfc2898DeriveBytes(passphrase, Encoding.UTF8.GetBytes("YourSaltHere"));
            key = keyGenerator.GetBytes(32); // AES 256-bit key
            iv = keyGenerator.GetBytes(16);  // AES block size is 128-bit
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                string welcomeMessage = "Select the target file to create a new Rosa";
                MessageBox.Show(welcomeMessage, "Initiate MysticMask Protocol 🚀", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    byte[] fileContent = File.ReadAllBytes(selectedFilePath);

                    string passphrase = checkBox1.Checked ? PromptForPassphrase() : "defaultPassphrase";
                    if (checkBox1.Checked && string.IsNullOrEmpty(passphrase))
                    {
                        MessageBox.Show("Oops! We need a passphrase to secure it. 🔑", "Passphrase Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    byte[] encryptedBytes = Encrypt(fileContent, passphrase);

                    // Display the GhostColorForm as a modal dialog
                    GhostColorForm ghostColorForm = new GhostColorForm();
                    var dialogResult = ghostColorForm.ShowDialog();

                    // Proceed only if the dialog was closed with a positive result
                    if (dialogResult == DialogResult.OK)
                    {
                        string selectedColor = ghostColorForm.SelectedColorName;
                        string ghostName = ghostColorForm.GhostName;

                        // Combine color metadata with file content
                        string colorMetadata = "color:" + selectedColor + ";";
                        byte[] encryptedBytesWithMetadata = EncryptWithMetadata(fileContent, passphrase, colorMetadata);

                        // Check if a valid ghost name was provided
                        if (string.IsNullOrWhiteSpace(ghostName))
                        {
                            MessageBox.Show("No name was provided for the Rosa. Please try again.", "Name Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.FileName = ghostName + ".Rosa"; // Set default file name
                        saveFileDialog.Filter = "MysticMask Encrypted Files (*.Rosa)|*.Rosa";
                        saveFileDialog.FilterIndex = 1;
                        saveFileDialog.RestoreDirectory = true;

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            string saveFilePath = saveFileDialog.FileName;

                            try
                            {
                                File.WriteAllBytes(saveFilePath, encryptedBytes);
                                MessageBox.Show("Success! Your file has been transformed into a 'Rosa'.\n\n" +
                                                "Now known as '" + Path.GetFileNameWithoutExtension(saveFilePath) + "', it will securely guard your secrets.\n\n" +
                                                "Keep it safe and secure. 🔐", "Encryption Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to save the Rosa file: " + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Rosa creation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show($"Encryption failed: {ex.Message} ⚠️", "Encryption Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message} ⚠️", "Error Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string PromptForPassphrase()
        {
            // Use `Microsoft.VisualBasic.Interaction.InputBox` for passphrase input
            return Microsoft.VisualBasic.Interaction.InputBox("Please enter a passphrase for encryption:", "Passphrase Required", "");
        }


    }
}

